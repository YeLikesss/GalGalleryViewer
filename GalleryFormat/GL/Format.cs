using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;

namespace GalleryFormat.GL
{
    /// <summary>
    /// 格式类型
    /// </summary>
    public enum FormatType
    {
        Webp,
        Png,
        Jpeg,
        Tga,
        Bmp,
    }
    /// <summary>
    /// 格式
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// 保存格式
        /// </summary>
        public static FormatType Type { get; set; } = FormatType.Png;
    }

    public static class FormatExtensions
    {
        private static readonly Dictionary<FormatType, ImageEncoder> EncodeProvider;

        static FormatExtensions()
        {
            Dictionary<FormatType, ImageEncoder> encodeProvider = new(8);

            WebpEncoder webp = new()
            {
                FileFormat = WebpFileFormatType.Lossless,
                FilterStrength = 0,
                Method = WebpEncodingMethod.BestQuality,
                NearLossless = false,
                Quality = 100,
                TransparentColorMode = WebpTransparentColorMode.Clear,
                SkipMetadata = true,
            };
            PngEncoder png = new()
            {
                BitDepth = PngBitDepth.Bit8,
                ColorType = PngColorType.RgbWithAlpha,
                CompressionLevel = PngCompressionLevel.BestCompression,
                TransparentColorMode = PngTransparentColorMode.Preserve,
                SkipMetadata = true,
            };
            JpegEncoder jpeg = new()
            {
                ColorType = JpegEncodingColor.YCbCrRatio444,
                Interleaved = false,
                Quality = 100,
                SkipMetadata = true,
            };
            TgaEncoder tga = new()
            {
                BitsPerPixel = TgaBitsPerPixel.Pixel32,
                Compression = TgaCompression.RunLength,
                SkipMetadata = true,
            };
            BmpEncoder bmp = new()
            {
                BitsPerPixel = BmpBitsPerPixel.Pixel32,
                SupportTransparency = true,
                SkipMetadata = true,
            };

            encodeProvider.Add(FormatType.Webp, webp);
            encodeProvider.Add(FormatType.Png, png);
            encodeProvider.Add(FormatType.Jpeg, jpeg);
            encodeProvider.Add(FormatType.Tga, tga);
            encodeProvider.Add(FormatType.Bmp, bmp);
            FormatExtensions.EncodeProvider = encodeProvider;
        }

        /// <summary>
        /// 获取扩展名
        /// </summary>
        public static string GetExtensions(this FormatType type)
        {
            return type switch
            {
                FormatType.Webp => ".webp",
                FormatType.Png => ".png",
                FormatType.Jpeg => ".jpeg",
                _ => string.Empty
            };
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <param name="directory">文件夹</param>
        /// <param name="name">名字</param>
        /// <exception cref="ArgumentException"></exception>
        public static void SaveAs(this Image<Rgba32> img, string directory, string name)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("文件夹为空", nameof(directory));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("名称为空", nameof(name));
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FormatType type = Format.Type;

            string filename = name + type.GetExtensions();
            string path = Path.Combine(directory, filename);

            img.Save(path, FormatExtensions.EncodeProvider[type]);
        }
    }
}
