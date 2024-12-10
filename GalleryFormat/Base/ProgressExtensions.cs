using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryFormat.Base
{
    /// <summary>
    /// 进度信息值类型
    /// </summary>
    public enum ProgressValueType
    {
        /// <summary>
        /// 绝对值
        /// </summary>
        Absolute,
        /// <summary>
        /// 相对值
        /// </summary>
        Relative,
        /// <summary>
        /// 清空值
        /// </summary>
        Reset,
        /// <summary>
        /// 设置总数
        /// </summary>
        AbsoluteCount,
        /// <summary>
        /// 设置总数同时清空进度
        /// </summary>
        AbsoluteCountAndReset,
    }
    /// <summary>
    /// 进度信息
    /// </summary>
    public class ProgressInfo
    {
        private readonly ProgressValueType mType;
        private readonly int mValue;

        /// <summary>
        /// 类型
        /// </summary>
        public ProgressValueType Type => this.mType;
        /// <summary>
        /// 值
        /// </summary>
        public int Value => this.mValue;

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="count">进度总数</param>
        public void GetValue(ref int current, ref int count)
        {
            switch (this.mType)
            {
                case ProgressValueType.Absolute:
                    current = this.mValue;
                    break;
                case ProgressValueType.Relative:
                    current += this.mValue;
                    break;
                case ProgressValueType.Reset:
                    current = 0;
                    break;
                case ProgressValueType.AbsoluteCount:
                    count = this.mValue;
                    break;
                case ProgressValueType.AbsoluteCountAndReset:
                    current = 0;
                    count = this.mValue;
                    break;
            }
        }

        /// <summary>
        /// 进度信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public ProgressInfo(ProgressValueType type, int value = 0)
        {
            this.mType = type;
            this.mValue = value;
        }
    }
}
