namespace svg.generator.shared.iOS
{
	public class IosContentFileImage
	{
		/// <summary>
		/// e.g. "somefilename_2x.png"
		/// </summary>
		public string filename { get; set; }

		/// <summary>
		/// e.g. "universal"
		/// </summary>
		public string idiom { get; set; }

		/// <summary>
		/// e.g. "2x"
		/// </summary>
		public string scale { get; set; }
	}
}