using System;
using CommandLine;
using mobile.imagetools.client.console.Options;
using mobile.imagetools.shared.Modules.Generator;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.client.console
{
	public class ConsoleContextFactory : IToolContextFactory
	{
		public string[] Arguments { get; set; }

		/// <inheritdoc />
		public IToolContext Create()
		{
			var generatorOptions = new GeneratorOptions();
			var progressVisualizerFactory = new ProgressVisualizerFactory();
			Action<string> lineLogger = message =>
			{
				if(generatorOptions.LoggingEnabled)
					Console.WriteLine(message);
			};
			Action<string> contentLogger = message =>
			{
				if(generatorOptions.LoggingEnabled)
					Console.Write(message);
			};

			if (Parser.Default.ParseArguments(Arguments, generatorOptions))
			{
				return new GeneratorContext(generatorOptions, progressVisualizerFactory, lineLogger, contentLogger);
			}

			return null;
		}
	}
}