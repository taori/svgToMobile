using System;

namespace mobile.imagetools.shared.Utility
{
	public interface IConsumableProgress : IProgress<double>, IDisposable { }

	public interface IProgressVisualizerFactory
	{
		IConsumableProgress Create();
	}
}