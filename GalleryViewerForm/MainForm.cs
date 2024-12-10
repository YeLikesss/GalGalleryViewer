using System;
using System.Windows.Forms;
using GalleryFormat.Base;
using GalleryFormat.GL;
using GalleryViewerForm.Base;
using GalleryViewerForm.Musica;

namespace GalleryViewerForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.mProgress = new(this.Progress_OnChanged);

            this.RenderMode_OnChanged();
            this.EncodeMode_OnChanged();
        }

        private IOperator? mCurrentTarget = null;           //当前操作对象

        private int mProgressCurrent;
        private int mProgressCount;
        private readonly Progress<ProgressInfo> mProgress;

        //打开文件夹
        private void File_OpenDirectory(object sender, EventArgs args)
        {
            this.ClearProgressStatus();
            if (this.mCurrentTarget is IOperator target)
            {
                using FolderBrowserDialog fbd = new()
                {
                    Description = "请选择文件夹",
                    ShowNewFolderButton = false,
                    AutoUpgradeEnabled = true,
                    UseDescriptionForTitle = true,
                };
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string dir = fbd.SelectedPath;

                    target.SetSourceDirectory(dir);
                    this.tssLabalSourceDirectory.Text = dir;
                }
            }
        }

        //导出立绘(单个)
        private async void File_Export(object sender, EventArgs args)
        {
            this.ClearProgressStatus();
            if (this.mCurrentTarget is IOperator target)
            {
                this.ExtractStatus_OnChanged(true);

                ResultState<string> result = await target.ExtractAsync(this.mProgress);
                if (!result.Successed)
                {
                    this.StatusMessage_OnChanged(result.Content!);
                }

                this.ExtractStatus_OnChanged(false);
            }
        }

        //导出所有立绘
        private async void File_ExportAll(object sender, EventArgs e)
        {
            this.ClearProgressStatus();
            if (this.mCurrentTarget is IOperator target)
            {
                this.ExtractStatus_OnChanged(true);

                ResultState<string> result = await target.ExtractAllAsync(this.mProgress);
                if (!result.Successed)
                {
                    this.StatusMessage_OnChanged(result.Content!);
                }

                this.ExtractStatus_OnChanged(false);
            }
        }

        //选择视图
        private void SelectView_OnClick(object sender, EventArgs args)
        {
            this.ClearProgressStatus();

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            //创建窗口
            Form? newForm = CreateSubFormInstance((string)item.Tag);
            if (newForm is null)
            {
                return;
            }
            newForm.TopLevel = false;
            newForm.FormBorderStyle = FormBorderStyle.None;
            newForm.Dock = DockStyle.Fill;

            //选中当前模式
            MainForm.MenuItemCheckedOnly(item);

            //功能窗体加载到布局面板
            Panel panel = this.panelView;
            {
                if (this.mCurrentTarget is IOperator oldOp)
                {
                    using Form? oldForm = oldOp as Form;
                    oldForm?.Close();
                }
            }
            panel.Controls.Clear();
            panel.Controls.Add(newForm);
            newForm.Show();

            IOperator? newOp = newForm as IOperator;

            this.mCurrentTarget = newOp;
            this.tssLabelMode.Text = item.Text;
        }

        //渲染模式选择
        private void BtnRenderMode_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Render.AVX2Enabled = MainForm.GetRenderIsEnableSIMD((string)item.Tag);
            this.RenderMode_OnChanged();
        }

        //渲染模式修改时
        private void RenderMode_OnChanged()
        {
            this.tssLabelRender.Text = $"CPU渲染器: {Environment.ProcessorCount}线程[{(Render.AVX2Enabled ? "Vector AVX2" : "Scalar IA32-AMD64")}]";
        }

        //编码模式选择
        private void BtnEncodeFormat_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (MainForm.GetEncoderFormat((string)item.Tag) is FormatType type)
            {
                Format.Type = type;
                this.EncodeMode_OnChanged();
            }
        }

        //编码状态切换
        private void EncodeMode_OnChanged()
        {
            this.tssLabelEncodeMode.Text = $"编码: {Format.Type.ToString().ToUpper()}";
        }

        //子窗口状态修改时
        private void StatusMessage_OnChanged(string msg)
        {
            this.tssLabalStatusMsg.Text = msg;
        }

        //进度状态更改时
        private void Progress_OnChanged(ProgressInfo info)
        {
            info.GetValue(ref this.mProgressCurrent, ref this.mProgressCount);
            this.tssLabelProgress.Text = $"进度: {this.mProgressCurrent} / {this.mProgressCount}";
        }

        //提取状态修改时
        private void ExtractStatus_OnChanged(bool extracting)
        {
            this.menuStrip.Enabled = !extracting;
        }

        /// <summary>
        /// 清空处理状态
        /// </summary>
        private void ClearProgressStatus()
        {
            this.tssLabelProgress.Text = string.Empty;
            this.tssLabalStatusMsg.Text = string.Empty;
        }

        /// <summary>
        /// 仅选中当前菜单按钮
        /// </summary>
        /// <param name="item">菜单按钮</param>
        private static void MenuItemCheckedOnly(ToolStripMenuItem item)
        {
            if (item.OwnerItem is ToolStripMenuItem topItem)
            {
                foreach (var subItem in topItem.DropDownItems)
                {
                    if (subItem is ToolStripMenuItem tsmi)
                    {
                        tsmi.Checked = false;
                    }
                }
            }
            item.Checked = true;
        }

        /// <summary>
        /// 创建子窗体实例
        /// </summary>
        private static Form? CreateSubFormInstance(string type)
        {
            return type switch
            {
                "Musica" => new MusicaForm(),
                _ => null,
            };
        }

        /// <summary>
        /// 获取编码类型
        /// </summary>
        private static FormatType? GetEncoderFormat(string s)
        {
            return s switch
            {
                "webp" => FormatType.Webp,
                "png" => FormatType.Png,
                "jpeg" => FormatType.Jpeg,
                "tga" => FormatType.Tga,
                "bmp" => FormatType.Bmp,
                _ => null,
            };
        }

        /// <summary>
        /// 获取是否启用SIMD渲染
        /// </summary>
        private static bool GetRenderIsEnableSIMD(string s)
        {
            return s switch
            {
                "vector" => true,
                "scalar" => false,
                _ => false,
            };
        }
    }
}