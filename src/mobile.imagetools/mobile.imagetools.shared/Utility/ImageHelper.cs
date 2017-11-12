using System;
using System.IO;
using System.Windows.Media.Imaging;
using mobile.imagetools.shared.Tools.ImageGenerator.Data;

namespace mobile.imagetools.shared.Utility
{
	public static class ImageHelper
	{
		public static FormatInfo GetFormatInfo(string fileName)
		{
			if (!File.Exists(fileName))
				throw new ArgumentException("File does not exist.", nameof(fileName));

			using (var imageStream = File.OpenRead(fileName))
			{
				var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
				var height = decoder.Frames[0].PixelHeight;
				var width = decoder.Frames[0].PixelWidth;

				return new FormatInfo(width, height);
			}
		}
	}
}