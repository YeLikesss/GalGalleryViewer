using System;
using System.Drawing;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using GalleryFormat.GL;

namespace GalleryViewerForm.Base
{
    /// <summary>
    /// Imagesharp扩展
    /// </summary>
    public static class ImageSharpExtensions
    {
        /// <summary>
        /// 转Bitmap
        /// </summary>
        /// <param name="img">ImageSharp RGBA32图像</param>
        public static unsafe Bitmap ToBitmap(this Image<Rgba32> img)
        {
            int pixelDataLen = img.Width * img.Height * 4;

            Bitmap bmp = new (img.Width, img.Height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Span<byte> pixelPtr = new (bmpData.Scan0.ToPointer(), pixelDataLen);

            img.CopyPixelDataTo(pixelPtr);
            PixelConverter.RGBA32ToBGRA32Vector(pixelPtr, pixelPtr, pixelDataLen);

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
