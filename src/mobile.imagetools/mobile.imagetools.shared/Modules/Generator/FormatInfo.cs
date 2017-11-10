namespace mobile.imagetools.shared.Modules.Generator
{
	public class FormatInfo
	{
		/// <inheritdoc />
		public FormatInfo(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public int Width { get; set; }

		public int Height { get; set; }
	}
}