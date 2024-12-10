using System;
using System.Reflection;
using System.Windows.Forms;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using GalleryViewerForm.Base;
using GalleryFormat.Format.Musica;

namespace GalleryViewerForm.Musica
{
    public partial class AniForm : Form
    {
        public AniForm()
        {
            InitializeComponent();

            ListView lv = this.lvFrames;
            lv.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(lv, true);

            ColumnHeader ch0 = new() { Name = "index", Text = string.Empty, Width = 30 };
            ColumnHeader ch1 = new() { Name = "name", Text = "名字", Width = 120, TextAlign = HorizontalAlignment.Center };
            ColumnHeader ch2 = new() { Name = "width", Text = "宽度", Width = 80, TextAlign = HorizontalAlignment.Center };
            ColumnHeader ch3 = new() { Name = "height", Text = "高度", Width = 80, TextAlign = HorizontalAlignment.Center };
            ColumnHeader ch4 = new() { Name = "offsetX", Text = "X偏移", Width = 80, TextAlign = HorizontalAlignment.Center };
            ColumnHeader ch5 = new() { Name = "offsetY", Text = "Y偏移", Width = 80, TextAlign = HorizontalAlignment.Center };

            lv.Columns.Add(ch0);
            lv.Columns.Add(ch1);
            lv.Columns.Add(ch2);
            lv.Columns.Add(ch3);
            lv.Columns.Add(ch4);
            lv.Columns.Add(ch5);
        }

        /// <summary>
        /// Ani图像
        /// </summary>
        public AniImage? ImageFrames { get; set; }

        /// <summary>
        /// 刷新图像
        /// </summary>
        private void RefreshImage()
        {
            if (this.ImageFrames is AniImage ani)
            {
                ListView lv = this.lvFrames;
                for (int i = 0; i < lv.Items.Count; ++i)
                {
                    ani.SelectedIndices[i] = lv.Items[i].Checked;
                }

                using Image<Rgba32> img = ani.GetImage();
                using System.Drawing.Image org = this.picViewBox.Image;
                this.picViewBox.Image = img.ToBitmap();
            }
        }

        /// <summary>
        /// 清除内容显示
        /// </summary>
        private void Clear()
        {
            this.tssLabelName.Text = string.Empty;
            this.tssLabelResolution.Text = string.Empty;
            this.lvFrames.Items.Clear();

            using System.Drawing.Image org = this.picViewBox.Image;
            this.picViewBox.Image = null;
        }

        //窗体加载
        private void AniForm_OnLoad(object sender, EventArgs e)
        {
            this.Clear();
            if (this.ImageFrames is AniImage ani)
            {
                this.tssLabelName.Text = ani.Name;
                this.tssLabelResolution.Text = $"{ani.Width} x {ani.Height}";

                ListView lv = this.lvFrames;
                ListView.ColumnHeaderCollection cols = lv.Columns;

                lv.BeginUpdate();
                for (int i = 0; i < ani.Frames.Count; ++i)
                {
                    AniImageInfo info = ani.Frames[i];

                    ListViewItem.ListViewSubItem[] items = new ListViewItem.ListViewSubItem[cols.Count - 1];

                    items[cols["name"].Index - 1] = new() { Text = $"{info.Name}" };
                    items[cols["width"].Index - 1] = new() { Text = $"{info.Width}" };
                    items[cols["height"].Index - 1] = new() { Text = $"{info.Height}" };
                    items[cols["offsetX"].Index - 1] = new() { Text = $"{info.OffsetX}" };
                    items[cols["offsetY"].Index - 1] = new() { Text = $"{info.OffsetY}" };

                    ListViewItem lvi = new() { UseItemStyleForSubItems = true, Text = string.Empty, Checked = ani.SelectedIndices[i] };
                    lvi.SubItems.AddRange(items);
                    lv.Items.Add(lvi);
                }
                lv.EndUpdate();

                this.RefreshImage();
            }
        }

        //复选框变化
        private void LvFrames_OnItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (this.ImageFrames is AniImage ani)
            {
                ListView lv = (ListView)sender;
                if (lv.Items.Count != ani.Frames.Count)
                {
                    return;
                }

                foreach (ListViewItem lvi in lv.Items)
                {
                    if (lvi is null)
                    {
                        return;
                    }
                }
                this.RefreshImage();
            }
        }
    }
}
