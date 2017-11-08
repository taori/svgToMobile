using System;
using svg.generator.client.console.lib;
using svg.generator.shared;
using svg.generator.shared.Utility;

namespace svg.generator.client.console
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