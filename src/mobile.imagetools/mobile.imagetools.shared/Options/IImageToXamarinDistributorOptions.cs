namespace mobile.imagetools.shared.Options
{
	public interface IImageToXamarinDistributorOptions : IToolOptions
	{
		string ModernPackageFolder { get; }

		string IosResourceFolder { get; }

		string AndroidResourceFolder { get; }

		bool DeleteExisting { get; }
	}
}