namespace mobile.imagetools.shared.Options
{
	public interface IUpdateXamarinProjectOptionsBase : IToolOptions
	{
		string CsProjectFilePath { get; }

		string ResourceFolder { get; }

		bool RemoveExcessive { get; }

		bool AddMissing { get; }
	}
}