using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using mobile.imagetools.shared.Modules;
using mobile.imagetools.shared.Modules.Generator;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools
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
		
		/// <inheritdoc />
		public override async Task<bool> ExecuteAsync()
		{
			if (!Directory.Exists(Context.Options.Source))
			{
				Context.LogLine($"Source folder \"{Context.Options.Source}\" does not exist.");
				return false;
			}

			if (!Directory.Exists(Context.Options.Destination))
			{
				IoHelper.CreateDirectoryRecursive(Context.Options.Destination);
			}

			var searchOption = Context.Options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			var sourceFiles = Directory.GetFiles(Context.Options.Source, "*.svg", searchOption);

			Context.LogLine($"Generating files based on {sourceFiles.Length} source files.");

			var formatCount = Context.Options.GetImageFormats().Count();
			if(formatCount <= 0)
			{
				Context.LogLine($"Invalid format specified: {Context.Options.ImageFormats}.");
				return false;
			}

			var colorCodes = Context.Options.GetColorCodes().ToArray();
			var colorCount = colorCodes.Count();
			var extensions = Context.Options.GetExtensions().ToArray();

			if(extensions.Length == 0)
			{
				Context.LogLine($"Invalid extensions specified: {Context.Options.FileExtensions}.");
				return false;
			}

			var current = 0;
			var max = formatCount * Math.Max(1, colorCount);

			if (colorCount <= 0)
			{
				foreach (var format in Context.Options.GetImageFormats())
				{
					current++;
					await RunModuleAsync(current, max, sourceFiles, format, null, extensions).ConfigureAwait(false);
				}
			}
			else
			{
				foreach (var colorCode in colorCodes)
				{
					foreach (var format in Context.Options.GetImageFormats())
					{
						current++;
						await RunModuleAsync(current, max, sourceFiles, format, colorCode, extensions).ConfigureAwait(false);
					}
				}
			}

			Context.LogLine($"done.");

#if DEBUG
			if (Context.Options.Interactive)
				Console.ReadKey();
#endif

			return true;
		}

		private async Task RunModuleAsync(int current, int max, string[] sourceFiles, FormatInfo format, ColorInfo color, string[] extensions)
		{
			var m = max.ToString().Length;
			var progress = string.Format("{0}/{1}", current.ToString().PadLeft(m, ' '), max);
			Context.LogLine($"");
			Context.LogLine($"   {progress} format: {format.Width}x{format.Height}, color: {color?.HexCode ?? "none"}, extension: {string.Join(",", extensions)}");
			Context.LogLine($"");

			foreach (var module in _modules)
			{
				module.Configure(Context, format.Width, format.Height, sourceFiles, color, extensions);
				var parameters = module.GetParameters().ToList();
				await module.GenerateAsync(parameters).ConfigureAwait(false);
			}
		}
	}
}