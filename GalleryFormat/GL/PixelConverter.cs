using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using GalleryFormat.Base;

namespace GalleryFormat.GL
{
    /// <summary>
    /// 像素转换器
    /// </summary>
    public class PixelConverter
    {
        /// <summary>
        /// Gray8转RGBA32
        /// <para>1Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void Gray8ToRGBA32Vector(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length / 4, src.Length), length);

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();

            Vector256<byte> selector3_0, selector7_4, selector11_8, selector15_12, alpha;
            {
                // 0F  0E  0D  0C  0B  0A  09  08  07  06  05  04  03  02  01  00  --> 源
                // 
                //
                // 80  03  03  03  80  02  02  02  80  01  01  01  80  00  00  00  --> 像素3..0
                //
                // 80  07  07  07  80  06  06  06  80  05  05  05  80  04  04  04  --> 像素7..4
                //
                // 80  0B  0B  0B  80  0A  0A  0A  80  09  09  09  80  08  08  08  --> 像素11..8
                //
                // 80  0F  0F  0F  80  0E  0E  0E  80  0D  0D  0D  80  0C  0C  0C  --> 像素15..12

                ulong lo, hi;

                lo = 0x8001010180000000ul;
                hi = 0x8003030380020202ul;
                selector3_0 = Vector256.Create(lo, hi, lo, hi).AsByte();

                lo = 0x8005050580040404ul;
                hi = 0x8007070780060606ul;
                selector7_4 = Vector256.Create(lo, hi, lo, hi).AsByte();

                lo = 0x8009090980080808ul;
                hi = 0x800B0B0B800A0A0Aul;
                selector11_8 = Vector256.Create(lo, hi, lo, hi).AsByte();

                lo = 0x800D0D0D800C0C0Cul;
                hi = 0x800F0F0F800E0E0Eul;
                selector15_12 = Vector256.Create(lo, hi, lo, hi).AsByte();

                alpha = Vector256.Create(0xFF000000u).AsByte();
            }

            //1路ymm
            while (length >= 32)
            {
                Vector256<byte> ymm31_0 = Avx2.LoadVector256(srcPtr);

                Vector256<byte> ymm19_16__3_0, ymm23_20__7_4, ymm27_24__11_8, ymm31_28__15_12;

                //vpshufb ymm, ymm, ymm
                ymm19_16__3_0 = Avx2.Shuffle(ymm31_0, selector3_0);
                ymm23_20__7_4 = Avx2.Shuffle(ymm31_0, selector7_4);
                ymm27_24__11_8 = Avx2.Shuffle(ymm31_0, selector11_8);
                ymm31_28__15_12 = Avx2.Shuffle(ymm31_0, selector15_12);

                //vpor ymm, ymm, ymm
                ymm19_16__3_0 = Avx2.Or(ymm19_16__3_0, alpha);
                ymm23_20__7_4 = Avx2.Or(ymm23_20__7_4, alpha);
                ymm27_24__11_8 = Avx2.Or(ymm27_24__11_8, alpha);
                ymm31_28__15_12 = Avx2.Or(ymm31_28__15_12, alpha);

                //15_0位于ymm寄存器低128位 31_16位于ymm寄存器高128位
                Vector128<byte> xmm0, xmm1, xmm2, xmm3;
                xmm0 = ymm19_16__3_0.GetLower();
                xmm1 = ymm23_20__7_4.GetLower();
                xmm2 = ymm27_24__11_8.GetLower();
                xmm3 = ymm31_28__15_12.GetLower();

                Avx2.Store(destPtr + 00, xmm0);
                Avx2.Store(destPtr + 16, xmm1);
                Avx2.Store(destPtr + 32, xmm2);
                Avx2.Store(destPtr + 48, xmm3);

                xmm0 = ymm19_16__3_0.GetUpper();
                xmm1 = ymm23_20__7_4.GetUpper();
                xmm2 = ymm27_24__11_8.GetUpper();
                xmm3 = ymm31_28__15_12.GetUpper();

                Avx2.Store(destPtr + 64, xmm0);
                Avx2.Store(destPtr + 80, xmm1);
                Avx2.Store(destPtr + 96, xmm2);
                Avx2.Store(destPtr + 112, xmm3);

                srcPtr += 32;
                destPtr += 128;
                length -= 32;
            }

