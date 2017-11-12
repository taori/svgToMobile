using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools.UpdateXamarinProject
{
	public abstract class UpdateXamarinProjectToolBase<TOption> : MobileImagingTool<TOption> where TOption : IUpdateXamarinProjectOptionsBase
	{
		protected XNamespace GetXmlNamespace()
		{
			return "http://schemas.microsoft.com/developer/msbuild/2003";
		}

		public sealed override async Task<bool> ExecuteAsync()
		{
			if (!File.Exists(Context.Options.CsProjectFilePath))
			{
				Context.LogLine($"{Context.Options.CsProjectFilePath} does not exist.");
				return false;
			}
			if (!Directory.Exists(Context.Options.ResourceFolder))
			{
				Context.LogLine($"{Context.Options.CsProjectFilePath} does not exist.");
				return false;
			}
			
			XNamespace xNamespace = GetXmlNamespace();
			var document = XDocument.Load(Context.Options.CsProjectFilePath);
			var supportedFiles = GetRelativePaths(GetSupportedResourceFiles());
			var presentElements = GetPresentElements(document, xNamespace);
			var presentInformations = presentElements.Select(s => new
			{
				s.element,
				path = GetRelativePathFromElement(s.element),
				s.group
			}).Where(d => !string.IsNullOrEmpty(d.path)).ToArray();

			var presentPaths = presentInformations.ToDictionary(s => s.path);
			var resourcePaths = new HashSet<string>(supportedFiles);

			if (presentPaths.Count == resourcePaths.Count)
			{
				Context.LogLine($"No change needed. {Context.Options.CsProjectFilePath} is up to date.");
				return true;
			}

			var missingPaths = new HashSet<string>();
			var excessPaths = new HashSet<string>();
			var matchingPaths = new HashSet<string>();
			
			foreach (var present in presentPaths)
			{
				if (resourcePaths.Contains(present.Key))
				{
					matchingPaths.Add(present.Key);
				}
				else
				{
					excessPaths.Add(present.Key);
				}
			}

			foreach (var path in resourcePaths)
			{
				if (!matchingPaths.Contains(path) && !excessPaths.Contains(path))
					missingPaths.Add(path);
			}

			if ((missingPaths.Count + excessPaths.Count + matchingPaths.Count) == 0)
			{
				Context.LogLine($"No change needed. {Context.Options.CsProjectFilePath} is up to date.");
				return true;
			}

			var firstGroup = presentElements.FirstOrDefault();
			if (firstGroup.group != null)
			{
				if (Context.Options.AddMissing)
				{
					foreach (var match in missingPaths)
					{
						AddElement(firstGroup.group, match, xNamespace);
					}
				}

				if (Context.Options.RemoveExcessive)
				{
					foreach (var match in excessPaths)
					{
						if (presentPaths.TryGetValue(match, out var anon))
						{
							anon.element.Remove();
						}
					}
				}
			}

			document.Save(Context.Options.CsProjectFilePath);

			return true;
		}

		protected abstract void AddElement(XElement itemGroup, string path, XNamespace ns);

		protected abstract IEnumerable<(XElement group, XElement element)> GetPresentElements(XDocument document, XNamespace xNamespace);

		protected IEnumerable<string> GetSupportedResourceFiles()
		{
			var searchPattern = GeneratorModule.SupportedFormats.Keys.ToArray();
			return IoHelper.GetFilesRecursive(Context.Options.ResourceFolder, searchPattern);
		}

		protected IEnumerable<string> GetRelativePaths(IEnumerable<string> files)
		{
			var root = Path.GetDirectoryName(Context.Options.CsProjectFilePath);
			return files.Select(file => file.Replace(root, string.Empty).TrimStart(Path.DirectorySeparatorChar));
		}

		protected abstract string GetRelativePathFromElement(XElement asset);
	}

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
 