using System;

namespace mobile.imagetools.client.console.lib
{
	public static class ConsoleExtensions
	{
		public static void WriteLine(string message, ConsoleColor foreground)
		{
			var previous = Console.ForegroundColor;
			Console.ForegroundColor = foreground;
			Console.WriteLine(message);
			Console.ForegroundColor = previous;
		}

		public static void Write(string message, ConsoleColor foreground)
		{
			var previous = Console.ForegroundColor;
			Console.ForegroundColor = foreground;
			Console.Write(message);
			Console.ForegroundColor = previous;
		}
	}
}