            while (length >= 16)
            {
                Vector128<byte> xmm15_0 = Avx2.LoadVector128(srcPtr);

                Vector128<byte> xmm3_0, xmm7_4, xmm11_8, xmm15_12;

                //vpshufb xmm, xmm, xmm
                xmm3_0 = Avx2.Shuffle(xmm15_0, selector3_0.GetLower());
                xmm7_4 = Avx2.Shuffle(xmm15_0, selector7_4.GetLower());
                xmm11_8 = Avx2.Shuffle(xmm15_0, selector11_8.GetLower());
                xmm15_12 = Avx2.Shuffle(xmm15_0, selector15_12.GetLower());

                //vpor xmm, xmm, xmm
                xmm3_0 = Avx2.Or(xmm3_0, alpha.GetLower());
                xmm7_4 = Avx2.Or(xmm7_4, alpha.GetLower());
                xmm11_8 = Avx2.Or(xmm11_8, alpha.GetLower());
                xmm15_12 = Avx2.Or(xmm15_12, alpha.GetLower());

                Avx2.Store(destPtr + 00, xmm3_0);
                Avx2.Store(destPtr + 16, xmm7_4);
                Avx2.Store(destPtr + 32, xmm11_8);
                Avx2.Store(destPtr + 48, xmm15_12);

                srcPtr += 16;
                destPtr += 64;
                length -= 16;
            }

