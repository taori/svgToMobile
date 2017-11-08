using System.IO;

namespace svg.generator.shared.Utility
{
	public static class FileNameHelper
	{
		public static string GetSanitizedAssetName(int width, int height, string sourceFile, string colorCode)
		{
			// someimage.svg -> someimage_24x24dp
			return $"{Path.GetFileNameWithoutExtension(sourceFile)?.Replace('.','_')}_{width}x{height}dp_{colorCode}";
		}

		public static string GetSanitizedAssetName(int width, int height, string sourceFile)
		{
			// someimage.svg -> someimage_24x24dp
			return $"{Path.GetFileNameWithoutExtension(sourceFile)?.Replace('.','_')}_{width}x{height}dp";
		}
	}
}