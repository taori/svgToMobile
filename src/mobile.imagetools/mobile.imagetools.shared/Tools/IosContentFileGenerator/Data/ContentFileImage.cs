using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.IosContentFileGenerator.Data
{
	public class ContentFileImage
	{
		/// <summary>
		/// e.g. "somefilename_2x.png"
		/// </summary>
		[JsonProperty("filename")]
		public string Filename { get; set; }

		/// <summary>
		/// e.g. "universal"
		/// </summary>
		[JsonProperty("idiom")]
		public string Idiom { get; set; }

		/// <summary>
		/// e.g. "2x"
		/// </summary>
		[JsonProperty("scale")]
		public string Scale { get; set; }

		/// <summary>
		/// e.g. "29x29"
		/// </summary>
		[JsonProperty("size")]
		public string Size { get; set; }

		/// <inheritdoc />
		public ContentFileImage()
		{
		}

		/// <inheritdoc />
		public ContentFileImage(string filename, string idiom, string scale, string size)
		{
			Filename = filename;
			Idiom = idiom;
			Scale = scale;
			Size = size;
		}
	}
}