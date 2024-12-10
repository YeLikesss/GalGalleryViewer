using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using GalleryFormat.Base;

namespace GalleryFormat.GL
{
    /// <summary>
    /// 渲染器
    /// </summary>
    public class Render
    {
        /// <summary>
        /// Avx2启用标志
        /// </summary>
        public static bool AVX2Enabled { get; set; } = false;

        /// <summary>
        /// Alpha混合
        /// <para>覆盖到背景</para>
        /// </summary>
        /// <param name="backGround">背景图</param>
        /// <param name="foreGround">前景图</param>
        /// <param name="bgRect">背景区块</param>
        /// <param name="fgRect">前景区块</param>
        /// <param name="opacity">不透明度</param>
        public static unsafe void BlendAlpha(Image<Rgba32> backGround, Image<Rgba32> foreGround, in Rectangle bgRect, in Rectangle fgRect, byte opacity)
        {
            Render.BlendAlpha(backGround, foreGround, bgRect, fgRect, opacity, Render.AVX2Enabled);
        }

        /// <summary>
        /// Alpha混合
        /// <para>覆盖到背景</para>
        /// </summary>
        /// <param name="backGround">背景图</param>
        /// <param name="foreGround">前景图</param>
        /// <param name="bgRect">背景区块</param>
        /// <param name="fgRect">前景区块</param>
        /// <param name="opacity">不透明度</param>
        /// <param name="enableSimd">启用SIMD</param>
        internal static unsafe void BlendAlpha(Image<Rgba32> backGround, Image<Rgba32> foreGround, in Rectangle bgRect, in Rectangle fgRect, byte opacity, bool enableSimd)
        {
            if (Unsafe.SizeOf<Rgba32>() != Unsafe.SizeOf<uint>())
            {
                throw new NotSupportedException("Rgba32结构大小不为4");
            }

            if (bgRect.Width < 0 || bgRect.Height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bgRect), "宽度和高度不可小于0");
            }

