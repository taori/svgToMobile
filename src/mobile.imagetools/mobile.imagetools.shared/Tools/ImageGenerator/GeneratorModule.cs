using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator.Data;
using mobile.imagetools.shared.Utility;
using Svg;

namespace mobile.imagetools.shared.Tools.ImageGenerator
{
	public abstract class GeneratorModule
	{
		internal void Configure(IToolContext<IImageGeneratorOptions> context, int width, int height, string[] sourceFiles, ColorInfo color,
			string[] extensions)
		{
			Context = context;
			Extensions = extensions;
			Color = color;
			Sources = sourceFiles ?? throw new ArgumentNullException(nameof(sourceFiles));
			Width = width;
			Height = height;
		}

		protected string GetSanitizedFileName(string sourceFile)
		{
			// someimage.svg -> someimage_24x24dp
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile);
			if (Context.Options.AliasMappings.TryGetValue(fileNameWithoutExtension, out var alias))
			{
				fileNameWithoutExtension = alias;
			}

			var aliasedName = fileNameWithoutExtension?.Replace('.', '_');

			if (string.IsNullOrEmpty(Color?.HexCode?.Trim()))
			{
				return $"{aliasedName}_{Width}x{Height}dp";
			}
			return $"{aliasedName}_{Width}x{Height}dp_{Color?.DisplayName ?? Color.HexCode}";
		}

		public IToolContext<IImageGeneratorOptions> Context { get; internal set; }

		public string[] Extensions { get; internal set; }

		public string[] Sources { get; internal set; }

		public ColorInfo Color { get; internal set; }

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
					
					foreach (var extension in Context.Options.FileExtensions)
					{
						if (TryGetImageFormat(extension.ToLowerInvariant(), out ImageFormat format))
						{
							var destinationFolder = Path.GetDirectoryName(parameter.DestinationPath);
							var fnWithoutEx = Path.GetFileNameWithoutExtension(parameter.DestinationPath);

							await ConvertImageAsync(parameter.SourcePath, Path.Combine(destinationFolder, $"{fnWithoutEx}{extension}"), parameter.WidthDp, parameter.HeightDp,
								parameter.Dpi, format, parameter.Color).ConfigureAwait(false);
						}
					}
				}
			}

			Context.Log(Environment.NewLine);
		}

		public abstract string GeneratorName { get; }

		protected async Task ConvertImageAsync(string source, string destination, int widthDp, int heightDp, int dpi, ImageFormat extension, ColorInfo color)
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
						ChangeFill(document, documentChild, ColorHelper.FromRgb(color.HexCode));
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

		public static readonly Dictionary<string, ImageFormat> SupportedFormats = new Dictionary<string, ImageFormat>()
		{
			{".png", ImageFormat.Png},
			{".jpg", ImageFormat.Jpeg},
			{".gif", ImageFormat.Gif},
			{".bmp", ImageFormat.Bmp},
//			{".icon", ImageFormat.Icon},
//			{".emf", ImageFormat.Emf},
//			{".exif", ImageFormat.Exif},
//			{".tiff", ImageFormat.Tiff},
//			{".wmf", ImageFormat.Wmf},
		};

		private bool TryGetImageFormat(string extension, out ImageFormat imageFormat)
		{
			return SupportedFormats.TryGetValue(extension, out imageFormat);
		}

		private void ChangeFill(SvgDocument document, SvgElement element, Color replaceColor)
		{
			if (element is SvgPath path)
			{
				// prevent full layer paints, so images from material.io don't get overdrawn by their first layer
				if (path.Bounds.Size != document.Bounds.Size)
					element.Fill = new SvgColourServer(replaceColor);
			}

			if (element.Children.Count > 0)
			{
				foreach (var item in element.Children)
				{
					ChangeFill(document, item, replaceColor);
				}
			}
		}
	}
}