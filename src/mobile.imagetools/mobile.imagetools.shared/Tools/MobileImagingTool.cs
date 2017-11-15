using System.Threading.Tasks;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools
{
	public abstract class MobileImagingTool
	{
		public abstract bool CanProcessOptions(IToolOptions options);

		public abstract bool TryClaimContext(IToolContext context);

		public abstract Task<bool> ExecuteAsync();
		
		public abstract string Name { get; }
	}

	public abstract class MobileImagingTool<TOptions> : MobileImagingTool where TOptions : IToolOptions
	{
		public IToolContext<TOptions> Context { get; protected set; }

		/// <inheritdoc />
		public override bool CanProcessOptions(IToolOptions options)
		{
			return options is TOptions;
		}

		/// <inheritdoc />
		public sealed override bool TryClaimContext(IToolContext context)
		{
			if (context is IToolContext<TOptions> c)
			{
				Context = c;
				return true;
			}
			return false;
		}
	}
}