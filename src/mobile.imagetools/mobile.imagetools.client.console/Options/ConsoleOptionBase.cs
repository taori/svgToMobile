using CommandLine;
using CommandLine.Text;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.client.console.Options
{
	public abstract class ConsoleOptionBase : IToolOptions
	{
		[Option("tool",
			Required = true,
			HelpText = "The name of the tool to be executed.")]
		public string ToolName { get; set; }

		[Option("verbose",
			DefaultValue = false,
			HelpText = "Verbose mode.")]
		public bool LoggingEnabled { get; set; }

		[Option("interactive",
			DefaultValue = true,
			HelpText = "Whether or not the application should be executed without user interaction.")]
		public bool Interactive { get; set; }

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