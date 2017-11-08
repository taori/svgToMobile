using System.Collections.Generic;

namespace svg.generator.shared.Modules.Generator
{
	public struct ImageInformation
	{
		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi) 
			: this(sourcePath, destinationPath, widthDp, heightDp, dpi, new KeyValuePair<string, string>[0])
		{
		}

		public ImageInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi, params KeyValuePair<string, string>[] attributes)
		{
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
			WidthDp = widthDp;
			HeightDp = heightDp;
			Dpi = dpi;
			Attributes = attributes;
		}

		public string SourcePath { get; set; }

		public string DestinationPath { get; set; }

		public int WidthDp { get; set; }

		public int HeightDp { get; set; }

		public int Dpi { get; set; }

		public KeyValuePair<string, string>[] Attributes { get; set; }
	}
}