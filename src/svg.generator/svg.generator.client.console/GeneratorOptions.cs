using CommandLine;
using CommandLine.Text;
using svg.generator.shared;

namespace svg.generator.client.console
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

		[Option('v', "verbosity",
			DefaultValue = false,
			HelpText = "Verbose mode.")]
		public bool LoggingEnabled { get; set; }

		[Option('f', "formats",
			Required = true,
			HelpText = "Formats to generate from svg files. Specify like: 32x32 or 32x32;64x64.")]
		public string Formats { get; set; }

		[Option('c', "createDestination",
			DefaultValue = false,
			HelpText = "Creates destination if it does not exist yet.")]
		public bool CreateDestinationFolder { get; set; }

		[Option("skipExisting",
			DefaultValue = false,
			HelpText = "Skip images which already exist")]
		public bool SkipExisting { get; set; }

		[Option("exit",
			DefaultValue = false,
			HelpText = "Exit application.")]
		public bool Exit { get; set; }

		[Option('r', "recursive",
			DefaultValue = false,
			HelpText = "Recursive source read mode.")]
		public bool Recursive { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}