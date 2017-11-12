using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.IosContentFileGenerator.Data
{
	public class ContentFile
	{
		[JsonProperty("images")]
		public ContentFileImage[] Images { get; set; }

		[JsonProperty("info")]
		public ContentFileInfo Info { get; set; }
	}
}