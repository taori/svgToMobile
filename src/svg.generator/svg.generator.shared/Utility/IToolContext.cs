using System;
using svg.generator.shared.Modules.Generator;

namespace svg.generator.shared.Utility
{
	public interface IToolContext
	{
		Action<string> LogLine { get; }

		Action<string> Log { get; }

		IProgressVisualizerFactory ProgressVisualizerFactory { get; }

		string Description { get; }
	}

	public interface IToolContext<out TOption> : IToolContext where TOption : IToolOptions
	{
		TOption Options { get; }
	}
}