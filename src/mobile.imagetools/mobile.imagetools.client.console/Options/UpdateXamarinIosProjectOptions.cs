using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class UpdateXamarinIosProjectOptions : ConsoleOptionBase, IUpdateXamarinIosProjectOptions
	{
		[Option('s', "projectFile",
			Required = true,
			HelpText = "Path to project file.")]
		public string CsProjectFilePath { get; set; }

		[Option('p', "pattern",
			DefaultValue = ".imageset$",
			HelpText = "Path to project file.")]
		public string ImageSetFolderPattern { get; set; }
	}
}