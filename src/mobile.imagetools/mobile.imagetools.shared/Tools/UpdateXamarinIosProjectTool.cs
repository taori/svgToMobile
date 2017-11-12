using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.shared.Tools
{
	public class UpdateXamarinIosProjectTool : MobileImagingTool<IUpdateXamarinIosProjectOptions>
	{
		/// <inheritdoc />
		public override async Task<bool> ExecuteAsync()
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
			if (!File.Exists(Context.Options.CsProjectFilePath))
			{
				Context.LogLine($"{Context.Options.CsProjectFilePath} does not exist.");
				return true;
			}

			var document = XDocument.Load(Context.Options.CsProjectFilePath);
			XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

			foreach (var itemGroup in document.Descendants(msbuild + "ItemGroup"))
			{
				foreach (var asset in itemGroup.Descendants(msbuild + "ImageAsset"))
				{

				}
			}

			return true;
		}

		/// <inheritdoc />
		public override string Name => "UpdateXamarinIosProject";
	}
}