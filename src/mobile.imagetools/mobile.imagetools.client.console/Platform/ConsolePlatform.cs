using mobile.imagetools.shared.Platform;

namespace mobile.imagetools.client.console.Platform
{
	public class ConsolePlatform : ImagingToolPlatformBase
	{
		/// <inheritdoc />
		public ConsolePlatform() :base(new ConsoleFeedbackFactory())
		{
		}
	}
}