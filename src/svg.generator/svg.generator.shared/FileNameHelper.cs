using System.IO;

namespace svg.generator.shared
{
	public static class FileNameHelper
	{
		public static string GetFirstLevelFolderName(int width, int height, string sourceFile)
		{
			// someimage.svg -> someimage_24x24dp
			return $"{Path.GetFileNameWithoutExtension(sourceFile)}_{width}x{height}dp";
		}

		public static string GetFileName(int width, int height, string sourceFile)
		{
			return $"{GetFirstLevelFolderName(width, height, sourceFile)}";
		}
	}
}