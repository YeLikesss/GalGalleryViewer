using System;
using System.IO;
using System.Text;

namespace GalleryFormat.Base
{
    /// <summary>
    /// 流方法扩展
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 读取ShiftJIS字符串
        /// <para>以'\0'结尾</para>
        /// </summary>
        /// <param name="stream">流</param>
        public static string ReadShiftJISString(this Stream stream)
        {
            long start = stream.Position;
            long length = 0L;

            while (stream.Position < stream.Length)
            {
                if (stream.ReadByte() == 0)
                {
                    length = stream.Position - start;
                    break;
                }
            }

            if (length == 0L)
            {
                return string.Empty;
            }

            byte[] bytes = new byte[length];
            stream.Position = start;
            stream.Read(bytes, 0, (int)length);

            return Encoding.GetEncoding(932).GetString(bytes, 0, (int)length - 1);
        }

        static StreamExtensions()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
