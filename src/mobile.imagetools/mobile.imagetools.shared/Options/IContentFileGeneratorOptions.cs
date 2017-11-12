namespace mobile.imagetools.shared.Options
{
	public enum ContentFileMode
	{
		ModernUiPackage = 0,
		IosAssetFolder = 1,
	}

	public interface IContentFileGeneratorOptions : IToolOptions
	{
		string SourceFolder { get; }

		ContentFileMode Mode { get; }
	}
}