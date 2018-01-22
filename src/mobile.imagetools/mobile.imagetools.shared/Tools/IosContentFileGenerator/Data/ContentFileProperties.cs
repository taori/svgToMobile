using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.IosContentFileGenerator.Data
{
	public class ContentFileProperties
	{
		[JsonProperty("template-rendering-intent")]
		public string TemplateRenderingIntent { get; set; }
	}
}