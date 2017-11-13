using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class UpdateXamarinIosProjectOptions : UpdateXamarinProjectOptionsBase, IUpdateXamarinIosProjectOptions
	{
		[Option('p', "pattern",
			DefaultValue = ".imageset$",
			HelpText = "Pattern used to identify folders which should be included to the project file by this tool.")]
		public string ImageSetFolderPattern { get; set; }
	}
}