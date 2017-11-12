using System;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools
{
	public abstract class ToolContextBase<TOptions> : IToolContext<TOptions> where TOptions : class, IToolOptions
	{
		/// <inheritdoc />
		protected ToolContextBase(TOptions options, IProgressVisualizerFactory progressVisualizerFactory, Action<string> logLine, Action<string> log)
		{
			Options = options ?? throw new ArgumentNullException(nameof(options));
			LogLine = logLine ?? throw new ArgumentNullException(nameof(logLine));
			Log = log ?? throw new ArgumentNullException(nameof(log));
			ProgressVisualizerFactory = progressVisualizerFactory ?? throw new ArgumentNullException(nameof(progressVisualizerFactory));
		}

		/// <inheritdoc />
		public TOptions Options { get; }

		/// <inheritdoc />
		public Action<string> LogLine { get; }

		/// <inheritdoc />
		public Action<string> Log { get; }

		/// <inheritdoc />
		public IProgressVisualizerFactory ProgressVisualizerFactory { get; }

		/// <inheritdoc />
		public virtual string Description => Options.Description;
	}
}