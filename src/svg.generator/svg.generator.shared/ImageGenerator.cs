using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using svg.generator.shared.iOS;

namespace svg.generator.shared
{
	public class ImageGenerator
	{
		private static Regex FormatRegex = new Regex(@"^(?<width>[\d]+)x(?<height>[\d]+)$");

		/// <inheritdoc />
		public ImageGenerator(IGeneratorOptions options, Action<string> loggingCallback)
		{
			_loggingCallback = loggingCallback;
			_options = options;
		}

		private Action<string> _loggingCallback;

		private IGeneratorOptions _options;

		public void Execute()
		{
			if (!Directory.Exists(_options.Source))
			{
				LogIfVerbose($"Source folder \"{_options.Source}\" does not exist.");
				return;
			}

			if (_options.CreateDestinationFolder)
			{
				LogIfVerbose($"Creating destination folder \"{_options.Destination}\".");
				IoHelper.CreateDirectoryRecursive(_options.Destination);
			}
			else
			{
				if (!Directory.Exists(_options.Destination))
				{
					LogIfVerbose($"Destination folder \"{_options.Destination}\" does not exist.");
					return;
				}
			}

			var searchOption = _options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			var sourceFiles = Directory.GetFiles(_options.Source, "*.svg",
				searchOption);

			LogIfVerbose($"Generating files based on {sourceFiles.Length} source files.");

			var formats = _options.Formats.Split(';');
			foreach (var format in formats)
			{
				if (!FormatRegex.IsMatch(format))
				{
					LogIfVerbose($"Invalid format specified: {format}");
					continue;
				}

				var match = FormatRegex.Match(format);

				LogIfVerbose($"Generating images for the format {format}");

				var width = Int32.Parse(match.Groups["width"].Value);
				var height = Int32.Parse(match.Groups["height"].Value);

				var androidFormatParameters = GetAndroidFormatParameters(width, height, _options.Destination, sourceFiles).ToList();
				var iosFormatParameters = GetIosFormatParameters(width, height, _options.Destination, sourceFiles).ToList();
				var webFormatParameters = GetWebFormatParameters(width, height, _options.Destination, sourceFiles).ToList();

				GenerateAndroidImages(androidFormatParameters);
				GenerateIosImages(iosFormatParameters);
				GenerateWebImages(webFormatParameters);
			}
		}

		private void LogIfVerbose(string message)
		{
			if (_options.LoggingEnabled)
				_loggingCallback?.Invoke(message);
		}

