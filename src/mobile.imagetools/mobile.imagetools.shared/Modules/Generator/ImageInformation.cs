using System.Collections.Generic;

namespace mobile.imagetools.shared.Modules.Generator
{
	public struct ImageInformation
	{
		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi, ColorInfo color) 
			: this(sourcePath, destinationPath, widthDp, heightDp, dpi, color, new KeyValuePair<string, string>[0])
		{
		}

		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi) 
			: this(sourcePath, destinationPath, widthDp, heightDp, dpi, null, new KeyValuePair<string, string>[0])
		{
		}

		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi, ColorInfo color, params KeyValuePair<string, string>[] attributes)
		{
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
			Color = color;
			WidthDp = widthDp;
			HeightDp = heightDp;
			Dpi = dpi;
			Attributes = attributes;
		}

		public string SourcePath { get; set; }

		public string DestinationPath { get; set; }

		public ColorInfo Color { get; set; }

		public int WidthDp { get; set; }

		public int HeightDp { get; set; }

		public int Dpi { get; set; }

		public KeyValuePair<string, string>[] Attributes { get; set; }
	}
}