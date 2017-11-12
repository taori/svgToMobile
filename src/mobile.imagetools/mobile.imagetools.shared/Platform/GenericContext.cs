using System;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Platform
{
	public class GenericContext<TOptions> : ToolContextBase<TOptions> where TOptions : class, IToolOptions
	{
		/// <inheritdoc />
		public GenericContext(TOptions options, IProgressVisualizerFactory progressVisualizerFactory,
			Action<string> logLine, Action<string> log) : base(options, progressVisualizerFactory, logLine, log)
		{
		}
	}
}