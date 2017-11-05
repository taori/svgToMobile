using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Svg;

namespace svg.generator.shared
{
	public static class SvgConverter
	{
		private static async Task GenerateImageAsync(string source, string destination, (int widthDp, int heightDp, int dpi) format, ImageFormat fileFormat)
		{
			var pixelWidth = ResolutionConverter.DpToPixel(format.widthDp, format.dpi);
			var pixelHeight = ResolutionConverter.DpToPixel(format.heightDp, format.dpi);

			using (var bitmap = new Bitmap(pixelWidth, pixelHeight))
			{
				var document = SvgDocument.Open(source);
				document.Width = new SvgUnit(SvgUnitType.Pixel, pixelWidth);
				document.Height = new SvgUnit(SvgUnitType.Pixel, pixelHeight);
				document.Ppi = format.dpi;
				document.Draw(bitmap);

				using (var writeStream =
					new FileStream(destination, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					bitmap.Save(writeStream, fileFormat);
				}
			}
		}

		public static async Task GeneratePngAsync(string source, string destination,
			(int widthDp, int heightDp, int dpi) format)
		{
			await GenerateImageAsync(source, destination, format, ImageFormat.Png);
		}
	}
}