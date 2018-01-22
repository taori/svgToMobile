using System;
using Newtonsoft.Json;

namespace mobile.imagetools.shared.Tools.IosContentFileGenerator.Data
{
	public class ContentFile
	{
		public ContentFile()
		{
			Properties = new ContentFileProperties();
			Properties.TemplateRenderingIntent = String.Empty;
		}

		[JsonProperty("images")]
		public ContentFileImage[] Images { get; set; }

		[JsonProperty("properties")]
		public ContentFileProperties Properties { get; set; }

		[JsonProperty("info")]
		public ContentFileInfo Info { get; set; }
	}
}