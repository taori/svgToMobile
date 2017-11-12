using System;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Platform
{
	public interface IFeedbackFactory
	{
		Action<string> CreateContentLogger<T>(T options) where T : class, IToolOptions;

		Action<string> CreateLineLogger<T>(T options) where T : class, IToolOptions;

		IProgressVisualizerFactory CreateProgressVisualizer<T>(T options) where T : class, IToolOptions;
	}
}