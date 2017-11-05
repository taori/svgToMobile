using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using svg.generator.shared;

namespace svg.generator.client.console
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Console args:");
			Console.WriteLine(string.Join(Environment.NewLine, args));

			if (Environment.UserInteractive)
			{
				while (true)
				{
					Console.Clear();
					if (!RunParse(null))
						return;
				}
			}
			else
			{
				RunParse(args);
			}
		}

		private static bool RunParse(string[] args)
		{
			var consoleOptions = new GeneratorOptions();
			Console.WriteLine(consoleOptions.GetUsage());

			if (args == null)
				args = Console.ReadLine().Split(' ');

			var parseSuccess = Parser.Default.ParseArguments(args, consoleOptions);

			if (!parseSuccess)
			{
				Console.WriteLine("One or multiple parsing errors occured.");
				foreach (var error in consoleOptions.LastParserState.Errors)
				{
					Console.WriteLine($"error: {error.BadOption.ShortName}");
				}
				Console.ReadKey();
			}

			if (consoleOptions.Exit)
				return false;

			Execute(consoleOptions);

			if (Environment.UserInteractive)
			{
				Console.WriteLine("done.");
				Console.ReadKey();
			}

			return true;
		}

		private static void Execute(GeneratorOptions consoleOptions)
		{
			var generator = new ImageGenerator(consoleOptions, Console.WriteLine);
			generator.Execute();
		}
	}
}
