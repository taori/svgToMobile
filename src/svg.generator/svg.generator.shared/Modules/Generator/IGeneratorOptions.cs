using System;
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

		/// <summary>
		/// Whether or not the application should be run in non interactive mode.
		/// </summary>
		bool Interactive { get; set; }

		/// <summary>
		/// Description of the options
		/// </summary>
		string Description { get; }
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

		/// <summary>
		/// Accepts pattern like .jpg,.png
		/// </summary>
		string FileExtensions { get; set; }
	}

	public static class GeneratorOptionsExtensions
	{
		public static readonly Regex ColorCodeRegex = new Regex(@"^(?:(#[0-9a-f]{6,8})x?)+$");

		public static IEnumerable<string> GetColorCodes(this IGeneratorOptions source)
		{
			if(string.IsNullOrEmpty(source.ColorCodes))
				yield break;

			foreach (var s in source.ColorCodes.Split('x'))
			{
				yield return s;
			}
		}

		public static readonly Regex FormatRegex = new Regex(@"^(?:([\d]+x[\d]+);?)+$");

		public static IEnumerable<(int width, int height)> GetImageFormats(this IGeneratorOptions source)
		{
			if(string.IsNullOrEmpty(source.ImageFormats))
				yield break;
			if(!FormatRegex.IsMatch(source.ImageFormats))
				yield break;

			foreach (var formatPair in source.ImageFormats.Split(';'))
			{
				var formatArr = formatPair.Split('x');
				yield return (int.Parse(formatArr[0]), Int32.Parse(formatArr[1]));

			}
		}

		public static readonly Regex ExtensionRegex = new Regex(@"^(?:(\.[\w]+),?)+$");

		public static IEnumerable<string> GetExtensions(this IGeneratorOptions source)
		{
			var extensions = source?.FileExtensions?.Trim();
			if(string.IsNullOrEmpty(extensions))
				yield break;
			if(!ExtensionRegex.IsMatch(extensions))
				yield break;

			foreach (var extension in extensions.Split(','))
			{
				yield return extension;

			}
		}
	}
}