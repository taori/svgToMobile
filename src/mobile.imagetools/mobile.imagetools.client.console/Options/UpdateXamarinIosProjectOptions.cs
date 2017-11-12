using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class UpdateXamarinIosProjectOptions : UpdateXamarinProjectOptionsBase, IUpdateXamarinIosProjectOptions
	{
		[Option('p', "pattern",
			DefaultValue = ".imageset$",
			HelpText = "Path to project file.")]
		public string ImageSetFolderPattern { get; set; }
	}
}