            //标量
            while (length != 0)
            {
                PixelConverter.Gray8ToRGBA32Unsafe(destPtr, srcPtr);
                ++srcPtr;
                destPtr += 4;
                --length;
            }
        }
        /// <summary>
        /// Gray8转RGBA32
        /// <para>1Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void Gray8ToRGBA32Scalar(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length / 4, src.Length), length);

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();
            while (length != 0)
            {
                PixelConverter.Gray8ToRGBA32Unsafe(destPtr, srcPtr);
                ++srcPtr;
                destPtr += 4;
                --length;
            }
        }
        /// <summary>
        /// Gray8转RGBA32
        /// <para>1Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void Gray8ToRGBA32Unsafe(byte* dest, byte* src)
        {
            byte c = src[0];
            dest[0] = c;
            dest[1] = c;
            dest[2] = c;
            dest[3] = 0xFF;
        }
        /// <summary>
        /// BGR565转RGBA32
        /// <para>2Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void BGR565ToRGBA32Vector(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length / 4, src.Length / 2), length / 2) * 2;

            Vector256<uint> maskR, maskG, maskB, alpha;
            {
                maskR = Vector256.Create(0x0000F800u);
                maskG = Vector256.Create(0x000007E0u);
                maskB = Vector256.Create(0x0000001Fu);
                alpha = Vector256.Create(0xFF000000u);
            }

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();

            //1路ymm
            while (length >= 32)
            {
                Vector256<ushort> ymmColor16 = Avx2.LoadVector256(srcPtr).AsUInt16();

                Vector256<uint> ymmR, ymmG, ymmB;

                //vpmovzxwd ymm, xmm
                Vector256<uint> ymmColor7_0 = Avx2.ConvertToVector256Int32(ymmColor16.GetLower()).AsUInt32();
                Vector256<uint> ymmColor15_8 = Avx2.ConvertToVector256Int32(ymmColor16.GetUpper()).AsUInt32();

                //vpand ymm, ymm, ymm
                ymmR = Avx2.And(ymmColor7_0, maskR);
                ymmG = Avx2.And(ymmColor7_0, maskG);
                ymmB = Avx2.And(ymmColor7_0, maskB);

                //vpsrld ymm, ymm, imm8
                //vpslld ymm, ymm, imm8
                ymmR = Avx2.ShiftRightLogical(ymmR, 8);
                ymmG = Avx2.ShiftLeftLogical(ymmG, 5);
                ymmB = Avx2.ShiftLeftLogical(ymmB, 19);

                //vpor ymm, ymm, ymm
                ymmColor7_0 = Avx2.Or(Avx2.Or(Avx2.Or(ymmR, ymmG), ymmB), alpha);

                //vpand ymm, ymm, ymm
                ymmR = Avx2.And(ymmColor15_8, maskR);
                ymmG = Avx2.And(ymmColor15_8, maskG);
                ymmB = Avx2.And(ymmColor15_8, maskB);

                //vpsrld ymm, ymm, imm8
                //vpslld ymm, ymm, imm8
                ymmR = Avx2.ShiftRightLogical(ymmR, 8);
                ymmG = Avx2.ShiftLeftLogical(ymmG, 5);
                ymmB = Avx2.ShiftLeftLogical(ymmB, 19);

                //vpor ymm, ymm, ymm
                ymmColor15_8 = Avx2.Or(Avx2.Or(Avx2.Or(ymmR, ymmG), ymmB), alpha);

                Avx2.Store(destPtr + 00, ymmColor7_0.AsByte());
                Avx2.Store(destPtr + 32, ymmColor15_8.AsByte());

                srcPtr += 32;
                destPtr += 64;
                length -= 32;
            }

            //1路xmm
            while (length >= 16)
            {
                Vector128<ushort> xmmColor8 = Avx2.LoadVector128(srcPtr).AsUInt16();

                Vector256<uint> ymmR, ymmG, ymmB;

                //vpmovzxwd ymm, xmm
                Vector256<uint> ymmColor7_0 = Avx2.ConvertToVector256Int32(xmmColor8).AsUInt32();

                //vpand ymm, ymm, ymm
                ymmR = Avx2.And(ymmColor7_0, maskR);
                ymmG = Avx2.And(ymmColor7_0, maskG);
                ymmB = Avx2.And(ymmColor7_0, maskB);

                //vpsrld ymm, ymm, imm8
                //vpslld ymm, ymm, imm8
                ymmR = Avx2.ShiftRightLogical(ymmR, 8);
                ymmG = Avx2.ShiftLeftLogical(ymmG, 5);
                ymmB = Avx2.ShiftLeftLogical(ymmB, 19);

                //vpor ymm, ymm, ymm
                ymmColor7_0 = Avx2.Or(Avx2.Or(Avx2.Or(ymmR, ymmG), ymmB), alpha);

                Avx2.Store(destPtr, ymmColor7_0.AsByte());

                srcPtr += 16;
                destPtr += 32;
                length -= 16;
            }

            //标量
            while (length != 0)
            {
                PixelConverter.BGR565ToRGBA32Unsafe(destPtr, srcPtr);
                srcPtr += 2;
                destPtr += 4;
                length -= 2;
            }
        }
        /// <summary>
        /// BGR565转RGBA32
        /// <para>2Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void BGR565ToRGBA32Scalar(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length / 4, src.Length / 2), length / 2) * 2;

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();
            while (length != 0)
            {
                PixelConverter.BGR565ToRGBA32Unsafe(destPtr, srcPtr);
                srcPtr += 2;
                destPtr += 4;
                length -= 2;
            }
        }
        /// <summary>
        /// BGR565转RGBA32
        /// <para>2Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void BGR565ToRGBA32Unsafe(byte* dest, byte* src)
        {
            const uint MaskR = 0x0000F800u;
            const uint MaskG = 0x000007E0u;
            const uint MaskB = 0x0000001Fu;

            uint color = *(ushort*)src;

            dest[0] = (byte)(((color & MaskR) >> 11) << 3);
            dest[1] = (byte)(((color & MaskG) >> 5) << 2);
            dest[2] = (byte)((color & MaskB) << 3);
            dest[3] = 0xFF;
        }
        /// <summary>
        /// BGR24转RGBA32
        /// <para>3Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void BGR24ToRGBA32Vector(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length / 4, src.Length / 3), length / 3) * 3;

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();

            Vector128<byte> selector, alpha;
            {
                //交换ptr[0]与ptr[2]
                // 0F  0E  0D  0C      0B  0A  09      08  07  06      05  04  03      02  01  00   ---> 12字节
                //
                // [            ]  80  09  0A  0B  80  06  07  08  80  03  04  05  80  00  01  02   ---> 16字节

                ulong lo = 0x8003040580000102ul;
                ulong hi = 0x80090A0B80060708ul;

                selector = Vector128.Create(lo, hi).AsByte();
                alpha = Vector128.Create(0xFF000000u).AsByte();
            }

            //6路xmm
            while (length >= 96)
            {
                Vector128<byte> xmm15_0, xmm31_16, xmm47_32, xmm63_48, xmm79_64, xmm95_80;
                xmm15_0 = Avx2.LoadVector128(srcPtr + 00);
                xmm31_16 = Avx2.LoadVector128(srcPtr + 16);
                xmm47_32 = Avx2.LoadVector128(srcPtr + 32);

                xmm63_48 = Avx2.LoadVector128(srcPtr + 48);
                xmm79_64 = Avx2.LoadVector128(srcPtr + 64);
                xmm95_80 = Avx2.LoadVector128(srcPtr + 80);

                //vpalignr xmm, xmm, xmm
                //vpsrldq xmm, xmm, imm8
                Vector128<byte> xmm0, xmm1, xmm2, xmm3, xmm4, xmm5, xmm6, xmm7;
                xmm0 = xmm15_0;
                xmm1 = Avx2.AlignRight(xmm31_16, xmm15_0, 12);
                xmm2 = Avx2.AlignRight(xmm47_32, xmm31_16, 8);
                xmm3 = Avx2.ShiftRightLogical128BitLane(xmm47_32, 4);

                xmm4 = xmm63_48;
                xmm5 = Avx2.AlignRight(xmm79_64, xmm63_48, 12);
                xmm6 = Avx2.AlignRight(xmm95_80, xmm79_64, 8);
                xmm7 = Avx2.ShiftRightLogical128BitLane(xmm95_80, 4);

                //vpshufb xmm, xmm, xmm
                xmm0 = Avx2.Shuffle(xmm0, selector);
                xmm1 = Avx2.Shuffle(xmm1, selector);
                xmm2 = Avx2.Shuffle(xmm2, selector);
                xmm3 = Avx2.Shuffle(xmm3, selector);

                xmm4 = Avx2.Shuffle(xmm4, selector);
                xmm5 = Avx2.Shuffle(xmm5, selector);
                xmm6 = Avx2.Shuffle(xmm6, selector);
                xmm7 = Avx2.Shuffle(xmm7, selector);

                //vpor xmm, xmm, xmm
                xmm0 = Avx2.Or(xmm0, alpha);
                xmm1 = Avx2.Or(xmm1, alpha);
                xmm2 = Avx2.Or(xmm2, alpha);
                xmm3 = Avx2.Or(xmm3, alpha);

                xmm4 = Avx2.Or(xmm4, alpha);
                xmm5 = Avx2.Or(xmm5, alpha);
                xmm6 = Avx2.Or(xmm6, alpha);
                xmm7 = Avx2.Or(xmm7, alpha);

                Avx2.Store(destPtr + 00, xmm0);
                Avx2.Store(destPtr + 16, xmm1);
                Avx2.Store(destPtr + 32, xmm2);
                Avx2.Store(destPtr + 48, xmm3);

                Avx2.Store(destPtr + 64, xmm4);
                Avx2.Store(destPtr + 80, xmm5);
                Avx2.Store(destPtr + 96, xmm6);
                Avx2.Store(destPtr + 112, xmm7);

                srcPtr += 96;
                destPtr += 128;
                length -= 96;
            }

            //3路xmm
            while (length >= 48)
            {
                Vector128<byte> xmm15_0, xmm31_16, xmm47_32;
                xmm15_0 = Avx2.LoadVector128(srcPtr + 00);
                xmm31_16 = Avx2.LoadVector128(srcPtr + 16);
                xmm47_32 = Avx2.LoadVector128(srcPtr + 32);

                //vpalignr xmm, xmm, xmm
                //vpsrldq xmm, xmm, imm8
                Vector128<byte> xmm0, xmm1, xmm2, xmm3;
                xmm0 = xmm15_0;
                xmm1 = Avx2.AlignRight(xmm31_16, xmm15_0, 12);
                xmm2 = Avx2.AlignRight(xmm47_32, xmm31_16, 8);
                xmm3 = Avx2.ShiftRightLogical128BitLane(xmm47_32, 4);

                //vpshufb xmm, xmm, xmm
                xmm0 = Avx2.Shuffle(xmm0, selector);
                xmm1 = Avx2.Shuffle(xmm1, selector);
                xmm2 = Avx2.Shuffle(xmm2, selector);
                xmm3 = Avx2.Shuffle(xmm3, selector);

                //vpor xmm, xmm, xmm
                xmm0 = Avx2.Or(xmm0, alpha);
                xmm1 = Avx2.Or(xmm1, alpha);
                xmm2 = Avx2.Or(xmm2, alpha);
                xmm3 = Avx2.Or(xmm3, alpha);

                Avx2.Store(destPtr + 00, xmm0);
                Avx2.Store(destPtr + 16, xmm1);
                Avx2.Store(destPtr + 32, xmm2);
                Avx2.Store(destPtr + 48, xmm3);

                srcPtr += 48;
                destPtr += 64;
                length -= 48;
            }

            //标量
            while (length != 0)
            {
                PixelConverter.BGR24ToRGBA32Unsafe(destPtr, srcPtr);
                srcPtr += 3;
                destPtr += 4;
                length -= 3;
            }
        }
        /// <summary>
        /// BGR24转RGBA32
        /// <para>3Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void BGR24ToRGBA32Scalar(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length / 4, src.Length / 3), length / 3) * 3;

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();
            while(length != 0)
            {
                PixelConverter.BGR24ToRGBA32Unsafe(destPtr, srcPtr);
                srcPtr += 3;
                destPtr += 4;
                length -= 3;
            }
        }
        /// <summary>
        /// BGR24转RGBA32
        /// <para>3Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void BGR24ToRGBA32Unsafe(byte* dest, byte* src)
        {
            dest[0] = src[2];
            dest[1] = src[1];
            dest[2] = src[0];
            dest[3] = 0xFF;
        }
        /// <summary>
        /// BGRA32转RGBA32
        /// <para>4Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void BGRA32ToRGBA32Vector(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length, src.Length), length);
            //不满4字节不处理
            length &= ~3;

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();

            Vector256<byte> selector;
            {
                //交换ptr[0]与ptr[2]
                // 0F  0E  0D  0C  0B  0A  09  08  07  06  05  04  03  02  01  00
                //
                // 0F  0C  0D  0E  0B  08  09  0A  07  04  05  06  03  00  01  02

                ulong lo = 0x0704050603000102ul;
                ulong hi = 0x0F0C0D0E0B08090Aul;
                selector = Vector256.Create(lo, hi, lo, hi).AsByte();
            }

            //4路ymm
            while (length >= 128)
            {
                Vector256<byte> ymm0 = Avx2.LoadVector256(srcPtr + 00);
                Vector256<byte> ymm1 = Avx2.LoadVector256(srcPtr + 32);
                Vector256<byte> ymm2 = Avx2.LoadVector256(srcPtr + 64);
                Vector256<byte> ymm3 = Avx2.LoadVector256(srcPtr + 96);

                //vpshufb ymm, ymm, ymm
                ymm0 = Avx2.Shuffle(ymm0, selector);
                ymm1 = Avx2.Shuffle(ymm1, selector);
                ymm2 = Avx2.Shuffle(ymm2, selector);
                ymm3 = Avx2.Shuffle(ymm3, selector);

                Avx2.Store(destPtr + 00, ymm0);
                Avx2.Store(destPtr + 32, ymm1);
                Avx2.Store(destPtr + 64, ymm2);
                Avx2.Store(destPtr + 96, ymm3);

                srcPtr += 128;
                destPtr += 128;
                length -= 128;
            }

            //2路ymm
            while (length >= 64)
            {
                Vector256<byte> ymm0 = Avx2.LoadVector256(srcPtr + 00);
                Vector256<byte> ymm1 = Avx2.LoadVector256(srcPtr + 32);

                //vpshufb ymm, ymm, ymm
                ymm0 = Avx2.Shuffle(ymm0, selector);
                ymm1 = Avx2.Shuffle(ymm1, selector);

                Avx2.Store(destPtr + 00, ymm0);
                Avx2.Store(destPtr + 32, ymm1);

                srcPtr += 64;
                destPtr += 64;
                length -= 64;
            }

            //1路ymm
            while (length >= 32)
            {
                Vector256<byte> ymm0 = Avx2.LoadVector256(srcPtr + 00);

                //vpshufb ymm, ymm, ymm
                ymm0 = Avx2.Shuffle(ymm0, selector);

                Avx2.Store(destPtr + 00, ymm0);

                srcPtr += 32;
                destPtr += 32;
                length -= 32;
            }

            //标量
            while (length != 0)
            {
                PixelConverter.BGRA32ToRGBA32Unsafe(destPtr, srcPtr);
                srcPtr += 4;
                destPtr += 4;
                length -= 4;
            }
        }
        /// <summary>
        /// BGRA32转RGBA32
        /// <para>4Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void BGRA32ToRGBA32Scalar(in Span<byte> dest, in Span<byte> src, int length)
        {
            if (length < 0)
            {
                length = 0;
            }

            length = Math.Min(Math.Min(dest.Length, src.Length), length);
            //不满4字节不处理
            length &= ~3;

            byte* srcPtr = (byte*)src.AsPointer();
            byte* destPtr = (byte*)dest.AsPointer();
            while (length != 0)
            {
                PixelConverter.BGRA32ToRGBA32Unsafe(destPtr, srcPtr);
                srcPtr += 4;
                destPtr += 4;
                length -= 4;
            }
        }
        /// <summary>
        /// BGRA32转RGBA32
        /// <para>4Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void BGRA32ToRGBA32Unsafe(byte* dest, byte* src)
        {
            dest[0] = src[2];
            dest[1] = src[1];
            dest[2] = src[0];
            dest[3] = src[3];
        }
        /// <summary>
        /// RGBA32转BGRA32
        /// <para>4Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void RGBA32ToBGRA32Vector(in Span<byte> dest, in Span<byte> src, int length)
        {
            PixelConverter.BGRA32ToRGBA32Vector(dest, src, length);
        }
        /// <summary>
        /// RGBA32转BGRA32
        /// <para>4Byte --> 4Byte</para>
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="src">源</param>
        /// <param name="length">源字节长度</param>
        public static unsafe void RGBA32ToBGRA32Scalar(in Span<byte> dest, in Span<byte> src, int length)
        {
            PixelConverter.BGRA32ToRGBA32Scalar(dest, src, length);
        }
    }
}
