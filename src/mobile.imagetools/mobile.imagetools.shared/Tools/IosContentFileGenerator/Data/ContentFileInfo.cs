using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.IosContentFileGenerator.Data
{
	public class ContentFileInfo
	{
		/// <summary>
		/// e.g. "xcode"
		/// </summary>
 		[JsonProperty("author")]
		public string Author { get; set; }

		/// <summary>
		/// e.g. 1
		/// </summary>
		[JsonProperty("version")]
		public int Version { get; set; }
	}
}