using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using mobile.imagetools.shared.Tools.ImageGenerator.Data;
using mobile.imagetools.shared.Tools.IosContentFileGenerator.Data;
using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.ImageGenerator.Modules
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

				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "ios", $"{fileName}.imageset", $"{fileName}_1x.png"), Width, Height, 163, Color, new KeyValuePair<string, string>("scale", "1x"));
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "ios", $"{fileName}.imageset", $"{fileName}_2x.png"), Width, Height, 326, Color, new KeyValuePair<string, string>("scale", "2x"));
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "ios", $"{fileName}.imageset", $"{fileName}_3x.png"), Width, Height, 489, Color, new KeyValuePair<string, string>("scale", "3x"));
			}
		}

		/// <inheritdoc />
		public override async Task GenerateAsync(List<ImageInformation> parameters)
		{
			await GenerateFilesAsync(parameters).ConfigureAwait(false);
		}
		
		/// <inheritdoc />
		public override string GeneratorName => "ios";
	}
}