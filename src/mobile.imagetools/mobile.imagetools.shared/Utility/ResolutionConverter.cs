using System;

namespace mobile.imagetools.shared.Utility
{
	public static class ResolutionConverter
	{
		public static int DpToPixel(int dp, int dpi) => (int)Math.Ceiling((float)dp * ((float)dpi / (float)160));
	}
}