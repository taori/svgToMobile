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
				ReadKey();
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
			var parser = new Parser(p => p.HelpWriter = null);
			var options = ConsoleOptionsFactory.CreateOptions(args).ToArray();

			if (options.Length == 0)
			{
				WriteLine("No options to work with.", ConsoleColor.Red);
				return;
			}

			var parsedOptions = options.Select(option => new { option, result = parser.ParseArguments(args, option) }).ToArray();
			var firstOption = options[0];
			if (!Tools.TryGetValue(firstOption.ToolName.ToLowerInvariant(), out var tool))
			{
				WriteLine($"There is no tool which matches the attempted invocation of '{firstOption.ToolName}'.", ConsoleColor.Red);
				return;
			}

			var matchingOption = parsedOptions.FirstOrDefault(d => tool.CanProcessOptions(d.option));
			if (matchingOption == null)
			{
				WriteLine($"There is no tool which can process the given options.", ConsoleColor.Red);
				return;
			}

			if (!matchingOption.result)
			{
				WriteLine($"The tool can't be executed because of a parsing error.", ConsoleColor.Red);
				WriteLine(matchingOption.option.Description);
				return;
			}

			if (tool.TryClaimContext(Platform.CreateContext(matchingOption.option)))
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
				
				WriteLine("");
				WriteLine("");
			}
		}
	}
}
