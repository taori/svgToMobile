using System.Threading.Tasks;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools
{
	public abstract class MobileImagingTool
	{
		public abstract bool TryClaimContext(IToolContext context);

		public abstract Task<bool> ExecuteAsync();
		
		public abstract string Name { get; }
	}

	public abstract class MobileImagingTool<TContext> : MobileImagingTool
	{
		public TContext Context { get; protected set; }
	}
}