            if (fgRect.Width < 0 || fgRect.Height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fgRect), "宽度和高度不可小于0");
            }

            if (opacity == 0u)
            {
                return;
            }

            Rectangle bgBlendRect, fgBlendRect;
            int columns, rows;

            //选择区块
            bgBlendRect = Rectangle.Intersect(backGround.Bounds, bgRect);
            fgBlendRect = Rectangle.Intersect(foreGround.Bounds, fgRect);

            //渲染行列数
            columns = Math.Min(bgBlendRect.Width, fgBlendRect.Width);
            rows = Math.Min(bgBlendRect.Height, fgBlendRect.Height);

            bgBlendRect.Width = columns;
            bgBlendRect.Height = rows;

            fgBlendRect.Width = columns;
            fgBlendRect.Height = rows;

            if (rows <= 0 || columns <= 0)
            {
                return;
            }

            Buffer2D<Rgba32> bgPixelBuf = backGround.Frames.RootFrame.PixelBuffer;
            Buffer2D<Rgba32> fgPixelBuf = foreGround.Frames.RootFrame.PixelBuffer;

            if (enableSimd)
            {
                for (int y = 0; y < rows; ++y)
                {
                    Span<Rgba32> bgLine = bgPixelBuf.DangerousGetRowSpan(bgBlendRect.Y + y).Slice(bgBlendRect.X, columns);
                    Span<Rgba32> fgLine = fgPixelBuf.DangerousGetRowSpan(fgBlendRect.Y + y).Slice(fgBlendRect.X, columns);

                    uint* bgPtr = (uint*)bgLine.AsPointer();
                    uint* fgPtr = (uint*)fgLine.AsPointer();

                    Render.BlendAlphaVectorUnsafe(bgPtr, fgPtr, opacity, columns);
                }
            }
            else
            {
                for (int y = 0; y < rows; ++y)
                {
                    Span<Rgba32> bgLine = bgPixelBuf.DangerousGetRowSpan(bgBlendRect.Y + y).Slice(bgBlendRect.X, columns);
                    Span<Rgba32> fgLine = fgPixelBuf.DangerousGetRowSpan(fgBlendRect.Y + y).Slice(fgBlendRect.X, columns);

                    uint* bgPtr = (uint*)bgLine.AsPointer();
                    uint* fgPtr = (uint*)fgLine.AsPointer();

                    Render.BlendAlphaScalarUnsafe(bgPtr, fgPtr, opacity, columns);
                }
            }
        }
        /// <summary>
        /// Alpha混合 (Vector)
        /// <para>背景像素与前景像素数组长度必须一致, length参数必须小于等于数组长度</para>
        /// <para>该函数不会做安全检查</para>
        /// </summary>
        /// <param name="bgPtr">背景像素</param>
        /// <param name="fgPtr">前景像素</param>
        /// <param name="opacity">不透明度</param>
        /// <param name="length">像素长度</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void BlendAlphaVectorUnsafe(uint* bgPtr, uint* fgPtr, byte opacity, int length)
        {
            if (length > 0)
            {
                //打包8像素部分
                int vectorLoopCount = length / 8;
                if (vectorLoopCount != 0)
                {
                    //一组处理8像素[7..0]
                    do
                    {
                        Vector256<uint> bgPack8 = Avx2.LoadVector256(bgPtr);
                        Vector256<uint> fgPack8 = Avx2.LoadVector256(fgPtr);
                        Vector256<uint> destPack8;
                        {
                            Vector128<uint> destTemp, bgTemp, fgTemp;

                            //处理[3..0]像素
                            {
                                bgTemp = bgPack8.GetLower();
                                fgTemp = fgPack8.GetLower();
                                destTemp = AlphaBlendMath.Pack4x32bpp(bgTemp, fgTemp, opacity);
                            }

                            //[0, 0, 0, 0, d3, d2, d1, d0]
                            //操作xmm ymm高128位清零
                            destPack8 = destTemp.ToVector256Unsafe();

                            //处理[7..4]像素
                            {
                                bgTemp = bgPack8.GetUpper();
                                fgTemp = fgPack8.GetUpper();
                                destTemp = AlphaBlendMath.Pack4x32bpp(bgTemp, fgTemp, opacity);
                            }
 
                            //[d7, d6, d5, d4, d3, d2, d1, d0]
                            //vinserti128 ymm, ymm, xmm, imm8
                            destPack8 = Avx2.InsertVector128(destPack8, destTemp, 1);
                        }
                        Avx2.Store(bgPtr, destPack8);
                        bgPtr += 8;
                        fgPtr += 8;
                        length -= 8;
                    }
                    while (--vectorLoopCount != 0);
                }

                //剩余部分
                while(length != 0)
                {
                    *bgPtr = AlphaBlendMath.Scalar32bpp(*bgPtr, *fgPtr, opacity);
                    ++bgPtr;
                    ++fgPtr;
                    --length;
                }
            }
        }
        /// <summary>
        /// Alpha混合 (Scalar)
        /// <para>使用条件:</para>
        /// <para>背景像素与前景像素数组长度必须一致, length参数必须小于等于数组长度</para>
        /// <para>该函数不会做安全检查</para>
        /// </summary>
        /// <param name="bgPtr">背景像素</param>
        /// <param name="fgPtr">前景像素</param>
        /// <param name="opacity">不透明度</param>
        /// <param name="length">像素长度</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void BlendAlphaScalarUnsafe(uint* bgPtr, uint* fgPtr, byte opacity, int length)
        {
            if (length > 0)
            {
                do
                {
                    *bgPtr = AlphaBlendMath.Scalar32bpp(*bgPtr, *fgPtr, opacity);
                    ++bgPtr;
                    ++fgPtr;
                }
                while (--length != 0);
            }
        }
    }

    /// <summary>
    /// Alpha混合数学库
    /// </summary>
    internal class AlphaBlendMath
    {
        /// <summary>
        /// Alpha混合
        /// <para>打包处理2个32bpp像素</para>
        /// </summary>
        /// <param name="pack2_Bg">打包2个背景像素(计算低64位 忽略高64位)</param>
        /// <param name="pack2_Fg">打包2个前景像素(计算低64位 忽略高64位)</param>
        /// <param name="opacity">不透明度</param>
        /// <returns>打包2个混合后像素(低64位) 高64位为无效数据</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<uint> Pack2x32bppUnsafe(Vector128<uint> pack2_Bg, Vector128<uint> pack2_Fg, byte opacity)
        {
            Vector256<uint> cPack8_Opacity;            //打包8个透明度
            Vector256<uint> cPack8_000000FF;           //打包8个255
            Vector256<uint> cPack8_0000FE01;           //打包8个65025
            Vector256<uint> cPack8_81018203;           //打包8个0x81018203
            Vector256<uint> cPack8_80000000;           //打包8个0x80000000
            {
                //movd xmm, reg
                //vpbroadcastd ymm, xmm
                Vector128<uint> xmm0 = Avx2.ConvertScalarToVector128UInt32(opacity);
                cPack8_Opacity = Avx2.BroadcastScalarToVector256(xmm0);

                cPack8_000000FF = Vector256.Create(0x000000FFu);
                cPack8_0000FE01 = Vector256.Create(0x0000FE01u);
                cPack8_81018203 = Vector256.Create(0x81018203u);
                cPack8_80000000 = Vector256.Create(0x80000000u);
            }

            //vpmovzxbd ymm, xmm
            Vector256<uint> bgC = Avx2.ConvertToVector256Int32(pack2_Bg.AsByte()).AsUInt32();
            Vector256<uint> fgC = Avx2.ConvertToVector256Int32(pack2_Fg.AsByte()).AsUInt32();

            //vpshufd ymm, ymm, imm8
            Vector256<uint> bgA = Avx2.Shuffle(bgC, 0xFF);
            Vector256<uint> fgA = Avx2.Shuffle(fgC, 0xFF);

            Vector256<uint> v1, v2, v3, v4, v5, v6;

            //vpmulld ymm, ymm, ymm
            //vpsubd ymm, ymm, ymm
            //vpaddd ymm, ymm, ymm
            v1 = Avx2.MultiplyLow(fgA, cPack8_Opacity);                 //fgA * opa
            v2 = Avx2.MultiplyLow(bgA, cPack8_0000FE01);                //255^2 * bgA
            v3 = Avx2.MultiplyLow(bgA, v1);                             //bgA * fgA * opa
            v4 = Avx2.MultiplyLow(cPack8_000000FF, v1);                 //255 * fgA * opa
            v5 = Avx2.Subtract(v2, v3);                                 //255^2 * bgA - bgA * fgA * opa
            v6 = Avx2.Add(v4, v5);                                      //255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa

            //判断alpha通道是否为零
            Vector256<uint> invalid;
            {
                //vpcmpeqd ymm, ymm, ymm
                invalid = Avx2.CompareEqual(v6, Vector256<uint>.Zero);
            }

            //计算alpha通道
            Vector256<uint> alpha;
            {
                //alpha = (255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa) / 255^2
                //高128和低128内4个UInt32(Alpha)值相同   只计算一组
                //vpmuludq ymm, ymm, ymm
                //vpsrld ymm, ymm, imm8
                Vector256<uint> ui1, ui2;
                ui1 = Avx2.Multiply(v6, cPack8_81018203).AsUInt32();
                ui2 = Avx2.ShiftRightLogical(ui1, 15);

                //vpshufd ymm, ymm, imm8
                alpha = Avx2.Shuffle(ui2, 0xFF);
            }

            //计算颜色通道
            Vector256<uint> color;
            {
                //((255^2 * bgA - bgA * fgA * opa) * bgC + (255 * fgA * opa) * fgC) / (255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa)
                //(bgC * v5 + fgC * v4) / v6
                //除法没有整数版本  转为fp32运算

                Vector256<uint> u32Dividend;
                {
                    //vpmulld ymm, ymm, ymm
                    //vpaddd ymm, ymm, ymm
                    Vector256<uint> ui1, ui2;
                    ui1 = Avx2.MultiplyLow(bgC, v5);       //bgC * v5
                    ui2 = Avx2.MultiplyLow(fgC, v4);       //fgC * v4
                    u32Dividend = Avx2.Add(ui1, ui2);      //bgC * v5 + fgC * v4
                }

                Vector256<float> fpDividend;
                {
                    //最大255^4 最高位可能为1 可能溢出

                    Vector256<uint> ui1, ui2, ui3, ui4, ui5, u32Value;
                    Vector256<float> fpValue, fpOffset;

                    //0x00000000 --> 0x00000000
                    //0x80000000 --> 0x4F000000
                    //vpand ymm, ymm, ymm
                    //vpsrad ymm, ymm, imm8
                    //vpsrld ymm, ymm, imm8
                    //vpor ymm, ymm, ymm
                    ui1 = Avx2.And(u32Dividend, cPack8_80000000);
                    ui2 = Avx2.ShiftRightArithmetic(ui1.AsInt32(), 3).AsUInt32();
                    ui3 = Avx2.ShiftRightLogical(ui1, 1);
                    ui4 = Avx2.ShiftRightLogical(ui2, 4);
                    ui5 = Avx2.Or(ui3, ui4);
                    fpOffset = ui5.AsSingle();

                    //u32Value = (~0x80000000) & u32Dividend = 0x7FFFFFFF & u32Dividend
                    //fpValue = (float)u32Value
                    //vpandn ymm, ymm, ymm
                    //vcvtdq2ps ymm, ymm
                    u32Value = Avx2.AndNot(cPack8_80000000, u32Dividend);
                    fpValue = Avx2.ConvertToVector256Single(u32Value.AsInt32());

                    //vaddps ymm, ymm, ymm
                    fpDividend = Avx2.Add(fpValue, fpOffset);
                }

                Vector256<float> fpDivisor;
                {
                    //此值最大255^3 最高位必为0 不会溢出
                    //vcvtdp2ps ymm, ymm
                    fpDivisor = Avx2.ConvertToVector256Single(v6.AsInt32());
                }

                Vector256<float> fpcolor;
                {
                    //vdivps ymm, ymm, ymm
                    fpcolor = Avx2.Divide(fpDividend, fpDivisor);
                }

                //vcvttps2pd ymm, ymm
                color = Avx2.ConvertToVector256Int32WithTruncation(fpcolor).AsUInt32();
            }

            //写入新alpha值  非法值置0
            //vpblendd ymm, ymm, ymm, imm8
            //vpandn ymm, ymm, ymm
            color = Avx2.Blend(color, alpha, 0x88);
            color = Avx2.AndNot(invalid, color);

            Vector128<uint> dest;
            {
                Vector128<uint> destDirty;
                Vector256<ushort> destU16;
                Vector256<byte> destU8;
                Vector128<byte> destU8Low, destU8High;

                //vpackusdw ymm, ymm, ymm
                //vpackuswb ymm, ymm, ymm
                destU16 = Avx2.PackUnsignedSaturate(color.AsInt32(), color.AsInt32());
                destU8 = Avx2.PackUnsignedSaturate(destU16.AsInt16(), destU16.AsInt16());

                destU8High = destU8.GetUpper();
                destU8Low = destU8.GetLower();

                //vpunpkldq xmm, xmm, xmm
                destDirty = Avx2.UnpackLow(destU8Low.AsUInt32(), destU8High.AsUInt32());

                dest = destDirty;
            }
            return dest;
        }
        /// <summary>
        /// Alpha混合
        /// <para>打包处理2个32bpp像素</para>
        /// </summary>
        /// <param name="pack2_Bg">打包2个背景像素(计算低64位 忽略高64位)</param>
        /// <param name="pack2_Fg">打包2个前景像素(计算低64位 忽略高64位)</param>
        /// <param name="opacity">不透明度</param>
        /// <returns>打包2个混合后像素(低64位) 高64位清零</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<uint> Pack2x32bpp(Vector128<uint> pack2_Bg, Vector128<uint> pack2_Fg, byte opacity)
        {
            Vector128<uint> dest = AlphaBlendMath.Pack2x32bppUnsafe(pack2_Bg, pack2_Fg, opacity);

            //高64位清零
            //movq xmm, xmm
            dest = Avx2.MoveScalar(dest.AsUInt64()).AsUInt32();

            return dest;
        }
        /// <summary>
        /// Alpha混合
        /// <para>打包处理4个32bpp像素</para>
        /// </summary>
        /// <param name="pack4_Bg">打包4个背景像素</param>
        /// <param name="pack4_Fg">打包4个前景像素</param>
        /// <param name="opacity">不透明度</param>
        /// <returns>打包4个混合后像素</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<uint> Pack4x32bpp(Vector128<uint> pack4_Bg, Vector128<uint> pack4_Fg, byte opacity)
        {
            Vector128<uint> dest;
            {
                Vector128<uint> destLo, destHi, bgTemp, fgTemp;

                bgTemp = pack4_Bg;
                fgTemp = pack4_Fg;
                destLo = AlphaBlendMath.Pack2x32bppUnsafe(bgTemp, fgTemp, opacity);

                //vpsrldq xmm, xmm, imm8
                bgTemp = Avx2.ShiftRightLogical128BitLane(pack4_Bg, 8);
                fgTemp = Avx2.ShiftRightLogical128BitLane(pack4_Fg, 8);
                destHi = AlphaBlendMath.Pack2x32bppUnsafe(bgTemp, fgTemp, opacity);

                //vpslldq xmm, xmm, imm8
                destHi = Avx2.ShiftLeftLogical128BitLane(destHi, 8);

                //vpblendd xmm, xmm, imm8
                dest = Avx2.Blend(destLo, destHi, 0x0C);
            }
            return dest;
        }
        /// <summary>
        /// Alpha混合
        /// </summary>
        /// <param name="bg">背景像素</param>
        /// <param name="fg">前景像素</param>
        /// <param name="opacity">不透明度</param>
        /// <returns>混合后像素</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe static uint Scalar32bpp(uint bg, uint fg, byte opacity)
        {
            /*
             * 透明度公式:
             * fgAlpha = fgAlpha * opacity / 255
             * 
             * Alpha混合公式:
             * alpha = bgAlpha + fgAlpha - (bgAlpha * fgAlpha / 255)
             * 
             * 颜色混合公式:
             * color = (bgColor * bgAlpha + fgColor * fgAlpha - bgColor * bgAlpha * fgAlpha / 255) / blendAlpha
             * 
             * 合并公式:
             * alpha = (255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa) / 255^2
             *
             * color = (255^2 * bgC * bgA + 255 * fgC * fgA * opa - bgC * bgA * fgA * opa) / (255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa)
             *
             */

            uint dest = 0u;

            uint bgA = bg >> 24;
            uint fgA = fg >> 24;
            uint opa = opacity;

            //两个Alpha通道均为零无效
            if(!(bgA == 0u && (fgA * opa) == 0u))
            {
                uint bgC, fgC, c;

                uint v1 = fgA * opa;        //fgA * opa
                uint v2 = bgA * 65025u;     //255^2 * bgA
                uint v3 = bgA * v1;         //bgA * fgA * opa
                uint v4 = 255u * v1;        //255 * fgA * opa
                uint v5 = v2 - v3;          //255^2 * bgA - bgA * fgA * opa
                uint v6 = v4 + v5;          //255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa

                //Alpha通道
                c = v6 / 65025u;

                dest |= c;
                dest <<= 8;

                //((255^2 * bgA - bgA * fgA * opa) * bgC + (255 * fgA * opa) * fgC) / (255^2 * bgA + 255 * fgA * opa - bgA * fgA * opa)

                bgC = (bg >> 16) & 0xFFu;
                fgC = (fg >> 16) & 0xFFu;
                c = (bgC * v5 + fgC * v4) / v6;

                dest |= c;
                dest <<= 8;

                bgC = (bg >> 08) & 0xFFu;
                fgC = (fg >> 08) & 0xFFu;
                c = (bgC * v5 + fgC * v4) / v6;

                dest |= c;
                dest <<= 8;

                bgC = (bg >> 00) & 0xFFu;
                fgC = (fg >> 00) & 0xFFu;
                c = (bgC * v5 + fgC * v4) / v6;

                dest |= c;
            }
            return dest;
        }
    }
}
