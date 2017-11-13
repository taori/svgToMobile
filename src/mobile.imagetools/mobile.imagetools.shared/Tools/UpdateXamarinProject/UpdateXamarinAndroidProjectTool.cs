using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator;

namespace mobile.imagetools.shared.Tools.UpdateXamarinProject
{
	public class UpdateXamarinAndroidProjectTool : UpdateXamarinProjectToolBase<IUpdateXamarinAndroidProjectOptions>
	{
		/// <inheritdoc />
		protected override void AddElement(XElement itemGroup, string path, XNamespace ns)
		{
			var element = new XElement(ns + "AndroidResource");
			element.Add(new XAttribute("Include", path));
			itemGroup.Add(element);
		}

		/// <inheritdoc />
		protected override IEnumerable<(XElement group, XElement element)> GetPresentElements(XDocument document, XNamespace xNamespace)
		{
			foreach (var itemGroup in document.Descendants(xNamespace + "ItemGroup"))
			{
				foreach (var asset in itemGroup.Elements(xNamespace + "AndroidResource"))
				{
					yield return (itemGroup, asset);
				}
			}
		}

		protected override string GetRelativePathFromElement(XElement asset)
		{
			//  <AndroidResource Include = "Resources\drawable-xxxhdpi\appbar_chevron_up_48x48dp.png" />
			if (asset.HasAttributes)
			{
				if (asset.Name.LocalName != "AndroidResource")
					return string.Empty;

				var attrValue = asset.Attribute("Include");
				var partialPath = attrValue?.Value ?? string.Empty;
				if(!GeneratorModule.SupportedFormats.ContainsKey(Path.GetExtension(partialPath)))
					return string.Empty;
				return partialPath;
			}

			return string.Empty;
		}

		/// <inheritdoc />
		public override string Name => "UpdateXamarinAndroidProject";
	}
}
 