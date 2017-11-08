using System;

namespace svg.generator.shared.Utility
{
	public interface IConsumableProgress : IProgress<double>, IDisposable { }

	public interface IProgressVisualizerFactory
	{
		IConsumableProgress Create();
	}
}