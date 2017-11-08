using System;
using System.Drawing;
using System.Globalization;

namespace svg.generator.shared.Utility
{
	public static class ColorHelper
	{
		public static Color FromArgb(string code)
		{
			if (code.Length != 9 || !code.StartsWith("#"))
				throw new ArgumentException("Argb string should be of length 9 and begin with #.");

			int argb = int.Parse(code.Replace("#", ""), NumberStyles.HexNumber);
			return Color.FromArgb(argb);
		}

		public static Color FromRgb(string code)
		{
			return ColorTranslator.FromHtml(code);
		}
	}
}