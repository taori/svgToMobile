using System;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Platform
{
	public abstract class ImagingToolPlatformBase : IToolPlatform
	{
		/// <inheritdoc />
		protected ImagingToolPlatformBase(IFeedbackFactory feedbackFactory)
		{
			FeedbackFactory = feedbackFactory;
		}

		public IToolContext<IToolOptions> CreateContext<T>(T options) where T : class, IToolOptions
		{
			if(FeedbackFactory == null)
				throw new ArgumentException("Feedback factory must be set.", nameof(FeedbackFactory));

			var progressVisualizerFactory = FeedbackFactory.CreateProgressVisualizer(options);
			var lineLogger = FeedbackFactory.CreateLineLogger(options);
			var contentLogger = FeedbackFactory.CreateContentLogger(options);

			IToolContext<IToolOptions> context;
			if (TryCreateContextInternal(options, progressVisualizerFactory, lineLogger, contentLogger, out context))
				return context;

			return null;
		}

		protected virtual bool TryCreateContextInternal<T>(T options, IProgressVisualizerFactory progressVisualizerFactory,
			Action<string> lineLogger, Action<string> contentLogger, out IToolContext<IToolOptions> context) where T : class, IToolOptions
		{
			context = null;
			if (options is T generatorOptions)
			{
				var genericBaseType = typeof(GenericContext<>);
				var genericType = genericBaseType.MakeGenericType(options.GetType());

				try
				{
					context = Activator.CreateInstance(genericType,
						new object[] { generatorOptions, progressVisualizerFactory, lineLogger, contentLogger }) as IToolContext<IToolOptions>;

					return context != null;
				}
				catch (Exception e)
				{
					return false;
				}
			}

			return false;
		}

		/// <inheritdoc />
		public IFeedbackFactory FeedbackFactory { get; }
	}
}