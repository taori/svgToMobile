using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mobile.imagetools.shared.Tools;

namespace mobile.imagetools.client.console
{
	public class Program
	{
		private static readonly List<MobileImagingTool> Tools = new List<MobileImagingTool>();

		static Program()
		{
			Tools.Add(new ImageGeneratorTool());
		}

		public static async Task Main(string[] args)
		{
			if (!args.Any(d => d.Contains("-t")))
			{
				Console.WriteLine("The following tools are supported: Pick a tool by using: -t {toolname}");
				Console.WriteLine("");
				foreach (var tool in Tools)
				{
					Console.WriteLine($"-{tool.Name}");
				}
				Console.WriteLine("");
			}

			var contextFactory = new ConsoleContextFactory();
			contextFactory.Arguments = args;
			var context = contextFactory.Create();

			foreach (var tool in Tools)
			{
				if (tool.TryClaimContext(context))
				{
					Console.WriteLine(context.Description);
					try
					{

						await tool.ExecuteAsync().ConfigureAwait(false);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						return;
					}
				}
			}
		}
	}
}
