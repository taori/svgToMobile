using CommandLine;
using CommandLine.Text;
using mobile.imagetools.shared.Modules.Generator;

namespace mobile.imagetools.client.console.Options
{
	public class GeneratorOptions : IGeneratorOptions
	{
		[Option('s', "source",
			Required = true,
			HelpText = "The input paths to be processed.")]
		public string Source { get; set; }

		[Option('d', "destination",
			Required = true,
			HelpText = "The output folder for rendered graphics.")]
		public string Destination { get; set; }

		[Option('t', "tool",
			Required = true,
			HelpText = "The name of the tool to be executed.")]
		public string ToolName { get; set; }

		[Option('v', "verbosity",
			DefaultValue = false,
			HelpText = "Verbose mode.")]
		public bool LoggingEnabled { get; set; }

		[Option('f', "formats",
			Required = true,
			HelpText = "Formats to generate from svg files. Specify like: 32x32 or 32x32;64x64.")]
		public string ImageFormats { get; set; }
		
		[Option("skipExisting",
			DefaultValue = false,
			HelpText = "Skip images which already exist.")]
		public bool SkipExisting { get; set; }

		[Option('c', "color",
			DefaultValue = null,
			HelpText = "Accepts colors in a pattern like #ffffffx#ffffffff ")]
		public string ColorCodes { get; set; }
		
		[Option('x', "extensions",
			DefaultValue = ".jpg,.bmp,.png",
			HelpText = "Accepts a pattern of accepted file formats like '.jpg,.png,.gif'.")]
		public string FileExtensions { get; set; }

		[Option('i', "interactive",
			DefaultValue = true,
			HelpText = "Whether or not the application should be executed without user interaction.")]
		public bool Interactive { get; set; }

		[Option('r', "recursive",
			DefaultValue = false,
			HelpText = "Recursive source read mode.")]
		public bool Recursive { get; set; }

		/// <inheritdoc />
		public string Description => GetUsage();

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}