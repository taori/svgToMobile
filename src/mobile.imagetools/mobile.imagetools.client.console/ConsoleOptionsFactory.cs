using System.Collections.Generic;
using mobile.imagetools.client.console.Options;
using mobile.imagetools.shared.Modules.Generator;

namespace mobile.imagetools.client.console
{
	public static class ConsoleOptionsFactory
	{
		public static IEnumerable<IToolOptions> CreateOptions(string[] args)
		{
			yield return new GeneratorOptions();
		}
	}
}