using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator;
using mobile.imagetools.shared.Tools.IosContentFileGenerator.Data;
using mobile.imagetools.shared.Utility;
using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.IosContentFileGenerator
{
	public class IosContentFileGeneratorTool : MobileImagingTool<IContentFileGeneratorOptions>
	{
		/// <inheritdoc />
		public override async Task<bool> ExecuteAsync()
		{
			await GenerateIosContentFilesAsync();

			return true;
		}

		private async Task<bool> GenerateIosContentFilesAsync()
		{
			if (!Directory.Exists(Context.Options.SourceFolder))
			{
				Context.LogLine($"Directory \"{Context.Options.SourceFolder}\" does not exist.");
				return false;
			}

			switch (Context.Options.Mode)
			{
				case ContentFileMode.ModernUiPackage:
					await GenerateFilesByFoldersAsync(GetFoldersForModernPackage().ToArray());
					break;
				case ContentFileMode.IosAssetFolder:
					await GenerateFilesByFoldersAsync(GetFoldersForIosResourceFolder());
					break;
				default:
					Context.LogLine($"Mode \"{Context.Options.Mode}\" not supported.");
					return false;
			}

			return true;
		}

		private async Task GenerateFilesByFoldersAsync(string[] folders)
		{
			var regex = new Regex(Context.Options.FolderPattern, RegexOptions.IgnoreCase);
			Context.LogLine($"Using the pattern '{Context.Options.FolderPattern}' to filter folders which are subject to Contents.json-generation.");

			folders = folders.Where(folder => regex.IsMatch(folder)).ToArray();

			Context.LogLine($"Generating {folders.Length} files.");

			using (var progress = Context.ProgressVisualizerFactory.Create())
			{
				var max = folders.Length;
				var folderCount = 0;
				foreach (var folder in folders)
				{
					folderCount++;
					progress.Report((float)folderCount / max);
					
					var contentFilePath = Path.Combine(folder, "Contents.json");
					var contentFile = CreateContentFile(folder);
					var serialized = JsonConvert.SerializeObject(contentFile, Formatting.Indented);

					File.WriteAllText(contentFilePath, serialized);
				}
			}
		}

		private string[] GetFoldersForIosResourceFolder()
		{
			return Directory.GetDirectories(Context.Options.SourceFolder);
		}

		private IEnumerable<string> GetFoldersForModernPackage()
		{
			// \svggeneration\appbar_3d_3ds_32x32dp_#ff0000\ios\appbar_3d_3ds_32x32dp_#ff0000.imageset
			foreach (var setFolder in Directory.GetDirectories(Context.Options.SourceFolder))
			{
				foreach (var platformFolder in Directory.GetDirectories(setFolder))
				{
					if(!platformFolder.ToLowerInvariant().EndsWith($"{Path.DirectorySeparatorChar}ios"))
						continue;

					foreach (var imageSetFolder in Directory.GetDirectories(platformFolder))
					{
						yield return imageSetFolder;
					}
				}
			}
		}

		private ContentFile CreateContentFile(string folder)
		{
			var files = Directory.GetFiles(folder);
			var filteredFiles = files.Where(d => GeneratorModule.SupportedFormats.ContainsKey(Path.GetExtension(d))).ToArray();
			var fileWithFormatInfo = filteredFiles.Select(d => new {file = d, info = ImageHelper.GetFormatInfo(d)});

			var contentFile = new ContentFile()
			{
				Info = new ContentFileInfo()
				{
					Author = "xcode",
					Version = 1
				}
			};

			var fileInfos = new ContentFileImage[filteredFiles.Length];
			var arrIndex = 0;
			var scaleIndex = 0;
			foreach (var group in fileWithFormatInfo.GroupBy(d => d.info.Width))
			{
				scaleIndex++;
				foreach (var section in group)
				{
					fileInfos[arrIndex] = new ContentFileImage(Path.GetFileName(section.file), "universal", $"{scaleIndex}x");
					arrIndex++;
				}
			}
			contentFile.Images = fileInfos;

			return contentFile;
		}

		/// <inheritdoc />
		public override string Name => "IosContentFileGenerator";
	}
}