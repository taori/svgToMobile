using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.shared.Tools
{
	public class UpdateXamarinAndroidProjectTool : MobileImagingTool<IUpdateXamarinAndroidProjectOptions>
	{
		/// <inheritdoc />
		public override async Task<bool> ExecuteAsync()
		{
			// <AndroidResource Include="Resources\drawable-hdpi\appbar_chevron_down_24x24dp.png" />

			if (!File.Exists(Context.Options.CsProjectFilePath))
			{
				Context.LogLine($"{Context.Options.CsProjectFilePath} does not exist.");
				return true;
			}

			var document = XDocument.Load(Context.Options.CsProjectFilePath);
			XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
			
			foreach (var itemGroup in document.Descendants(msbuild + "ItemGroup"))
			{
				foreach (var asset in itemGroup.Elements(msbuild + "AndroidResource"))
				{

				}
			}

			return true;
		}

		/// <inheritdoc />
		public override string Name => "UpdateXamarinAndroidProject";
	}
}
 