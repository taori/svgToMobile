using System;
using CommandLine;
using svg.generator.client.console.Options;
using svg.generator.shared.Modules.Generator;
using svg.generator.shared.Utility;

namespace svg.generator.client.console
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