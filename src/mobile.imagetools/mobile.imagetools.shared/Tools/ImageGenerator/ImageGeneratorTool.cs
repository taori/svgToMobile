using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator.Data;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools.ImageGenerator
{
	public class ImageGeneratorTool : MobileImagingTool<IImageGeneratorOptions>
	{
		public ImageGeneratorTool()
		{
			var moduleTypes = typeof(GeneratorModule).Assembly.ExportedTypes.Where(type =>
				!type.IsAbstract && typeof(GeneratorModule).IsAssignableFrom(type));

			_modules = new List<GeneratorModule>();
			_modules.AddRange(moduleTypes.Select(s => Activator.CreateInstance(s) as GeneratorModule));
		}
		
		/// <inheritdoc />
		public override string Name => "ImageGenerator";

		private readonly List<GeneratorModule> _modules;
		
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
			
			if(Context.Options.ImageFormats.Length <= 0)
			{
				Context.LogLine($"Invalid format specified: {Context.Options.ImageFormats}.");
				return false;
			}
			
			if(Context.Options.FileExtensions.Length == 0)
			{
				Context.LogLine($"Invalid extensions specified: {Context.Options.FileExtensions}.");
				return false;
			}

			Context.Options.AliasMappings = new Dictionary<string, string>();
			if (File.Exists(Context.Options.AliasMappingPath))
			{
				Context.LogLine($"Alias mapping file '{Context.Options.AliasMappingPath}' is being used.");

				var reader = new CsvHelper.CsvReader(new CsvReader(Context.Options.AliasMappingPath), new Configuration(), false);
				while (reader.Read())
				{
					var fileName = reader.GetField(0);
					var alias = reader.GetField(1);
					if (Context.Options.AliasMappings.TryGetValue(fileName, out var presentAlias))
					{
						Context.LogLine($"Alias for '{fileName}' is already mapped to '{presentAlias}'. Discarding alias '{alias}'.");
					}
					else
					{
						Context.Options.AliasMappings.Add(fileName, alias);
					}
				}
			}

			HashSet<string> filteredExtensions = new HashSet<string>();
			foreach (var extension in Context.Options.FileExtensions)
			{
				if(GeneratorModule.SupportedFormats.ContainsKey(extension.ToLowerInvariant()))
				{
					filteredExtensions.Add(extension);
				}
				else
				{
					Context.LogLine($"Extension {extension} is not supported and will not be rendered.");
				}
			}

			var extensions = filteredExtensions.ToArray();
			if (extensions.Length == 0)
			{
				Context.LogLine("There are no valid extensions left to render.");
				return false;
			}

			var current = 0;
			var max = Context.Options.ImageFormats.Length * Math.Max(1, Context.Options.ColorCodes.Length);

			if (Context.Options.ColorCodes.Length <= 0)
			{
				foreach (var format in Context.Options.ImageFormats)
				{
					current++;
					await RunModuleAsync(current, max, sourceFiles, format, null, extensions).ConfigureAwait(false);
				}
			}
			else
			{
				foreach (var colorCode in Context.Options.ColorCodes)
				{
					foreach (var format in Context.Options.ImageFormats)
					{
						current++;
						await RunModuleAsync(current, max, sourceFiles, format, colorCode, extensions).ConfigureAwait(false);
					}
				}
			}

			return true;
		}

		public class CsvReader : TextReader
		{
			private StreamReader _stream;

			/// <inheritdoc />
			protected override void Dispose(bool disposing)
			{
				if(disposing)
				{
					_stream?.Dispose();
					_stream = null;
				}

				base.Dispose(disposing);
			}

			public CsvReader(string path)
			{
				_stream = new StreamReader(path);
			}

			/// <inheritdoc />
			public override string ReadLine()
			{
				return _stream.ReadLine();
			}

			/// <inheritdoc />
			public override Task<string> ReadLineAsync()
			{
				return _stream.ReadLineAsync();
			}

			/// <inheritdoc />
			public override Task<string> ReadToEndAsync()
			{
				return _stream.ReadToEndAsync();
			}

			/// <inheritdoc />
			public override void Close()
			{
				_stream.Close();
			}

			/// <inheritdoc />
			public override int Read()
			{
				return _stream.Read();
			}

			/// <inheritdoc />
			public override string ReadToEnd()
			{
				return base.ReadToEnd();
			}

			/// <inheritdoc />
			public override Task<int> ReadAsync(char[] buffer, int index, int count)
			{
				return base.ReadAsync(buffer, index, count);
			}
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