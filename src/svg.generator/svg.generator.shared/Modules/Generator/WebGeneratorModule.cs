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
			foreach (var sourceFile in Sources)
			{
				var fileName = GetSanitizedFileName(sourceFile);

				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "web", $"{fileName}_1x.png"), Width, Height, 72, ColorCode);
				yield return new ImageInformation(sourceFile, Path.Combine(Context.Options.Destination, fileName, "web", $"{fileName}_2x.png"), Width, Height, 144, ColorCode);
			}
		}

		/// <inheritdoc />
		public override async Task GenerateAsync(List<ImageInformation> parameters)
		{
			await GenerateFilesAsync(parameters).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public override string GeneratorName => "web";
	}
}