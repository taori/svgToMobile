using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator;

namespace mobile.imagetools.shared.Tools.UpdateXamarinProject
{
	public class UpdateXamarinIosProjectTool : UpdateXamarinProjectToolBase<IUpdateXamarinIosProjectOptions>
	{
		/// <inheritdoc />
		protected override void AddElement(XElement itemGroup, string path, XNamespace ns)
		{
			var element = new XElement(ns + "ImageAsset");
			element.Add(new XAttribute("Include", path));
			element.Add(new XElement("InProject", "false"));
			itemGroup.Add(element);
		}

		/// <inheritdoc />
		protected override IEnumerable<(XElement group, XElement element)> GetPresentElements(XDocument document,
			XNamespace xNamespace)
		{
			/*
			 * 
				<ImageAsset Include="Resources\Medien.xcassets\appbar_chevron_down_24x24dp.imageset\appbar_chevron_down_24x24dp_1x.png">
					<InProject>false</InProject>
				</ImageAsset>
				<ImageAsset Include="Resources\Medien.xcassets\appbar_chevron_down_24x24dp.imageset\appbar_chevron_down_24x24dp_2x.png">
					<InProject>false</InProject>
				</ImageAsset>
				<ImageAsset Include="Resources\Medien.xcassets\appbar_chevron_down_24x24dp.imageset\appbar_chevron_down_24x24dp_3x.png">
					<InProject>false</InProject>
				</ImageAsset>
				<ImageAsset Include="Resources\Medien.xcassets\appbar_chevron_down_24x24dp.imageset\Contents.json">
					<InProject>false</InProject>
				</ImageAsset>
			 */

			foreach (var itemGroup in document.Descendants(xNamespace + "ItemGroup"))
			{
				foreach (var asset in itemGroup.Descendants(xNamespace + "ImageAsset"))
				{
					yield return (itemGroup, asset);
				}
			}
		}

		protected override string GetRelativePathFromElement(XElement asset)
		{
			if (asset.HasAttributes)
			{
				if (asset.Name.LocalName != "ImageAsset")
					return string.Empty;

				var attrValue = asset.Attribute("Include");
				var partialPath = attrValue?.Value ?? string.Empty;
				if (!GeneratorModule.SupportedFormats.ContainsKey(Path.GetExtension(partialPath)))
					return string.Empty;
				return partialPath;
			}

			return string.Empty;
		}

		/// <inheritdoc />
		public override string Name => "UpdateXamarinIosProject";
	}
}