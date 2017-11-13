using System;
using mobile.imagetools.shared.Options;

namespace mobile.imagetools.shared.Utility
{
	public interface IToolContext
	{
		void LogLine(string message, bool important = false);

		void Log(string message, bool important = false);

		IProgressVisualizerFactory ProgressVisualizerFactory { get; }

		string Description { get; }
	}

	public interface IToolContext<out TOption> : IToolContext where TOption : IToolOptions
	{
		TOption Options { get; }
	}
}