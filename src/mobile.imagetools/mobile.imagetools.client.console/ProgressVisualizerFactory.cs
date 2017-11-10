using mobile.imagetools.client.console.lib;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.client.console
{
	public class ProgressVisualizerFactory : IProgressVisualizerFactory
	{
		/// <inheritdoc />
		public IConsumableProgress Create()
		{
			return new ProgressBar();
		}
	}
}