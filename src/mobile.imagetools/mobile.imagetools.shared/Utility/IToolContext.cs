using System;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.shared.Utility
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