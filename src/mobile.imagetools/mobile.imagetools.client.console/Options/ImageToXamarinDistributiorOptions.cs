using CommandLine;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public class ImageToXamarinDistributiorOptions : ConsoleOptionBase, IImageToXamarinDistributorOptions
	{
		[Option('s', "source",
			Required = true,
			HelpText = "folder of package in a fashion prodced similar to modernui.com.")]
		public string ModernPackageFolder { get; set; }

		[Option("iosFolder",
			Required = true,
			HelpText = "Media assets folder of ios project.")]
		public string IosResourceFolder { get; set; }

		[Option('a', "androidFolder",
			Required = true,
			HelpText = "Resource root folder of android project.")]
		public string AndroidResourceFolder { get; set; }

		[Option('r', "androidFolder",
			DefaultValue = true,
			HelpText = "Remove existing asset folders at destination.")]
		public bool DeleteExisting { get; set; }
	}
}