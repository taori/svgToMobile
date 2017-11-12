namespace mobile.imagetools.shared.Tools.ImageGenerator.Data
{
	public class ColorInfo
	{
		public ColorInfo(string hexCode, string displayName)
		{
			HexCode = hexCode;
			DisplayName = displayName;
		}

		public string HexCode { get; set; }

		public string DisplayName { get; set; }
	}
}