using System;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using GalleryFormat.Format.Musica;
using GalleryViewerForm.Base;
using System.Linq;
using System.Reflection;

namespace GalleryViewerForm.Musica
{
    public partial class SqzForm : Form
    {
        public SqzForm()
        {
            InitializeComponent();

            ListBox lb = this.lbFrames;
            lb.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(lb, true);
        }

        /// <summary>
        /// Sqz图像
        /// </summary>
        public SqzImage? ImageFrames { get; set; }

        /// <summary>
        /// 清除内容显示
        /// </summary>
        private void Clear()
        {
            this.tssLabelName.Text = string.Empty;
            this.tssLabelFrameSelected.Text = string.Empty;
            this.tssLabelResolution.Text = string.Empty;
            this.lbFrames.Items.Clear();

            using System.Drawing.Image org = this.picViewBox.Image;
            this.picViewBox.Image = null;
        }

        //窗体加载
        private void SqzForm_OnLoad(object sender, EventArgs e)
        {
            this.Clear();
            if (this.ImageFrames is SqzImage sqz)
            {
                this.tssLabelName.Text = sqz.Name;
                this.tssLabelResolution.Text = $"{sqz.Width} x {sqz.Height}";

                //添加帧序号
                ListBox lb = this.lbFrames;
                lb.BeginUpdate();
                lb.Items.AddRange(Enumerable.Range(0, sqz.Frames.Count).ToList().ConvertAll(i => $"{i:D4}").ToArray());
                lb.EndUpdate();

                lb.SelectedIndex = sqz.SelectedIndex;
            }
        }

        //列表选项修改
        private void LbFrames_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ListBox lb && this.ImageFrames is SqzImage sqz)
            {
                sqz.SelectedIndex = lb.SelectedIndex;

                using Image<Rgba32> img = sqz.GetImage();
                using System.Drawing.Image org = this.picViewBox.Image;
                this.picViewBox.Image = img.ToBitmap();

                if (sqz.SelectedIndex == -1)
                {
                    this.tssLabelFrameSelected.Text = "主视图";
                }
                else
                {
                    this.tssLabelFrameSelected.Text = lb.SelectedItem.ToString();
                }
            }
        }

        //当前帧表情点击
        private void TssLabelFrameSelected_OnClick(object sender, EventArgs e)
        {
            this.lbFrames.ClearSelected();
        }
    }
}
