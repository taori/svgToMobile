using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using svg.generator.shared.Utility;

namespace svg.generator.shared.Modules.Generator
{
	public class AndroidGeneratorModule : GeneratorModule
	{
		/// <inheritdoc />
		public override IEnumerable<ImageInformation> GetParameters()
		{
			/** https://developer.android.com/guide/practices/screens_support.html
				 * ldpi (low) ~120dpi
				 *	mdpi (medium) ~160dpi
				 *	hdpi (high) ~240dpi
				 *	xhdpi (extra-high) ~320dpi
				 *	xxhdpi (extra-extra-high) ~480dpi
				 *	xxxhdpi (extra-extra-extra-high) ~640dpi
				*/

			if (!string.IsNullOrEmpty(Context.Options.ColorCodes))
			{
				foreach (var colorCode in Context.Options.GetColorCodes())
				{
					foreach (var sourceFile in Sources)
					{
						var sanitizedAssetName = FileNameHelper.GetSanitizedAssetName(Width, Height, sourceFile, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable", $"{sanitizedAssetName}.png"), Width, Height, 160, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-ldpi", $"{sanitizedAssetName}.png"), Width, Height, 120, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-mdpi", $"{sanitizedAssetName}.png"), Width, Height, 160, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-hdpi", $"{sanitizedAssetName}.png"), Width, Height, 240, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-xhdpi", $"{sanitizedAssetName}.png"), Width, Height, 320, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-xxhdpi", $"{sanitizedAssetName}.png"), Width, Height, 480, colorCode);
						yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-xxxhdpi", $"{sanitizedAssetName}.png"), Width, Height, 640, colorCode);
					}
				}
			}
			else
			{
				foreach (var sourceFile in Sources)
				{
					var sanitizedAssetName = FileNameHelper.GetSanitizedAssetName(Width, Height, sourceFile);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable", $"{sanitizedAssetName}.png"), Width, Height, 160);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-ldpi", $"{sanitizedAssetName}.png"), Width, Height, 120);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-mdpi", $"{sanitizedAssetName}.png"), Width, Height, 160);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-hdpi", $"{sanitizedAssetName}.png"), Width, Height, 240);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-xhdpi", $"{sanitizedAssetName}.png"), Width, Height, 320);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-xxhdpi", $"{sanitizedAssetName}.png"), Width, Height, 480);
					yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, sanitizedAssetName, "android", "drawable-xxxhdpi", $"{sanitizedAssetName}.png"), Width, Height, 640);
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