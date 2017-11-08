using svg.generator.shared.Modules.Generator;

namespace svg.generator.shared.Utility
{
	public interface IToolContext
	{
	}

	public interface IToolContext<out TOption> : IToolContext where TOption : IToolOptions
	{
		TOption Options { get; }
	}
}