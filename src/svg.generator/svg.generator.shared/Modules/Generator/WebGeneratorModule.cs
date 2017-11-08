using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using svg.generator.shared.Utility;

namespace svg.generator.shared.Modules.Generator
{
	public class WebGeneratorModule : GeneratorModule
	{
		/// <inheritdoc />
		public override IEnumerable<ImageInformation> GetParameters()
		{
			if (!string.IsNullOrEmpty(Context.Options.ColorCodes))
			{
				foreach (var colorCode in Context.Options.GetColorCodes())
				{
					foreach (var sourceFile in Sources)
					{
						var fileName = FileNameHelper.GetSanitizedAssetName(Width, Height, sourceFile, colorCode);

						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "web", $"{fileName}_1x.png"), Width, Height, 72, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "web", $"{fileName}_2x.png"), Width, Height, 144, colorCode);
					}
				}
			}
			else
			{
				foreach (var sourceFile in Sources)
				{
					var fileName = FileNameHelper.GetSanitizedAssetName(Width, Height, sourceFile);

					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "web", $"{fileName}_1x.png"), Width, Height, 72);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "web", $"{fileName}_2x.png"), Width, Height, 144);
				}
			}
		}

		/// <inheritdoc />
		public override async Task GenerateAsync(List<ImageInformation> parameters)
		{
			await GenerateFilesAsync(parameters);
		}
	}
}