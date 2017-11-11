using System;
using CommandLine;
using mobile.imagetools.client.console.Options;
using mobile.imagetools.shared.Modules.Generator;
using mobile.imagetools.shared.Tools;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.client.console
{
	public static class ConsoleToolContextFactory
	{
		public static IToolContext<IToolOptions> Create<T>(T options) where T : class, IToolOptions
		{
			var progressVisualizerFactory = new ProgressVisualizerFactory();
			Action<string> lineLogger = message =>
			{
				if (options.LoggingEnabled)
					Console.WriteLine(message);
			};
			Action<string> contentLogger = message =>
			{
				if (options.LoggingEnabled)
					Console.Write(message);
			};

			if (options is IGeneratorOptions generatorOptions)
				return new GeneratorContext(generatorOptions, progressVisualizerFactory, lineLogger, contentLogger);

			return null;
		}
	}

	public class GeneratorContext : InvocationContext<IGeneratorOptions>
	{
		/// <inheritdoc />
		public GeneratorContext(IGeneratorOptions options, IProgressVisualizerFactory progressVisualizerFactory,
			Action<string> logLine, Action<string> log) : base(options, progressVisualizerFactory, logLine, log)
		{
		}
	}
}