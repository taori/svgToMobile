using System;
using svg.generator.shared.Utility;

namespace svg.generator.shared.Modules.Generator
{
	public class GeneratorContext : IToolContext<IGeneratorOptions>
	{
		/// <inheritdoc />
		public GeneratorContext(IGeneratorOptions options, Action<string> log, IProgressVisualizerFactory progressVisualizerFactory)
		{
			Options = options ?? throw new ArgumentNullException(nameof(options));
			Log = log ?? throw new ArgumentNullException(nameof(log));
			ProgressVisualizerFactory = progressVisualizerFactory ?? throw new ArgumentNullException(nameof(progressVisualizerFactory));
		}

		public IGeneratorOptions Options { get; }

		public Action<string> Log { get; }

		public IProgressVisualizerFactory ProgressVisualizerFactory { get; }
	}
}