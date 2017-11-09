using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using svg.generator.shared.Modules.Generator.iOS;
using svg.generator.shared.Utility;

namespace svg.generator.shared.Modules.Generator
{
	public class IosGeneratorModule : GeneratorModule
	{
		/// <inheritdoc />
		public override IEnumerable<ImageInformation> GetParameters()
		{
			// https://stackoverflow.com/questions/1365112/what-dpi-resolution-is-used-for-an-iphone-app

			foreach (var sourceFile in Sources)
			{
				var fileName = GetSanitizedFileName(sourceFile);

				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "ios", $"{fileName}.imageset", $"{fileName}_1x.png"), Width, Height, 163, ColorCode, new KeyValuePair<string, string>("scale", "1x"));
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "ios", $"{fileName}.imageset", $"{fileName}_2x.png"), Width, Height, 326, ColorCode, new KeyValuePair<string, string>("scale", "2x"));
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "ios", $"{fileName}.imageset", $"{fileName}_3x.png"), Width, Height, 489, ColorCode, new KeyValuePair<string, string>("scale", "3x"));
			}
		}

		/// <inheritdoc />
		public override async Task GenerateAsync(List<ImageInformation> parameters)
		{
			await GenerateFilesAsync(parameters).ConfigureAwait(false);
			await GenerateIosContentFiles(parameters).ConfigureAwait(false);
		}

		private async Task GenerateIosContentFiles(List<ImageInformation> parameters)
		{
			var imageSetGroups = parameters.GroupBy(d => Path.GetDirectoryName(d.DestinationPath));
			int i = 0;
			var max = imageSetGroups.Count();

			Context.Log($" - {GeneratorName} - Content.json ");

			using (var progress = Context.ProgressVisualizerFactory.Create())
			{
				foreach (var group in imageSetGroups)
				{
					progress.Report((float)i / max);

					var imageSetFolder = Path.GetDirectoryName(group.First().DestinationPath);
					var contentFilePath = Path.Combine(imageSetFolder, "Contents.json");

					if (Context.Options.SkipExisting && File.Exists(contentFilePath))
						continue;

					var contentFile = CreateContentFile(group.Select(s => s));
					var serialized = JsonConvert.SerializeObject(contentFile, Formatting.Indented);
					
					File.WriteAllText(contentFilePath, serialized);
				}
			}

			Context.Log(Environment.NewLine);
		}

		private IosContentFile CreateContentFile(IEnumerable<ImageInformation> items)
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
					scale = s.Attributes[0].Value
				}).ToArray()
			};
		}

		/// <inheritdoc />
		public override string GeneratorName => "ios";
	}
}