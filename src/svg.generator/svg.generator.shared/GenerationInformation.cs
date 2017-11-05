namespace svg.generator.shared
{
	public struct GenerationInformation
	{
		public GenerationInformation(string sourcePath, string destinationPath, int widthDp, int heightDp, int dpi)
		{
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
			WidthDp = widthDp;
			HeightDp = heightDp;
			Dpi = dpi;
		}

		public string SourcePath { get; set; }

		public string DestinationPath { get; set; }

		public int WidthDp { get; set; }

		public int HeightDp { get; set; }

		public int Dpi { get; set; }
	}
}