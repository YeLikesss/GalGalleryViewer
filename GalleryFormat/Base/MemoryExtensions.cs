using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GalleryFormat.Base
{
    /// <summary>
    /// 内存方法扩展
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// 获取指针
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="span"></param>
        public unsafe static void* AsPointer<T>(this Span<T> span)
        {
            return Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
        }
    }
}