		static void ClearConsoleLine()
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, currentLineCursor - 1);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor - 1);
		}

		private async Task GenerateImages(List<GenerationInformation> parameters)
		{
			LogIfVerbose($"Generating files");
			for (var index = 0; index < parameters.Count; index++)
			{
				ClearConsoleLine();
				LogIfVerbose($"Progress {index+1} / {parameters.Count}");

				var parameter = parameters[index];
				
				if (_options.SkipExisting && File.Exists(parameter.DestinationPath))
					continue;

				IoHelper.CreateDirectoryRecursive(Path.GetDirectoryName(parameter.DestinationPath));
				
				await SvgConverter.GeneratePngAsync(parameter.SourcePath, parameter.DestinationPath,
					(parameter.WidthDp, parameter.HeightDp, parameter.Dpi));
			}
		}

		private async void GenerateAndroidImages(List<GenerationInformation> parameters)
		{
			LogIfVerbose($"Generating android images");
			await GenerateImages(parameters);
		}

		private async void GenerateIosImages(List<GenerationInformation> parameters)
		{
			LogIfVerbose($"Generating ios images");
			await GenerateImages(parameters);

			LogIfVerbose($"Generating ios Contents.json files");
			await GenerateIosContentFiles(parameters);
		}

		private async Task GenerateIosContentFiles(List<GenerationInformation> parameters)
		{
			var imageSetGroups = parameters.GroupBy(d => Path.GetDirectoryName(d.DestinationPath));
			int i = 0;
			var max = imageSetGroups.Count();

			LogIfVerbose($"Generating ios Contents.json files");
			foreach (var group in imageSetGroups)
			{
				ClearConsoleLine();
				LogIfVerbose($"Progress {++i} / {max}");

				var imageSetFolder = Path.GetDirectoryName(group.First().DestinationPath);
				var contentFilePath = Path.Combine(imageSetFolder, "Contents.json");

				if (_options.SkipExisting && File.Exists(contentFilePath))
					continue;

				var contentFile = CreateContentFile(group.Select(s => s));
				var serialized = JsonConvert.SerializeObject(contentFile, Formatting.Indented);


				File.WriteAllText(contentFilePath, serialized);
			}
		}

		private IosContentFile CreateContentFile(IEnumerable<GenerationInformation> items)
		{
			return new IosContentFile()
			{
				info = new IosContentFileInfo()
				{
					author = "xcode",
					version = 1,
				},
				images = items.Select(s => new IosContentFileImage()
				{
					filename = Path.GetFileName(s.DestinationPath),
					idiom = "universal",
					scale = GetScaleFromDpi(s.Dpi)
				}).ToArray()
			};
		}

		private string GetScaleFromDpi(int dpi)
		{
			switch (dpi)
			{
				case 163:
					return "1x";
				case 326:
					return "2x";
				case 489:
					return "3x";
				default:
					throw new ArgumentException($"{nameof(GetScaleFromDpi)} can't convert dpi {dpi} to a scale.");
			}
		}

		private async void GenerateWebImages(List<GenerationInformation> parameters)
		{
			LogIfVerbose($"Generating web images");
			await GenerateImages(parameters);
		}

		private static IEnumerable<GenerationInformation> GetWebFormatParameters(int width, int height, string destination, string[] sourceFiles)
		{
			foreach (var sourceFile in sourceFiles)
			{
				var topLevelFolder = FileNameHelper.GetFirstLevelFolderName(width, height, sourceFile);
				var fileName = FileNameHelper.GetFileName(width, height, sourceFile);

				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "web", $"{fileName}_1x.png"), width, height, 72);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "web", $"{fileName}_2x.png"), width, height, 144);
			}
		}

		private static IEnumerable<GenerationInformation> GetIosFormatParameters(int width, int height, string destination, string[] sourceFiles)
		{
			// https://stackoverflow.com/questions/1365112/what-dpi-resolution-is-used-for-an-iphone-app

			foreach (var sourceFile in sourceFiles)
			{
				var topLevelFolder = FileNameHelper.GetFirstLevelFolderName(width, height, sourceFile);
				var fileName = FileNameHelper.GetFileName(width, height, sourceFile);

				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "ios", $"{topLevelFolder}.imageset", $"{fileName}_1x.png"), width, height, 163);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "ios", $"{topLevelFolder}.imageset", $"{fileName}_2x.png"), width, height, 326);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "ios", $"{topLevelFolder}.imageset", $"{fileName}_3x.png"), width, height, 489);
			}
		}

		private static IEnumerable<GenerationInformation> GetAndroidFormatParameters(int width, int height, string destination, string[] sourceFiles)
		{
			/** https://developer.android.com/guide/practices/screens_support.html
				 * ldpi (low) ~120dpi
				 *	mdpi (medium) ~160dpi
				 *	hdpi (high) ~240dpi
				 *	xhdpi (extra-high) ~320dpi
				 *	xxhdpi (extra-extra-high) ~480dpi
				 *	xxxhdpi (extra-extra-extra-high) ~640dpi
				*/

			foreach (var sourceFile in sourceFiles)
			{
				var topLevelFolder = FileNameHelper.GetFirstLevelFolderName(width, height, sourceFile);
				var fileName = FileNameHelper.GetFileName(width, height, sourceFile);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "android", "drawable-mdpi", $"{fileName}.png"), width, height, 160);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "android", "drawable-hdpi", $"{fileName}.png"), width, height, 240);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "android", "drawable-xhdpi", $"{fileName}.png"), width, height, 320);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "android", "drawable-xxhdpi", $"{fileName}.png"), width, height, 480);
				yield return new GenerationInformation(sourceFile, Path.Combine(destination, topLevelFolder, "android", "drawable-xxxhdpi", $"{fileName}.png"), width, height, 640);
			}
		}
	}
}