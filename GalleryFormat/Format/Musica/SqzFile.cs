using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using GalleryFormat.GL;

namespace GalleryFormat.Format.Musica
{
    /// <summary>
    /// Sqz文件
    /// </summary>
    public class SqzFile
    {
        private struct Entry
        {
            public uint Offset;
            public uint Size;
        }

        /// <summary>
        /// 文件标志
        /// </summary>
        public const uint Signature = 0x315A5153;

        private readonly List<byte[]> mFrames = new();
        private int mWidth;
        private int mHeight;
        private int mFPS;
        private bool mIsValid;

        /// <summary>
        /// 帧像素
        /// </summary>
        public List<ReadOnlyCollection<byte>> Frames
        {
            get
            {
                return this.mFrames.ConvertAll(bytes => Array.AsReadOnly(bytes));
            }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width => this.mWidth;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height => this.mHeight;
        /// <summary>
        /// 帧率
        /// </summary>
        public int FPS => this.mFPS;
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
            if (br.ReadUInt32() != SqzFile.Signature)
            {
                return false;
            }

            int frameCount = br.ReadInt32();
            int width = br.ReadInt32();
            int height = br.ReadInt32();
            int fps = br.ReadInt32();

            //读取帧数据
            Entry[] entries = new Entry[frameCount];
            for(int i = 0; i < frameCount; ++i)
            {
                entries[i].Offset = br.ReadUInt32();
                entries[i].Size = br.ReadUInt32();
            }

            List<byte[]> frames = this.mFrames;
            frames.Capacity = frameCount;
            for(int i = 0; i < frameCount; ++i)
            {
                input.Position = entries[i].Offset;
                int orgLen = (int)entries[i].Size;

                byte[] orgData = ArrayPool<byte>.Shared.Rent(orgLen);

                input.Read(orgData, 0, orgLen);

                using MemoryStream orgMs = new(orgData, 0, orgLen, false);
                using ZLibStream zLib = new(orgMs, CompressionMode.Decompress);

                int destLen = width * height * 4;
                using MemoryStream outMs = new(destLen);
                zLib.CopyTo(outMs);
                outMs.Position = 0L;

                byte[] destData = new byte[destLen];
                outMs.Read(destData, 0, destLen);

                PixelConverter.BGRA32ToRGBA32Vector(destData, destData, destLen);
                frames.Add(destData);

                ArrayPool<byte>.Shared.Return(orgData);
            }

            this.mWidth = width;
            this.mHeight = height;
            this.mFPS = fps;
            this.mIsValid = true;
            return true;
        }

        private void Clear()
        {
            this.mFrames.Clear();
            this.mWidth = 0;
            this.mHeight = 0;
            this.mFPS = 0;
            this.mIsValid = false;
        }
    }
}
