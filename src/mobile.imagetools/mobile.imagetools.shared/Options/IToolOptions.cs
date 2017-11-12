namespace mobile.imagetools.shared.Options
{
	public interface IToolOptions
	{
		/// <summary>
		/// Name of the tool responsible for these options.
		/// </summary>
		string ToolName { get; set; }

		/// <summary>
		/// Whether or not logging is enabled. 
		/// </summary>
		bool LoggingEnabled { get; set; }

		/// <summary>
		/// Whether or not the application should be run in non interactive mode.
		/// </summary>
		bool Interactive { get; set; }

		/// <summary>
		/// Description of the options
		/// </summary>
		string Description { get; }
	}
}