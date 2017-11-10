using System;
using mobile.imagetools.shared.Tools;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Modules.Generator
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