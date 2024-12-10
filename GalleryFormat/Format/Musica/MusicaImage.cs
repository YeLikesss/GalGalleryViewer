using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using GalleryFormat.GL;
using GalleryFormat.Base;

namespace GalleryFormat.Format.Musica
{
    /// <summary>
    /// Musica图像类型
    /// </summary>
    public enum MusicaImageType
    {
        Normal,
        Ani,
        Sqz,
    }

    /// <summary>
    /// Musica图像接口
    /// </summary>
    public interface IMusicaImage
    {
        /// <summary>
        /// 图像类型
        /// </summary>
        public MusicaImageType Type { get; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// 获取图像
        /// </summary>
        public Image<Rgba32> GetImage();
    }

    /// <summary>
    /// 普通图像
    /// </summary>
    public class NormalImage : IMusicaImage
    {
        protected readonly byte[] mMainImage;
        protected readonly string mName;
        protected readonly int mWidth;
        protected readonly int mHeight;

        public virtual MusicaImageType Type => MusicaImageType.Normal;
        public string Name => this.mName;
        public int Width => this.mWidth;
        public int Height => this.mHeight;

        public virtual Image<Rgba32> GetImage()
        {
            return Image.WrapMemory<Rgba32>((byte[])this.mMainImage.Clone(), this.mWidth, this.mHeight);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainImage">主视图像素数组(RGBA32)</param>
        /// <param name="name">名称</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public NormalImage(byte[] mainImage, string name, int width, int height)
        {
            this.mMainImage = mainImage;
            this.mName = name;
            this.mWidth = width;
            this.mHeight = height;
        }
    }

    /// <summary>
    /// Ani图像
    /// </summary>
    public sealed class AniImage : NormalImage
    {
        private readonly AniFile mAni;
        private readonly bool[] mSelectedIndices;

        public override MusicaImageType Type => MusicaImageType.Ani;

        /// <summary>
        /// 差分帧
        /// </summary>
        public ReadOnlyCollection<AniImageInfo> Frames => this.mAni.Images;
        /// <summary>
        /// 差分帧索引
        /// </summary>
        public bool[] SelectedIndices => this.mSelectedIndices;

        public override Image<Rgba32> GetImage()
        {
            Image<Rgba32> dest = base.GetImage();
            for(int i = 0; i < this.mSelectedIndices.Length; ++i)
            {
                if (this.mSelectedIndices[i])
                {
                    AniImageInfo info = this.mAni.Images[i];

                    using Image<Rgba32> fg = Image.WrapMemory<Rgba32>(info.PixelData.ToArray(), info.Width, info.Height);
                    //绘图位置
                    Rectangle bgRect = new(info.OffsetX, info.OffsetY, info.Width, info.Height);

                    Render.BlendAlpha(dest, fg, bgRect, fg.Bounds, 255);
                }
            }
            return dest;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainImage">主视图像素数组(RGBA32)</param>
        /// <param name="name">名称</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="ani">ani文件</param>
        public AniImage(byte[] mainImage, string name, int width, int height, AniFile ani)
            : base(mainImage, name, width, height)
        {
            this.mAni = ani;
            this.mSelectedIndices = new bool[ani.Images.Count];
        }
    }

    /// <summary>
    /// Sqz图像
    /// </summary>
    public sealed class SqzImage : NormalImage
    {
        private readonly SqzFile mSqz;
        private int mSelectedIndex;

        public override MusicaImageType Type => MusicaImageType.Sqz;

        /// <summary>
        /// 图像帧
        /// </summary>
        public ReadOnlyCollection<ReadOnlyCollection<byte>> Frames => this.mSqz.Frames.AsReadOnly();
        /// <summary>
        /// 帧索引
        /// <para>帧索引 主视图为-1</para>
        /// </summary>
        public int SelectedIndex
        {
            get => this.mSelectedIndex;
            set
            {
                if (value < 0 || value >= this.mSqz.Frames.Count)
                {
                    this.mSelectedIndex = -1;
                }
                else
                {
                    this.mSelectedIndex = value;
                }
            }
        }

