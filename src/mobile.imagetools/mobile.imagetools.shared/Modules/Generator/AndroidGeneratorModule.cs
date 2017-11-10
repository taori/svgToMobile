using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace mobile.imagetools.shared.Modules.Generator
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

			foreach (var sourceFile in Sources)
			{
				var fileName = GetSanitizedFileName(sourceFile);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable", $"{fileName}.png"), Width, Height, 160, Color);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable-ldpi", $"{fileName}.png"), Width, Height, 120, Color);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable-mdpi", $"{fileName}.png"), Width, Height, 160, Color);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable-hdpi", $"{fileName}.png"), Width, Height, 240, Color);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable-xhdpi", $"{fileName}.png"), Width, Height, 320, Color);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable-xxhdpi", $"{fileName}.png"), Width, Height, 480, Color);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "android", "drawable-xxxhdpi", $"{fileName}.png"), Width, Height, 640, Color);
			}
		}

		/// <inheritdoc />
		public override async Task GenerateAsync(List<ImageInformation> parameters)
		{
			await GenerateFilesAsync(parameters).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public override string GeneratorName => "android";
	}
}