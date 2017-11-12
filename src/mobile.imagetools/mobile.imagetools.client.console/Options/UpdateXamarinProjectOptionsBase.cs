using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class UpdateXamarinProjectOptionsBase : ConsoleOptionBase, IUpdateXamarinProjectOptionsBase
	{
		[Option('s', "projectFile",
			Required = true,
			HelpText = "Path to project file.")]
		public string CsProjectFilePath { get; set; }

		[Option('r', "xcassetsFolder",
			Required = true,
			HelpText = "Path to Media.xcassets folder.")]
		public string ResourceFolder { get; set; }

		[Option("removeExcessive",
			DefaultValue = false,
			HelpText = "Remove files which are listed in the project file but missing in the resource folder.")]
		public bool RemoveExcessive { get; set; }

		[Option("addMissing",
			DefaultValue = true,
			HelpText = "Add files which are in the Media.xcassets folder but missing in the project file.")]
		public bool AddMissing { get; set; }
	}
}