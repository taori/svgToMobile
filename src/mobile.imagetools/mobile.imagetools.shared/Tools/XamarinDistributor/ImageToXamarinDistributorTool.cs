using System.IO;
using System.Threading.Tasks;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Utility;

namespace mobile.imagetools.shared.Tools.XamarinDistributor
{
	public class ImageToXamarinDistributorTool : MobileImagingTool<IImageToXamarinDistributorOptions>
	{
		/// <inheritdoc />
		public override async Task<bool> ExecuteAsync()
		{
			if (!Directory.Exists(Context.Options.ModernPackageFolder))
			{
				Context.LogLine($"{Context.Options.ModernPackageFolder} does not exist.");
				return false;
			}

			if (!string.IsNullOrEmpty(Context.Options.AndroidResourceFolder))
				MoveAndroidFolders();
			if (!string.IsNullOrEmpty(Context.Options.IosResourceFolder))
				MoveIosFolders();

			return false;
		}

		private void MoveIosFolders()
		{
			// svggeneration\appbar_3d_3ds_32x32dp_white\ios\appbar_3d_3ds_32x32dp_white.imageset
			var directories = Directory.GetDirectories(Context.Options.ModernPackageFolder);
			Context.LogLine($"Distributing {directories.Length} folders for iOS.");

			using (var progress = Context.ProgressVisualizerFactory.Create())
			{
				for (var directoryIndex = 0; directoryIndex < directories.Length; directoryIndex++)
				{
					progress.Report(((float) directoryIndex + 1) / directories.Length);

					var set = directories[directoryIndex];
					foreach (var platform in Directory.GetDirectories(set))
					{
						if (!platform.EndsWith($"{Path.DirectorySeparatorChar}ios"))
							continue;

						foreach (var imageSet in Directory.GetDirectories(platform))
						{
							var imageSetName = Path.GetFileName(imageSet);

							var destination = Path.Combine(Context.Options.IosResourceFolder, imageSetName);

							if (Directory.Exists(destination) && Context.Options.DeleteExisting)
								Directory.Delete(destination, true);

							IoHelper.CreateDirectoryRecursive(destination);

							foreach (var file in Directory.GetFiles(imageSet))
							{
								var fileName = Path.GetFileName(file);
								var sourcePath = Path.Combine(imageSet, fileName);
								var destinationPath = Path.Combine(destination, fileName);

								if (File.Exists(destinationPath) && Context.Options.DeleteExisting)
									File.Delete(destinationPath);

								File.Copy(sourcePath, destinationPath);
							}
						}
					}
				}
			}
		}

		private void MoveAndroidFolders()
		{
			// svggeneration\appbar_3d_3ds_32x32dp_white\android\drawable-hdpi

			var directories = Directory.GetDirectories(Context.Options.ModernPackageFolder);
			Context.LogLine($"Distributing {directories.Length} folders for Android.");

			using (var progress = Context.ProgressVisualizerFactory.Create())
			{
				for (var directoryIndex = 0; directoryIndex < directories.Length; directoryIndex++)
				{
					progress.Report(((float)directoryIndex + 1) / directories.Length);

					var set = directories[directoryIndex];
					foreach (var platform in Directory.GetDirectories(set))
					{
						if (!platform.EndsWith($"{Path.DirectorySeparatorChar}android"))
							continue;

						foreach (var resolutionFolder in Directory.GetDirectories(platform))
						{
							var resolutionName = Path.GetFileName(resolutionFolder);

							var destination = Path.Combine(Context.Options.AndroidResourceFolder, resolutionName);

							IoHelper.CreateDirectoryRecursive(destination);

							foreach (var imageFilePath in Directory.GetFiles(resolutionFolder))
							{
								var fullDestinationPath = Path.Combine(destination, Path.GetFileName(imageFilePath));
								if (File.Exists(fullDestinationPath))
									File.Delete(fullDestinationPath);

								File.Copy(imageFilePath, fullDestinationPath);
							}
						}
					}
				}
			}
		}

		/// <inheritdoc />
		public override string Name => "ImageToXamarinDistributor";
	}
}