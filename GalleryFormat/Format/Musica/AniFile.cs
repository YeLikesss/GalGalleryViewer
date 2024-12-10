using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using GalleryFormat.Base;
using GalleryFormat.GL;

namespace GalleryFormat.Format.Musica
{
    /// <summary>
    /// Ani图像信息
    /// </summary>
    public class AniImageInfo
    {
        private readonly string mName = string.Empty;
        private readonly byte[] mPixelData = Array.Empty<byte>();
        private readonly int mWidth;
        private readonly int mHeight;
        private readonly int mOffsetX;
        private readonly int mOffsetY;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name => this.mName;
        /// <summary>
        /// 像素数据(RGBA32)
        /// </summary>
        public ReadOnlyCollection<byte> PixelData => Array.AsReadOnly(this.mPixelData);
        /// <summary>
        /// 宽
        /// </summary>
        public int Width => this.mWidth;
        /// <summary>
        /// 高
        /// </summary>
        public int Height => this.mHeight;
        /// <summary>
        /// X
        /// </summary>
        public int OffsetX => this.mOffsetX;
        /// <summary>
        /// Y
        /// </summary>
        public int OffsetY => this.mOffsetY;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AniImageInfo(string name, byte[] pixelData, int width, int height, int offsetX, int offsetY)
        {
            this.mName = name;
            this.mPixelData = pixelData;
            this.mWidth = width;
            this.mHeight = height;
            this.mOffsetX = offsetX;
            this.mOffsetY = offsetY;
        }
    }

    /// <summary>
    /// Ani差分文件
    /// </summary>
    public class AniFile
    {
        /// <summary>
        /// 文件标志
        /// </summary>
        public const ushort Signature = 0x0100;

        private readonly List<AniImageInfo> mImages = new();
        private bool mIsValid;

        /// <summary>
        /// 图像
        /// </summary>
        public ReadOnlyCollection<AniImageInfo> Images => this.mImages.AsReadOnly();
        /// <summary>
        /// 有效标志
        /// </summary>
        public bool IsValid => this.mIsValid;

        /// <summary>
        /// 尝试解析文件
        /// </summary>
        /// <param name="input">输入流</param>
        /// <returns>True解析成功 False解析失败</returns>
        public bool TryParse(Stream input)
        {
            this.Clear();

            using BinaryReader br = new(input, Encoding.Default, true);
            if(br.ReadUInt16() != AniFile.Signature)
            {
                return false;
            }

            //图片个数
            int count = br.ReadUInt16();

            if (br.ReadUInt32() != 0u)
            {
                return false;
            }

            List<AniImageInfo> imgs = this.mImages;
            imgs.Capacity = count;
            for(int i = 0; i < count; ++i)
            {
                string name = input.ReadShiftJISString();
                if (string.IsNullOrEmpty(name))
                {
                    return false;
                }

                int width = br.ReadUInt16();
                int height = br.ReadUInt16();
                int bpp = br.ReadUInt16();
                int offsetX = br.ReadUInt16();
                int offsetY = br.ReadUInt16();

                byte[] destData = new byte[width * height * 4];

                int orgLen = width * height * bpp / 8;
                byte[] orgData = ArrayPool<byte>.Shared.Rent(orgLen);
                input.Read(orgData, 0, orgLen);
                switch (bpp)
                {
                    case 32:
                    {
                        //BGRA32
                        PixelConverter.BGRA32ToRGBA32Vector(destData, orgData, orgLen);
                        break;
                    }
                    case 24:
                    {
                        //BGR24
                        PixelConverter.BGR24ToRGBA32Vector(destData, orgData, orgLen);
                        break;
                    }
                    case 16:
                    {
                        //BGR565
                        PixelConverter.BGR565ToRGBA32Vector(destData, orgData, orgLen);
                        break;
                    }
                    case 8:
                    {
                        //Gray8
                        PixelConverter.Gray8ToRGBA32Vector(destData, orgData, orgLen);
                        break;
                    }
                    default:
                    {
                        throw new NotImplementedException($"位深:{bpp} 不支持的颜色位数");
                    }
                }
                ArrayPool<byte>.Shared.Return(orgData);

                AniImageInfo img = new(name, destData, width, height, offsetX, offsetY);
                imgs.Add(img);
            }
            this.mIsValid = true;
            return true;
        }

        private void Clear()
        {
            this.mImages.Clear();
            this.mIsValid = false;
        }
    }
}
