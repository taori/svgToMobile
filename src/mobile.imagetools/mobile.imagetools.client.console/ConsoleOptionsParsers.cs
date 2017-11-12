using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using mobile.imagetools.shared.Tools.ImageGenerator.Data;

namespace mobile.imagetools.client.console
{
	public static class ConsoleOptionsParsers
	{
		public static class Generator
		{
			public static readonly Regex ColorCodeRegex = new Regex(@"(?:(?<color>#[0-9a-f]{8}|#[0-9a-f]{6}(?![0-9a-f]))(:(?<alias>[\w\d#]+))?;?)");

			public static IEnumerable<ColorInfo> GetColorCodes(string source)
			{
				if (string.IsNullOrEmpty(source))
					yield break;

				foreach (Match match in ColorCodeRegex.Matches(source))
				{
					var displayName = match.Groups["alias"].Value == string.Empty ? null : match.Groups["alias"].Value;
					yield return new ColorInfo(match.Groups["color"].Value, displayName);
				}
			}

			public static readonly Regex FormatRegex = new Regex(@"^(?:([\d]+x[\d]+);?)+$");

			public static IEnumerable<FormatInfo> GetImageFormats(string source)
			{
				if (string.IsNullOrEmpty(source))
					yield break;
				if (!FormatRegex.IsMatch(source))
					yield break;

				foreach (var formatPair in source.Split(';'))
				{
					var formatArr = formatPair.Split('x');
					yield return new FormatInfo(int.Parse(formatArr[0]), Int32.Parse(formatArr[1]));

				}
			}

			public static readonly Regex ExtensionRegex = new Regex(@"\.[\w]{3,}");

			public static IEnumerable<string> GetExtensions(string source)
			{
				var extensions = source?.Trim();
				if (string.IsNullOrEmpty(extensions))
					yield break;
				if (!ExtensionRegex.IsMatch(extensions))
					yield break;

				foreach (Match match in ExtensionRegex.Matches(source))
				{
					yield return match.Value;
				}
			}
		}
	}
}