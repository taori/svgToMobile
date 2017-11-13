using System;
using CommandLine;
using mobile.imagetools.client.console.Options;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Platform;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.client.console
{
	public class ConsoleFeedbackFactory : IFeedbackFactory
	{
		/// <inheritdoc />
		public Action<string> CreateContentLogger<T>(T options) where T : class, IToolOptions
		{
			return s => Console.Write(s);
		}

		/// <inheritdoc />
		public Action<string> CreateLineLogger<T>(T options) where T : class, IToolOptions
		{
			return s => Console.WriteLine(s);
		}

		/// <inheritdoc />
		public IProgressVisualizerFactory CreateProgressVisualizer<T>(T options) where T : class, IToolOptions
		{
			return new ProgressVisualizerFactory();
		}
	}
}