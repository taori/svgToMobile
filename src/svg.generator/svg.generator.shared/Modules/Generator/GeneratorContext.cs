using System;
using svg.generator.shared.Tools;
using svg.generator.shared.Utility;

namespace svg.generator.shared.Modules.Generator
{
	public class GeneratorContext : ToolContextBase<IGeneratorOptions>
	{
		/// <inheritdoc />
		public GeneratorContext(IGeneratorOptions options, IProgressVisualizerFactory progressVisualizerFactory, Action<string> logLine, Action<string> log) : base(options, progressVisualizerFactory, logLine, log)
		{
		}

		/// <inheritdoc />
		public override string Description => Options.Description;
	}
}