using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalleryViewerForm.Base
{
    /// <summary>
    /// 返回值状态
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class ResultState<T>
    {
        public T? Content { get; }
        public bool Successed { get; }

        private ResultState(bool successed, T? content)
        {
            this.Successed = successed;
            this.Content = content;
        }

        /// <summary>
        /// 成功操作
        /// </summary>
        public static ResultState<T> Success()
        {
            return new ResultState<T>(true, default);
        }

        /// <summary>
        /// 成功操作
        /// </summary>
        public static ResultState<T> Success(T content)
        {
            return new ResultState<T>(true, content);
        }

        /// <summary>
        /// 失败操作
        /// </summary>
        public static ResultState<T> Fail()
        {
            return new ResultState<T>(false, default);
        }

        /// <summary>
        /// 失败操作
        /// </summary>
        public static ResultState<T> Fail(T content)
        {
            return new ResultState<T>(false, content);
        }
    }
}
