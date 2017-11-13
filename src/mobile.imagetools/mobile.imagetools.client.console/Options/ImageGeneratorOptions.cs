using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using mobile.imagetools.shared.Options;
using mobile.imagetools.shared.Tools.ImageGenerator.Data;

namespace mobile.imagetools.client.console.Options
{
	public class ImageGeneratorOptions : ConsoleOptionBase, IImageGeneratorOptions
	{
		[Option('s', "source",
			Required = true,
			HelpText = "The input paths to be processed.")]
		public string Source { get; set; }

		[Option('d', "destination",
			Required = true,
			HelpText = "The output folder for rendered graphics.")]
		public string Destination { get; set; }

		[Option("skipExisting",
			DefaultValue = false,
			HelpText = "Skip images which already exist.")]
		public bool SkipExisting { get; set; }

		private string _colorCodesRaw;

		[Option('c', "color",
			DefaultValue = null,
			HelpText = "Accepts colors in a pattern like #ffffffx#ffffffff.")]
		public string ColorCodesRaw
		{
			get { return _colorCodesRaw; }
			set
			{
				_colorCodesRaw = value;
				ColorCodes = ConsoleOptionsParsers.Generator.GetColorCodes(value).ToArray();
			}
		}

		private string _imageFormatsRaw;

		[Option('f', "formats",
			Required = true,
			HelpText = "Formats to generate from svg files. Specify like: 32x32 or 32x32;64x64.")]
		public string ImageFormatsRaw
		{
			get { return _imageFormatsRaw; }
			set
			{
				_imageFormatsRaw = value;
				ImageFormats = ConsoleOptionsParsers.Generator.GetImageFormats(value).ToArray();
			}
		}

		private string _fileExtensionsRaw;

		[Option('x', "extensions",
			DefaultValue = ".jpg,.bmp,.png",
			HelpText = "Accepts a pattern of accepted file formats like '.jpg,.png,.gif'.")]
		public string FileExtensionsRaw
		{
			get { return _fileExtensionsRaw; }
			set
			{
				_fileExtensionsRaw = value;
				FileExtensions = ConsoleOptionsParsers.Generator.GetExtensions(value).ToArray();
			}
		}

		[Option('r', "recursive",
			DefaultValue = false,
			HelpText = "Recursive source read mode.")]
		public bool Recursive { get; set; }

		/// <inheritdoc />
		public FormatInfo[] ImageFormats { get; private set; }

		/// <inheritdoc />
		public ColorInfo[] ColorCodes { get; private set; }

		/// <inheritdoc />
		public string[] FileExtensions { get; private set; }

		[Option('a', "aliasMapping",
			HelpText = "Accepts path to alias-mapping csv file.")]
		public string AliasMappingPath { get; set; }

		public Dictionary<string, string> AliasMappings { get; set; }
	}
}