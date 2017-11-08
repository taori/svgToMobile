using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using svg.generator.shared.Modules;
using svg.generator.shared.Modules.Generator;
using svg.generator.shared.Utility;

namespace svg.generator.shared.Tools
{
	public class ImageGeneratorTool : MobileImagingTool<GeneratorContext>
	{
		public ImageGeneratorTool()
		{
			_modules = new List<GeneratorModule>()
			{
				new AndroidGeneratorModule(),
				new IosGeneratorModule(),
				new WebGeneratorModule()
			};
		}

		private static readonly Regex FormatRegex = new Regex(@"^(?<width>[\d]+)x(?<height>[\d]+)$");

		/// <inheritdoc />
		public override string Name => "ImageGenerator";

		private List<GeneratorModule> _modules = new List<GeneratorModule>();

		/// <inheritdoc />
		public override bool TryClaimContext(IToolContext context)
		{
			if (context is GeneratorContext c)
			{
				Context = c;
				return true;
			}

			return false;
		}

		/// <param name="context"></param>
		/// <inheritdoc />
		public override async Task<bool> ExecuteAsync()
		{
			if (!Directory.Exists(Context.Options.Source))
			{
				Context.Log($"Source folder \"{Context.Options.Source}\" does not exist.");
				return false;
			}

			if (!Directory.Exists(Context.Options.Destination))
			{
				Context.Log($"Creating destination folder \"{Context.Options.Destination}\".");
				IoHelper.CreateDirectoryRecursive(Context.Options.Destination);
			}

			var searchOption = Context.Options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			var sourceFiles = Directory.GetFiles(Context.Options.Source, "*.svg", searchOption);

			Context.Log($"Generating files based on {sourceFiles.Length} source files.");

			var formats = Context.Options.ImageFormats.Split(';');
			foreach (var format in formats)
			{
				if (!FormatRegex.IsMatch(format))
				{
					Context.Log($"Invalid format specified: {format}");
					continue;
				}

				var match = FormatRegex.Match(format);

				Context.Log($"");
				Context.Log($"   format: {format}");
				Context.Log($"");

				var width = Int32.Parse(match.Groups["width"].Value);
				var height = Int32.Parse(match.Groups["height"].Value);

				foreach (var module in _modules)
				{
					module.Configure(Context, width, height, sourceFiles);
					var parameters = module.GetParameters().ToList();
					await module.GenerateAsync(parameters);
				}
			}

#if DEBUG
			Console.ReadKey();
#endif

			return true;
		}
	}
}