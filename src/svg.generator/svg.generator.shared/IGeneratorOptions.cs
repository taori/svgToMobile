namespace svg.generator.shared
{
	public interface IGeneratorOptions
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
		/// Whether or not logging is enabled. 
		/// </summary>
		bool LoggingEnabled { get; set; }

		/// <summary>
		/// Formats to generate from svg files. Specify like: 32x32 or 32x32;64x64.
		/// </summary>
		string Formats { get; set; }

		/// <summary>
		/// Whether or not the Source folder is scanned recursively for svg files.
		/// </summary>
		bool Recursive { get; set; }

		/// <summary>
		/// Whether or not the destination folder should be created if it does not exist yet
		/// </summary>
		bool CreateDestinationFolder { get; set; }

		/// <summary>
		/// Whether or not the destination subfolders should be deleted if they already exist
		/// </summary>
		bool SkipExisting { get; set; }
	}
}