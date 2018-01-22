using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
			element.Add(new XElement(ns + "Visible", "false"));
			itemGroup.Add(element);
		}

		/// <inheritdoc />
		protected override IEnumerable<string> GetFilteredResourceFiles(IEnumerable<string> relativePaths)
		{
			var regex = GetInitializedResourcePattern();

			if (regex != null)
			{
				foreach (var relativePath in base.GetFilteredResourceFiles(relativePaths))
				{
					var directory = Path.GetDirectoryName(relativePath);
					if (directory != null && regex.IsMatch(directory))
						yield return relativePath;
				}
			}
			else
			{
				foreach (var file in base.GetFilteredResourceFiles(relativePaths))
					yield return file;
			}
		}

		private Regex _initializedResourcePattern;
		private Regex GetInitializedResourcePattern()
		{
			_initializedResourcePattern = string.IsNullOrEmpty(Context.Options.ImageSetFolderPattern)
				? null
				: new Regex(Context.Options.ImageSetFolderPattern, RegexOptions.IgnoreCase);
			return _initializedResourcePattern;
		}

		/// <inheritdoc />
		protected override string[] GetSupportedResourcePatterns()
		{
			return base.GetSupportedResourcePatterns().Concat(new []{".json"}).ToArray();
		}

		/// <inheritdoc />
		protected override bool IsRemovableReference(string path)
		{
			return _initializedResourcePattern == null || _initializedResourcePattern.IsMatch(path);
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
				if (!IsSupportedElement(partialPath))
					return string.Empty;
				return partialPath;
			}

			return string.Empty;
		}

		/// <inheritdoc />
		protected override bool IsSupportedElement(string partialPath)
		{
			return base.IsSupportedElement(partialPath) || partialPath.EndsWith("contents.json", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <inheritdoc />
		public override string Name => "UpdateXamarinIosProject";
	}
}