using System;
using System.Threading.Tasks;
using GalleryFormat.Base;

namespace GalleryViewerForm.Base
{
    /// <summary>
    /// 操作接口
    /// </summary>
    internal interface IOperator
    {
        /// <summary>
        /// 设置资源文件夹路径
        /// </summary>
        /// <param name="directory">文件夹全路径</param>
        public void SetSourceDirectory(string directory);

        /// <summary>
        /// 提取当前图层图片
        /// </summary>
        /// <param name="progress">进度回调</param>
        public Task<ResultState<string>> ExtractAsync(IProgress<ProgressInfo>? progress);

        /// <summary>
        /// 提取所有图片
        /// <para>按引擎规则</para>
        /// </summary>
        /// <param name="progress">进度回调</param>
        public Task<ResultState<string>> ExtractAllAsync(IProgress<ProgressInfo>? progress);
    }
}
