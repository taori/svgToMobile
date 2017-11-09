﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using svg.generator.shared.Modules.Generator;
using svg.generator.shared.Utility;
using Svg;

namespace svg.generator.shared.Modules
{
	public abstract class GeneratorModule
	{
		internal void Configure(GeneratorContext context, int width, int height, string[] sourceFiles, string colorCode,
			string[] extensions)
		{
			Context = context;
			Extensions = extensions;
			ColorCode = colorCode;
			Sources = sourceFiles ?? throw new ArgumentNullException(nameof(sourceFiles));
			Width = width;
			Height = height;
		}

		protected string GetSanitizedFileName(string sourceFile)
		{
			// someimage.svg -> someimage_24x24dp
			if (string.IsNullOrEmpty(ColorCode?.Trim()))
			{
				return $"{Path.GetFileNameWithoutExtension(sourceFile)?.Replace('.', '_')}_{Width}x{Height}dp";
			}
			return $"{Path.GetFileNameWithoutExtension(sourceFile)?.Replace('.', '_')}_{Width}x{Height}dp_{ColorCode}";
		}

		public GeneratorContext Context { get; internal set; }

		public string[] Extensions { get; internal set; }

		public string[] Sources { get; internal set; }

		public string ColorCode { get; internal set; }

		public int Height { get; internal set; }

		public int Width { get; internal set; }

		public abstract IEnumerable<ImageInformation> GetParameters();

		public abstract Task GenerateAsync(List<ImageInformation> parameters);

		protected async Task GenerateFilesAsync(List<ImageInformation> parameters)
		{
			Context.Log($" - {this.GeneratorName} ");

			using (var progress = Context.ProgressVisualizerFactory.Create())
			{
				for (var index = 0; index < parameters.Count; index++)
				{
					progress.Report((float) index / parameters.Count);

					var parameter = parameters[index];

					if (Context.Options.SkipExisting && File.Exists(parameter.DestinationPath))
						continue;

					IoHelper.CreateDirectoryRecursive(Path.GetDirectoryName(parameter.DestinationPath));

					foreach (var extension in Extensions)
					{
						if (TryGetFormat(extension, out ImageFormat format))
						{
							var destinationFolder = Path.GetDirectoryName(parameter.DestinationPath);
							var fnWithoutEx = Path.GetFileNameWithoutExtension(parameter.DestinationPath);

							await ConvertImageAsync(parameter.SourcePath, Path.Combine(destinationFolder, $"{fnWithoutEx}{extension}"), parameter.WidthDp, parameter.HeightDp,
								parameter.Dpi, format, parameter.ColorCode).ConfigureAwait(false);
						}
					}
				}
			}

			Context.Log(Environment.NewLine);
		}

		public abstract string GeneratorName { get; }

		protected async Task ConvertImageAsync(string source, string destination, int widthDp, int heightDp, int dpi, ImageFormat extension, string color = null)
		{
			var pixelWidth = ResolutionConverter.DpToPixel(widthDp, dpi);
			var pixelHeight = ResolutionConverter.DpToPixel(heightDp, dpi);

			using (var bitmap = new Bitmap(pixelWidth, pixelHeight))
			{
				var document = SvgDocument.Open(source);
				document.Width = new SvgUnit(SvgUnitType.Pixel, pixelWidth);
				document.Height = new SvgUnit(SvgUnitType.Pixel, pixelHeight);
				document.Ppi = dpi;
				if (color != null)
				{
					foreach (var documentChild in document.Children)
					{
						ChangeFill(documentChild, ColorHelper.FromRgb(color));
					}
				}

				document.Draw(bitmap);

				using (var writeStream =
					new FileStream(destination, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					bitmap.Save(writeStream, extension);
				}
			}
		}

		public Dictionary<string, ImageFormat> SupportedFormats = new Dictionary<string, ImageFormat>()
		{
			{".png", ImageFormat.Png},
			{".jpg", ImageFormat.Jpeg},
			{".gif", ImageFormat.Gif},
			{".bmp", ImageFormat.Bmp},
			{".emf", ImageFormat.Emf},
			{".exif", ImageFormat.Exif},
			{".icon", ImageFormat.Icon},
			{".tiff", ImageFormat.Tiff},
			{".wmf", ImageFormat.Wmf},
		};

		private bool TryGetFormat(string extension, out ImageFormat imageFormat)
		{
			return SupportedFormats.TryGetValue(extension, out imageFormat);
		}

		private void ChangeFill(SvgElement element, Color replaceColor)
		{
			if (element is SvgPath path)
			{
				element.Fill = new SvgColourServer(replaceColor);
			}

			if (element.Children.Count > 0)
			{
				foreach (var item in element.Children)
				{
					ChangeFill(item, replaceColor);
				}
			}
		}
	}
}