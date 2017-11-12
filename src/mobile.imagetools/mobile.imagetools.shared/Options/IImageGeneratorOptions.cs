using mobile.imagetools.shared.Tools.ImageGenerator.Data;

namespace mobile.imagetools.shared.Options
{
	public interface IImageGeneratorOptions : IToolOptions
	{
		/// <summary>
		/// Full path of image source
		/// </summary>
		string Source { get; }

		/// <summary>
		/// Full path of destination
		/// </summary>
		string Destination { get; }

		/// <summary>
		/// Formats to generate from svg files. Specify like: 32x32 or 32x32;64x64.
		/// </summary>
		FormatInfo[] ImageFormats { get; }

		/// <summary>
		/// Whether or not the Source folder is scanned recursively for svg files.
		/// </summary>
		bool Recursive { get; }

		/// <summary>
		/// Whether or not the destination subfolders should be deleted if they already exist.
		/// </summary>
		bool SkipExisting { get; }

		/// <summary>
		/// Color which should be applied to the rendered image.
		/// Accepts pattern like #ffffffx#ffffffff
		/// </summary>
		ColorInfo[] ColorCodes { get; }

		/// <summary>
		/// Accepts pattern like .jpg,.png
		/// </summary>
		string[] FileExtensions { get; }
	}
}