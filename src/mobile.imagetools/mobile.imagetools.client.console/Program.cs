using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using mobile.imagetools.client.console.Platform;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Platform;
using mobile.imagetools.shared.Tools;
using mobile.imagetools.shared.Tools.ImageGenerator;
using mobile.imagetools.shared.Utility;
using static mobile.imagetools.client.console.lib.ConsoleExtensions;
using static System.Console;

namespace mobile.imagetools.client.console
{
	public class Program
	{
		private static readonly Dictionary<string, MobileImagingTool> Tools;

		private static readonly IToolPlatform Platform = new ConsolePlatform();

		private static readonly Regex ToolInvocationsRegex = new Regex("--tool .+?(?= --tool |$)");

		static Program()
		{
			var toolTypes = typeof(MobileImagingTool).Assembly.ExportedTypes.Where(type =>
				typeof(MobileImagingTool).IsAssignableFrom(type) && !type.IsAbstract);

			var tools = new List<MobileImagingTool>();
			foreach (var toolType in toolTypes)
			{
				tools.Add(Activator.CreateInstance(toolType) as MobileImagingTool);
			}

			Tools = tools.ToDictionary(d => d.Name.ToLowerInvariant());
		}

		public static async Task Main(string[] args)
		{
			var joinedArgs = string.Join(" ", args);
			var matches = ToolInvocationsRegex.Matches(joinedArgs);
			if (matches.Count == 0)
			{
				DisplaySupportedToolNames();
			}
			else
			{
				DisplayToolInvocationCalls(matches);
				DisplaySupportedToolNames();
				foreach (Match match in matches)
				{
					await RunToolAsync(match.Value.Split(' '));
				}
			}

#if DEBUG
			if (args.Contains("--interactive") || Debugger.IsAttached)
				Console.ReadKey();
#endif
		}

		private static void DisplayToolInvocationCalls(MatchCollection matches)
		{
			WriteLine($" {matches.Count} calls have been queued for execution.", ConsoleColor.Yellow);
			WriteLine($"");
			foreach (Match match in matches)
			{
				WriteLine($" {match.Value}", ConsoleColor.Yellow);
				WriteLine($"");
			}
			WriteLine($"");
		}

		private static void DisplaySupportedToolNames()
		{
			WriteLine("The following tools are supported: Pick a tool by using: --tool {toolname}", ConsoleColor.Yellow);
			WriteLine("");
			foreach (var tool in Tools.Values)
			{
				WriteLine($" - {tool.Name}", ConsoleColor.Yellow);
			}
			WriteLine("");
		}

		private static async Task RunToolAsync(string[] args)
		{
			var parser = new CommandLine.Parser(p => p.HelpWriter = null);
			var options = ConsoleOptionsFactory.CreateOptions(args).ToArray();
			var parseResults = options.Select(option => new {option, result = parser.ParseArguments(args, option)}).ToArray();

			foreach (var combo in parseResults.Where(d => d.result))
			{
				if (Tools.TryGetValue(combo.option.ToolName.ToLowerInvariant(), out var tool))
				{
					if (tool.TryClaimContext(Platform.CreateContext(combo.option)))
					{
						try
						{
							WriteLine("#", ConsoleColor.Green);
							WriteLine($"# Executing {tool.Name}", ConsoleColor.Green);
							WriteLine($"# {string.Join(" ", args)}", ConsoleColor.Green);
							WriteLine("#", ConsoleColor.Green);
							WriteLine("");

							await tool.ExecuteAsync().ConfigureAwait(false);

							WriteLine("");
							WriteLine("#", ConsoleColor.Green);
							WriteLine($"# {tool.Name} finished successful.", ConsoleColor.Green);
							WriteLine("#", ConsoleColor.Green);
						}
						catch (Exception e)
						{
							WriteLine("");
							WriteLine("#", ConsoleColor.Red);
							WriteLine($"# {tool.Name} crashed with errors.", ConsoleColor.Red);
							WriteLine(e.ToString(), ConsoleColor.Red);
							WriteLine("#", ConsoleColor.Red);
						}

						return;
					}
				}

				WriteLine("");
				WriteLine("");
			}

			foreach (var combo in parseResults.Where(d => !d.result))
			{
				Console.WriteLine(combo.option.Description);
			}
		}
	}
}
