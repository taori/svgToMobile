using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace svg.generator.shared.Modules.Generator
{
	public interface IToolOptions
	{
		/// <summary>
		/// Name of the tool responsible for these options.
		/// </summary>
		string ToolName { get; set; }

		/// <summary>
		/// Whether or not logging is enabled. 
		/// </summary>
		bool LoggingEnabled { get; set; }
	}

	public interface IGeneratorOptions : IToolOptions
	{
		/// <summary>
		/// Full path of image source
		/// </summary>
		string Source { get; set; }

		/// <summary>
		/// Full path of destination
		/// </summary>
		string Destination { get; set; }

		/// <summary>
		/// Formats to generate from svg files. Specify like: 32x32 or 32x32;64x64.
		/// </summary>
		string ImageFormats { get; set; }

		/// <summary>
		/// Whether or not the Source folder is scanned recursively for svg files.
		/// </summary>
		bool Recursive { get; set; }

		/// <summary>
		/// Whether or not the destination subfolders should be deleted if they already exist.
		/// </summary>
		bool SkipExisting { get; set; }

		/// <summary>
		/// Color which should be applied to the rendered image.
		/// Accepts pattern like #ffffffx#ffffffff
		/// </summary>
		string ColorCodes { get; set; }
	}

	public static class GeneratorOptionsExtensions
	{
		private static readonly Regex ColorCodeRegex = new Regex(@"(?:(#[0-9a-f]{6,8})x?)+");

		public static IEnumerable<string> GetColorCodes(this IGeneratorOptions source)
		{
			if(string.IsNullOrEmpty(source.ColorCodes))
				yield break;

			foreach (var s in source.ColorCodes.Split('x'))
			{
				yield return s;
			}
		}
	}
}