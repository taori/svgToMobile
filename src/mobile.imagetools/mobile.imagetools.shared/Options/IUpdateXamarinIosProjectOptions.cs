namespace mobile.imagetools.shared.Options
{
	public interface IUpdateXamarinIosProjectOptions : IToolOptions
	{
		string CsProjectFilePath { get; }

		string ImageSetFolderPattern { get; }
	}
}