using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Platform
{
	public interface IToolPlatform
	{
		IToolContext<IToolOptions> CreateContext<T>(T options) where T : class, IToolOptions;

		IFeedbackFactory FeedbackFactory { get; }
	}
}