        public override Image<Rgba32> GetImage()
        {
            int idx = this.mSelectedIndex;
            SqzFile sqz = this.mSqz;
            if (idx == -1)
            {
                return base.GetImage();
            }
            else
            {
                return Image.WrapMemory<Rgba32>(sqz.Frames[idx].ToArray(), sqz.Width, sqz.Height);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainImage">主视图像素数组(RGBA32)</param>
        /// <param name="name">名称</param>
        /// <param name="sqz">sqz图像文件</param>
        public SqzImage(byte[] mainImage, string name, SqzFile sqz) 
                 : base(mainImage, name, sqz.Width, sqz.Height)
        {
            this.mSqz = sqz;
            this.mSelectedIndex = -1;
        }
    }

    /// <summary>
    /// Musica图像工厂
    /// </summary>
    public static class MusicaImageFactory
    {
        private static readonly string[] Filters = new string[] { "*.png" };
        private static readonly string[] Extensions = new string[] { ".png" };
        private static readonly string AniExtensions = ".ani";
        private static readonly string SqzExtensions = ".sqz";

        /// <summary>
        /// 导出路径
        /// </summary>
        public static string OutputDirectory { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Musica_Output");

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="directory">文件夹全路径</param>
        /// <returns>文件路径列表</returns>
        public static List<string> GetFiles(string directory)
        {
            List<string> files = new(1024);
            if (Directory.Exists(directory))
            {
                foreach (string filter in MusicaImageFactory.Filters)
                {
                    files.AddRange(Directory.GetFiles(directory, filter, SearchOption.TopDirectoryOnly));
                }
            }
            return files;
        }

        /// <summary>
        /// 创建图像实例
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static IMusicaImage? CreateInstance(string filePath)
        {
            if (!MusicaImageFactory.Extensions.Contains(Path.GetExtension(filePath)))
            {
                return null;
            }
            if (!File.Exists(filePath))
            {
                return null;
            }

            string name = Path.GetFileNameWithoutExtension(filePath);
            byte[] mainImg;
            int width, height;

            //加载主视图
            {
                using Image<Rgba32> img = Image.Load<Rgba32>(filePath);
                width = img.Width;
                height = img.Height;
                mainImg = new byte[width * height * 4];
                img.CopyPixelDataTo(mainImg);
            }
            
            string ani = Path.ChangeExtension(filePath, MusicaImageFactory.AniExtensions);
            string sqz = Path.ChangeExtension(filePath, MusicaImageFactory.SqzExtensions);
            if (File.Exists(ani))
            {
                //ani差分立绘
                using FileStream aniFs = File.OpenRead(ani);
                AniFile aniFile = new();
                if (aniFile.TryParse(aniFs))
                {
                    return new AniImage(mainImg, name, width, height, aniFile);
                }
            }
            else if (File.Exists(sqz))
            {
                //sqz动画立绘
                using FileStream sqzFs = File.OpenRead(sqz);
                SqzFile sqzFile = new();
                if (sqzFile.TryParse(sqzFs))
                {
                    return new SqzImage(mainImg, name, sqzFile);
                }
            }
            else
            {
                return new NormalImage(mainImg, name, width, height);
            }
            return null;
        }

        /// <summary>
        /// 合成图像
        /// </summary>
        /// <param name="musicaImages">图像信息</param>
        public static Image<Rgba32>? GetImage(List<IMusicaImage> musicaImages)
        {
            if(!musicaImages.Any())
            {
                return null;
            }

            Image<Rgba32> dest = musicaImages[0].GetImage();
            for(int i = 1; i < musicaImages.Count; ++i)
            {
                using Image<Rgba32> fg = musicaImages[i].GetImage();
                Render.BlendAlpha(dest, fg, dest.Bounds, fg.Bounds, 255);
            }
            return dest;
        }

        /// <summary>
        /// 提取图像
        /// </summary>
        /// <param name="musicaImages">图像信息</param>
        public static bool Extract(List<IMusicaImage> musicaImages, IProgress<ProgressInfo>? progress = null)
        {
            using Image<Rgba32>? dest = MusicaImageFactory.GetImage(musicaImages);
            if(dest is null)
            {
                return false;
            }

            progress?.Report(new(ProgressValueType.AbsoluteCountAndReset, 1));

            string dir = Path.Combine(MusicaImageFactory.OutputDirectory, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            dest.SaveAs(dir, "Single");

            progress?.Report(new(ProgressValueType.Relative, 1));

            return true;
        }

        /// <summary>
        /// 提取图像
        /// <para>异步多线程</para>
        /// </summary>
        /// <param name="musicaImages">图像信息</param>
        /// <param name="progress">进度信息</param>
        public static Task<bool> ExtractAsync(List<IMusicaImage> musicaImages, IProgress<ProgressInfo>? progress = null)
        {
            return Task.Run(() =>
            {
                return MusicaImageFactory.Extract(musicaImages, progress);
            });
        }


        /// <summary>
        /// 提取图像(所有帧)
        /// </summary>
        /// <param name="musicaImages">图像信息</param>
        /// <param name="progress">进度回调</param>
        public static Task<bool> ExtractAllAsync(List<IMusicaImage> musicaImages, IProgress<ProgressInfo>? progress = null)
        {
            if (!musicaImages.Any())
            {
                return Task.FromResult(false);
            }

            return Task.Run(() => 
            {
                string directory = Path.Combine(MusicaImageFactory.OutputDirectory, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                int layerCount = musicaImages.Count;        //图层数
                int pictureCount = 1;                       //图片数

                //排列组合所有帧
                for (int i = 0; i < layerCount; ++i)
                {
                    IMusicaImage img = musicaImages[i];
                    int differenceCount = img.Type switch
                    {
                        MusicaImageType.Sqz => ((SqzImage)img).Frames.Count,
                        _ => 1,
                    };
                    pictureCount *= differenceCount;
                }

                progress?.Report(new(ProgressValueType.AbsoluteCountAndReset, pictureCount));

                ParallelLoopResult loopResult = Parallel.For(0, pictureCount, picIdx =>
                {
                    Image<Rgba32>[] layers = new Image<Rgba32>[layerCount];
                    int layerIdx = layerCount - 1;
                    int picPos = picIdx;

                    //选择各图层帧
                    do
                    {
                        IMusicaImage img = musicaImages[layerIdx];
                        switch (img.Type)
                        {
                            case MusicaImageType.Normal:
                            case MusicaImageType.Ani:
                            {
                                layers[layerIdx] = img.GetImage();
                                break;
                            }
                            case MusicaImageType.Sqz:
                            {
                                SqzImage sqz = (SqzImage)img;

                                sqz.SelectedIndex = picPos % sqz.Frames.Count;
                                layers[layerIdx] = sqz.GetImage();

                                picPos /= sqz.Frames.Count;
                                break;
                            }
                        }
                    }
                    while (--layerIdx >= 0);

                    //合成
                    using Image<Rgba32> dest = layers[0];
                    for (int j = 1; j < layerCount; ++j)
                    {
                        using Image<Rgba32> fg = layers[j];
                        Render.BlendAlpha(dest, fg, dest.Bounds, fg.Bounds, 255);
                    }
                    dest.SaveAs(directory, $"{picIdx:D4}");

                    progress?.Report(new(ProgressValueType.Relative, 1));
                });
                return loopResult.IsCompleted;
            });
        }
    }
}
