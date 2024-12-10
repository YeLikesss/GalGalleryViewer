using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using GalleryFormat.Base;
using GalleryFormat.Format.Musica;
using GalleryFormat.GL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GalleryFormat
{
    /// <summary>
    /// 测试类
    /// </summary>
    public class UnitTest
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "QueryPerformanceCounter")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "QueryPerformanceFrequency")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        public unsafe static void Entry()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;

            if(true)
            {
                string bgPath = Path.Combine(dir, "あすはポーズ1_2_288.png");
                string fgPath = Path.Combine(dir, "あすはポーズ1_2_299.png");
                Rectangle bgRect = new(414, 232, 253, 207);
                Rectangle fgRect = new(0, 0, 253, 207);
                TestAlphaBlend(bgPath, fgPath, bgRect, fgRect);
            }

            TestAlphaBlend();
            TestPixelConverter();

            if (true)
            {
                string path = Path.Combine(dir, "ev01_1080a01.ani");
                TestAniFile(path);
            }

            if (true)
            {
                string path = Path.Combine(dir, "stw_sara04b01a_01_fg.sqz");
                TestSqzFile(path);
            }

        }

        public static void TestAlphaBlend(string bgPath, string fgPath, in Rectangle bgRect, in Rectangle fgRect)
        {
            void checkFunc(Image<Rgba32> p1, Image<Rgba32> p2, int w, int h, int id)
            {
                p1.ProcessPixelRows(p2, (p1Pixel, p2Pixel) =>
                {
                    bool successedZeroMistake = true;
                    bool successedOneMistake = true;

                    for (int y = 0; y < h; ++y)
                    {
                        Span<Rgba32> ptr1 = p1Pixel.GetRowSpan(y);
                        Span<Rgba32> ptr2 = p2Pixel.GetRowSpan(y);

                        for (int x = 0; x < w; ++x)
                        {
                            Rgba32 pixel1 = ptr1[x];
                            Rgba32 pixel2 = ptr2[x];

                            successedZeroMistake &= pixel1 == pixel2;

                            bool valid = true;
                            valid &= (Math.Abs(pixel1.R - pixel2.R) <= 1);
                            valid &= (Math.Abs(pixel1.G - pixel2.G) <= 1);
                            valid &= (Math.Abs(pixel1.B - pixel2.B) <= 1);
                            valid &= (Math.Abs(pixel1.A - pixel2.A) <= 1);
                            successedOneMistake &= valid;
                        }
                    }

                    if (successedZeroMistake)
                    {
                        Console.WriteLine("AlphaBlend图片 [0误差] {0}:验证通过", id);
                    }
                    else
                    {
                        Console.WriteLine("AlphaBlend图片 [0误差] {0}:验证失败", id);
                    }

                    if (successedOneMistake)
                    {
                        Console.WriteLine("AlphaBlend图片 [+-1误差] {0}:验证通过", id);
                    }
                    else
                    {
                        Console.WriteLine("AlphaBlend图片 [+-1误差] {0}:验证失败", id);
                    }
                });
            }

            using Image bg = Image.Load(bgPath);
            using Image fg = Image.Load(fgPath);

            using Image<Rgba32> destVector = bg.CloneAs<Rgba32>();
            using Image<Rgba32> destScalar = bg.CloneAs<Rgba32>();
            using Image<Rgba32> src = fg.CloneAs<Rgba32>();

            Render.BlendAlpha(destVector, src, bgRect, fgRect, 255, true);
            Render.BlendAlpha(destScalar, src, bgRect, fgRect, 255, false);

            checkFunc(destVector, destScalar, bg.Width, bg.Height, 0);
        }

        public static void TestAlphaBlend()
        {
            const int w = 400;
            const int h = 400;

            Rectangle bgRect = new(0, 0, w, h);
            Rectangle fgRect = new(0, 0, w, h);

            Image<Rgba32> src, dest, destS, destV;
            {
                Rgba32[] srcData = new Rgba32[w * h];
                Rgba32[] destData = new Rgba32[w * h];

                Random ran;
                QueryPerformanceCounter(out long tick);
                ran = new((int)tick);
                for(int i = 0; i < w * h; ++i)
                {
                    srcData[i].Rgba = (uint)ran.Next();
                }
                QueryPerformanceCounter(out tick);
                ran = new((int)tick);
                for (int i = 0; i < w * h; ++i)
                {
                    destData[i].Rgba = (uint)ran.Next();
                }

                src = Image.WrapMemory(new Memory<Rgba32>(srcData), w, h);
                dest = Image.WrapMemory(new Memory<Rgba32>(destData), w, h);
            }

            void checkFunc(Image<Rgba32> p1, Image<Rgba32> p2, int w, int h, int id)
            {
                p1.ProcessPixelRows(p2, (p1Pixel, p2Pixel) =>
                {
                    bool successedZeroMistake = true;
                    bool successedOneMistake = true;

                    for (int y = 0; y < h; ++y)
                    {
                        Span<Rgba32> ptr1 = p1Pixel.GetRowSpan(y);
                        Span<Rgba32> ptr2 = p2Pixel.GetRowSpan(y);

                        for (int x = 0; x < w; ++x)
                        {
                            Rgba32 pixel1 = ptr1[x];
                            Rgba32 pixel2 = ptr2[x];

                            successedZeroMistake &= pixel1 == pixel2;

                            bool valid = true;
                            valid &= (Math.Abs(pixel1.R - pixel2.R) <= 1);
                            valid &= (Math.Abs(pixel1.G - pixel2.G) <= 1);
                            valid &= (Math.Abs(pixel1.B - pixel2.B) <= 1);
                            valid &= (Math.Abs(pixel1.A - pixel2.A) <= 1);
                            successedOneMistake &= valid;
                        }
                    }

                    if (successedZeroMistake)
                    {
                        Console.WriteLine("AlphaBlend随机 [0误差] {0}:验证通过", id);
                    }
                    else
                    {
                        Console.WriteLine("AlphaBlend随机 [0误差] {0}:验证失败", id);
                    }

                    if (successedOneMistake)
                    {
                        Console.WriteLine("AlphaBlend随机 [+-1误差] {0}:验证通过", id);
                    }
                    else
                    {
                        Console.WriteLine("AlphaBlend随机 [+-1误差] {0}:验证失败", id);
                    }
                });
            }

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 0, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 0, true);
            checkFunc(destV, destS, w, h, 1);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 40, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 40, true);
            checkFunc(destV, destS, w, h, 2);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 90, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 90, true);
            checkFunc(destV, destS, w, h, 3);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 127, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 127, true);
            checkFunc(destV, destS, w, h, 4);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 128, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 128, true);
            checkFunc(destV, destS, w, h, 5);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 160, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 160, true);
            checkFunc(destV, destS, w, h, 6);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 210, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 210, true);
            checkFunc(destV, destS, w, h, 7);
            destS.Dispose();
            destV.Dispose();

            destS = dest.Clone();
            destV = dest.Clone();
            Render.BlendAlpha(destS, src, bgRect, fgRect, 255, false);
            Render.BlendAlpha(destV, src, bgRect, fgRect, 255, true);
            checkFunc(destV, destS, w, h, 8);
            destS.Dispose();
            destV.Dispose();

            src.Dispose();
            dest.Dispose();
        }

        public static void TestPixelConverter()
        {
            {
                byte[] src = new byte[0xFFF];
                byte[] destV = new byte[0xFFF];
                byte[] destS = new byte[0xFFF];

                Random ran;
                QueryPerformanceCounter(out long tick);
                ran = new((int)tick);
                ran.NextBytes(src);

                PixelConverter.BGRA32ToRGBA32Scalar(destS, src, src.Length);
                PixelConverter.BGRA32ToRGBA32Vector(destV, src, src.Length);

                if (destV.SequenceEqual(destS))
                {
                    Console.WriteLine("BGRA32ToRGBA32:验证通过");
                }
                else
                {
                    Console.WriteLine("BGRA32ToRGBA32:验证失败");
                }
            }

            {
                byte[] src = new byte[48 * 13 + 47];
                byte[] destV = new byte[64 * 14];
                byte[] destS = new byte[64 * 14];

                Random ran;
                QueryPerformanceCounter(out long tick);
                ran = new((int)tick);
                ran.NextBytes(src);

                PixelConverter.BGR24ToRGBA32Scalar(destS, src, src.Length);
                PixelConverter.BGR24ToRGBA32Vector(destV, src, src.Length);

                if (destV.SequenceEqual(destS))
                {
                    Console.WriteLine("BGR24ToRGBA32:验证通过");
                }
                else
                {
                    Console.WriteLine("BGR24ToRGBA32:验证失败");
                }
            }

            {
                byte[] src = new byte[159];
                byte[] destV = new byte[158 * 4 + 3];
                byte[] destS = new byte[158 * 4 + 3];

                Random ran;
                QueryPerformanceCounter(out long tick);
                ran = new((int)tick);
                ran.NextBytes(src);

                PixelConverter.Gray8ToRGBA32Scalar(destS, src, src.Length);
                PixelConverter.Gray8ToRGBA32Vector(destV, src, src.Length);

                if (destV.SequenceEqual(destS))
                {
                    Console.WriteLine("Gray8ToRGBA32:验证通过");
                }
                else
                {
                    Console.WriteLine("Gray8ToRGBA32:验证失败");
                }
            }

            {
                byte[] src = new byte[159];
                byte[] destV = new byte[158 * 2 + 3];
                byte[] destS = new byte[158 * 2 + 3];

                Random ran;
                QueryPerformanceCounter(out long tick);
                ran = new((int)tick);
                ran.NextBytes(src);

                PixelConverter.BGR565ToRGBA32Scalar(destS, src, src.Length);
                PixelConverter.BGR565ToRGBA32Vector(destV, src, src.Length);

                if (destV.SequenceEqual(destS))
                {
                    Console.WriteLine("BGR565ToRGBA32:验证通过");
                }
                else
                {
                    Console.WriteLine("BGR565ToRGBA32:验证失败");
                }
            }
        }

        public static void TestAniFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string aniName = Path.GetFileNameWithoutExtension(filePath);
                string dir = Path.Combine(Path.GetDirectoryName(filePath)!, aniName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using FileStream fs = File.OpenRead(filePath);

                AniFile ani = new();
                if (ani.TryParse(fs))
                {
                    ReadOnlyCollection<AniImageInfo> imgs = ani.Images;
                    int count = imgs.Count;
                    for(int i = 0; i < count; ++i)
                    {
                        AniImageInfo aniImg = imgs[i];
                        string p = Path.Combine(dir, aniImg.Name) + ".bmp";

                        Memory<byte> mem = new(aniImg.PixelData.ToArray());
                        using Image<Rgba32> image = Image.WrapMemory<Rgba32>(mem, aniImg.Width, aniImg.Height);

                        BmpEncoder enc = new()
                        {
                            BitsPerPixel = BmpBitsPerPixel.Pixel32,
                            SupportTransparency = true,
                        };
                        image.SaveAsBmp(p, enc);
                    }
                    Console.WriteLine("AniFile:验证通过");
                }
            }
        }

        public static void TestSqzFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                BmpEncoder enc = new()
                {
                    BitsPerPixel = BmpBitsPerPixel.Pixel32,
                    SupportTransparency = true,
                };

                string sqzName = Path.GetFileNameWithoutExtension(filePath);
                string dir = Path.Combine(Path.GetDirectoryName(filePath)!, sqzName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string mainPath = Path.ChangeExtension(filePath, ".png");
                if (File.Exists(mainPath))
                {
                    using Image<Rgba32> main = Image.Load<Rgba32>(mainPath);
                    main.SaveAsBmp(Path.Combine(dir, "main.bmp"), enc);
                }


                using FileStream fs = File.OpenRead(filePath);

                SqzFile sqz = new();
                if (sqz.TryParse(fs))
                {
                    List<ReadOnlyCollection<byte>> frames = sqz.Frames;
                    int count = frames.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        ReadOnlyCollection<byte> img = frames[i];
                        string p = $"{dir}\\{i:D4}.bmp";

                        Memory<byte> mem = new(img.ToArray());
                        using Image<Rgba32> image = Image.WrapMemory<Rgba32>(mem, sqz.Width, sqz.Height);

                        image.SaveAsBmp(p, enc);
                    }
                    Console.WriteLine("SqzFile:验证通过");
                }
            }
        }
    }
}
