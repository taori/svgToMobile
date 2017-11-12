using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class UpdateXamarinAndroidProjectOptions : ConsoleOptionBase, IUpdateXamarinAndroidProjectOptions
	{
		[Option('s', "projectFile",
			Required = true,
			HelpText = "Path to project file.")]
		public string CsProjectFilePath { get; set; }
	}
}