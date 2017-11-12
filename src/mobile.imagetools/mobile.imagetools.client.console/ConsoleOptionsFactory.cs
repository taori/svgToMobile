using System;
using System.Collections.Generic;
using System.Linq;
using mobile.imagetools.client.console.Options;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools;

namespace mobile.imagetools.client.console
{
	public static class ConsoleOptionsFactory
	{
		public static IEnumerable<IToolOptions> CreateOptions(string[] args)
		{
			var toolTypes = typeof(ConsoleOptionBase).Assembly.ExportedTypes.Where(type =>
				typeof(ConsoleOptionBase).IsAssignableFrom(type) && !type.IsAbstract);
			foreach (var type in toolTypes)
			{
				yield return Activator.CreateInstance(type) as IToolOptions;
			}
		}
	}
}