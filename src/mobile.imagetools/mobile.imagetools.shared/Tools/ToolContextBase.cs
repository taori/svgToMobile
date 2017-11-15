using System;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools
{
	public abstract class ToolContextBase<TOptions> : IToolContext<TOptions> where TOptions : class, IToolOptions
	{
		private Action<string> _logLine;
		private Action<string> _log;

		/// <inheritdoc />
		protected ToolContextBase(TOptions options, IProgressVisualizerFactory progressVisualizerFactory, Action<string> logLine, Action<string> log)
		{
			Options = options ?? throw new ArgumentNullException(nameof(options));
			ProgressVisualizerFactory = progressVisualizerFactory ?? throw new ArgumentNullException(nameof(progressVisualizerFactory));
			_logLine = logLine ?? throw new ArgumentNullException(nameof(logLine));
			_log = log ?? throw new ArgumentNullException(nameof(log));
		}

		/// <inheritdoc />
		public TOptions Options { get; }
		

		/// <inheritdoc />
		public void LogLine(string message, bool important = false)
		{
			if (important || Options.Verbose)
				_logLine(message);
		}

		/// <inheritdoc />
		public void Log(string message, bool important = false)
		{
			if (important || Options.Verbose)
				_log(message);
		}

		/// <inheritdoc />
		public IProgressVisualizerFactory ProgressVisualizerFactory { get; }

		/// <inheritdoc />
		public virtual string Description => Options.Description;
	}
}