using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using mobile.imagetools.shared.Tools;

namespace mobile.imagetools.client.console
{
	public class Program
	{
		private static readonly List<MobileImagingTool> Tools = new List<MobileImagingTool>();

		private static readonly Regex ToolInvocationsRegex = new Regex("-t .+?(?= -t |$)");

		static Program()
		{
			Tools.Add(new ImageGeneratorTool());
		}

		public static async Task Main(string[] args)
		{
			var joinedArgs = string.Join(" ", args);
			var matches = ToolInvocationsRegex.Matches(joinedArgs);
			if (matches.Count == 0)
			{
				Console.WriteLine("The following tools are supported: Pick a tool by using: -t {toolname}");
				Console.WriteLine("");
				foreach (var tool in Tools)
				{
					Console.WriteLine($" - {tool.Name}");
				}
				Console.WriteLine("");
				return;
			}
			else
			{
				foreach (Match match in matches)
				{
					await RunToolAsync(match.Value.Split(' '));
				}
			}
		}

		private static async Task RunToolAsync(string[] args)
		{
			var options = ConsoleOptionsFactory.CreateOptions(args).ToArray();
			var usages = new List<string>();

			foreach (var option in options)
			{
				if (Parser.Default.ParseArguments(args, option))
				{
					foreach (var tool in Tools)
					{
						if (tool.TryClaimContext(ConsoleToolContextFactory.Create(option)))
						{
							try
							{
								await tool.ExecuteAsync().ConfigureAwait(false);
							}
							catch (Exception e)
							{
								Console.WriteLine(e);
							}
							return;
						}
						return;
					}
					return;
				}
				else
				{
					usages.Add(option.Description);
				}
			}
		}
	}
}
