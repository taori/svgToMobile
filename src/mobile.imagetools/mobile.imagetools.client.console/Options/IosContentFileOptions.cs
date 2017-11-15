using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class IosContentFileOptions : ConsoleOptionBase, IContentFileGeneratorOptions
	{
		[Option('s', "source",
			Required = true,
			HelpText = "Source folder. This must be the root of the modernui package or the media assets folder of the ios project.")]
		public string SourceFolder { get; set; }

		[Option('m', "mode",
			Required = true,
			HelpText = "Mode of execution: ModernUiPackage, IosAssetFolder.")]
		public ContentFileMode Mode { get; set; }

		[Option('f', "filter",
			DefaultValue = @"\.imageset$",
			HelpText = "Pattern used to identify folders which are subject to Contents.json generation.")]
		public string FolderPattern { get; set; }
	}
}