using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace mobile.imagetools.shared.Modules.Generator
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
		public static readonly Regex ColorCodeRegex = new Regex(@"(?:(?<color>#[0-9a-f]{8}|#[0-9a-f]{6}(?![0-9a-f]))(:(?<alias>[\w\d#]+))?;?)");

		public static IEnumerable<ColorInfo> GetColorCodes(this IGeneratorOptions source)
		{
			if(string.IsNullOrEmpty(source.ColorCodes))
				yield break;

			foreach (Match match in ColorCodeRegex.Matches(source.ColorCodes))
			{
				var displayName = match.Groups["alias"].Value == string.Empty ? null : match.Groups["alias"].Value;
				yield return new ColorInfo(match.Groups["color"].Value, displayName);
			}
		}

		public static readonly Regex FormatRegex = new Regex(@"^(?:([\d]+x[\d]+);?)+$");

		public static IEnumerable<FormatInfo> GetImageFormats(this IGeneratorOptions source)
		{
			if(string.IsNullOrEmpty(source.ImageFormats))
				yield break;
			if(!FormatRegex.IsMatch(source.ImageFormats))
				yield break;

			foreach (var formatPair in source.ImageFormats.Split(';'))
			{
				var formatArr = formatPair.Split('x');
				yield return new FormatInfo(int.Parse(formatArr[0]), Int32.Parse(formatArr[1]));

			}
		}

		public static readonly Regex ExtensionRegex = new Regex(@"\.[\w]{3,}");

		public static IEnumerable<string> GetExtensions(this IGeneratorOptions source)
		{
			var extensions = source?.FileExtensions?.Trim();
			if(string.IsNullOrEmpty(extensions))
				yield break;
			if(!ExtensionRegex.IsMatch(extensions))
				yield break;

			foreach (Match match in ExtensionRegex.Matches(source.FileExtensions))
			{
				yield return match.Value;
			}
		}
	}
}