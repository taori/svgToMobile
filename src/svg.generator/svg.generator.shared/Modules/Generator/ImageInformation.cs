using System.Collections.Generic;

namespace svg.generator.shared.Modules.Generator
{
	public struct ImageInformation
	{
		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi, string colorCode) 
			: this(sourcePath, destinationPath, widthDp, heightDp, dpi, colorCode, new KeyValuePair<string, string>[0])
		{
		}

		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi) 
			: this(sourcePath, destinationPath, widthDp, heightDp, dpi, string.Empty, new KeyValuePair<string, string>[0])
		{
		}

		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi, string colorCode, params KeyValuePair<string, string>[] attributes)
		{
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
			ColorCode = colorCode;
			WidthDp = widthDp;
			HeightDp = heightDp;
			Dpi = dpi;
			Attributes = attributes;
		}

		public string SourcePath { get; set; }

		public string DestinationPath { get; set; }

		public string ColorCode { get; set; }

		public int WidthDp { get; set; }

		public int HeightDp { get; set; }

		public int Dpi { get; set; }

		public KeyValuePair<string, string>[] Attributes { get; set; }
	}
}