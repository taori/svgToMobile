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
				Context.LogLine($"{Context.Options.CsProjectFilePath} does not exist.", true);
				return false;
			}
			if (!Directory.Exists(Context.Options.ResourceFolder))
			{
				Context.LogLine($"{Context.Options.CsProjectFilePath} does not exist.", true);
				return false;
			}
			
			XNamespace xNamespace = GetXmlNamespace();
			var document = XDocument.Load(Context.Options.CsProjectFilePath);
			var resourceFiles = GetFilteredResourceFiles(GetRelativePaths(GetSupportedResourceFiles()));
			var presentElements = GetPresentElements(document, xNamespace);
			var f = presentElements.Count();
			var presentInformations = presentElements.Select(s => new
			{
				s.element,
				path = GetRelativePathFromElement(s.element),
				s.group
			}).Where(d => !string.IsNullOrEmpty(d.path)).ToArray();

			var a = resourceFiles.Where(d => d.EndsWith(".json")).Count();
			var b = presentInformations.Where(d => d.path.EndsWith(".json")).Count();

			var presentPaths = presentInformations.ToDictionary(s => s.path);
			var resourcePaths = new HashSet<string>(resourceFiles);

			if (presentPaths.Count == resourcePaths.Count)
			{
				Context.LogLine($"No change needed. {Context.Options.CsProjectFilePath} is up to date.", true);
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
					if (IsRemovableReference(present.Key))
						excessPaths.Add(present.Key);
				}
			}

			foreach (var path in resourcePaths)
			{
				if (!presentPaths.ContainsKey(path))
					missingPaths.Add(path);
			}

			if ((missingPaths.Count + excessPaths.Count + matchingPaths.Count) == 0)
			{
				Context.LogLine($"No change needed. {Context.Options.CsProjectFilePath} is up to date.", true);
				return true;
			}

			var firstGroup = presentElements.FirstOrDefault();
			var anyChange = false;
			if (firstGroup.group != null)
			{
				if (Context.Options.AddMissing)
				{
					if (missingPaths.Count > 0)
					{
						Context.LogLine($"Adding {missingPaths.Count} references.", true);
						foreach (var match in missingPaths)
						{
							anyChange = true;
							AddElement(firstGroup.group, match, xNamespace);
						}
					}
				}

				if (Context.Options.RemoveExcessive)
				{
					if (excessPaths.Count > 0)
					{
						Context.LogLine($"Removing {excessPaths.Count} references.", true);

						foreach (var match in excessPaths)
						{
							if (presentPaths.TryGetValue(match, out var anon))
							{
								anyChange = true;
								anon.element.Remove();
							}
						}
					}
				}
			}

			if (anyChange)
			{
				document.Save(Context.Options.CsProjectFilePath);
			}
			else
			{
				Context.LogLine($"No change needed. {Context.Options.CsProjectFilePath} is up to date.", true);
				return true;
			}

			return true;
		}

		protected virtual bool IsRemovableReference(string path)
		{
			return true;
		}

		protected virtual bool IsSupportedElement(string partialPath)
		{
			return GeneratorModule.SupportedFormats.ContainsKey(Path.GetExtension(partialPath));
		}

		protected virtual IEnumerable<string> GetFilteredResourceFiles(IEnumerable<string> relativePaths) => relativePaths;

		protected abstract void AddElement(XElement itemGroup, string path, XNamespace ns);

		protected abstract IEnumerable<(XElement group, XElement element)> GetPresentElements(XDocument document, XNamespace xNamespace);

		protected virtual string[] GetSupportedResourcePatterns() => GeneratorModule.SupportedFormats.Keys.ToArray();

		protected IEnumerable<string> GetSupportedResourceFiles()
		{
			var searchPattern = GetSupportedResourcePatterns();
			return IoHelper.GetFilesRecursive(Context.Options.ResourceFolder, searchPattern);
		}

		protected IEnumerable<string> GetRelativePaths(IEnumerable<string> files)
		{
			var root = Path.GetDirectoryName(Context.Options.CsProjectFilePath);
			return files.Select(file => file.Replace(root, string.Empty).TrimStart(Path.DirectorySeparatorChar));
		}

		protected abstract string GetRelativePathFromElement(XElement asset);
	}
}