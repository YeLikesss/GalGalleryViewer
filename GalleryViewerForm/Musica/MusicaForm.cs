using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using GalleryFormat.Base;
using GalleryFormat.Format.Musica;
using GalleryViewerForm.Base;

namespace GalleryViewerForm.Musica
{
    public partial class MusicaForm : Form, IOperator
    {
        public MusicaForm()
        {
            InitializeComponent();

            ListBox lb = this.lbFiles;
            lb.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(lb, true);

            ListView lv = this.lvLayerStack;
            lv.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(lv, true);

            ColumnHeader ch0 = new() { Name = "index", Text = string.Empty, Width = 30 };
            ColumnHeader ch1 = new() { Name = "name", Text = "名字", Width = 160, TextAlign = HorizontalAlignment.Center };
            ColumnHeader ch2 = new() { Name = "type", Text = "类型", Width = 60, TextAlign = HorizontalAlignment.Center };

            lv.Columns.Add(ch0);
            lv.Columns.Add(ch1);
            lv.Columns.Add(ch2);
        }

        private List<string> mFiles = new();        //文件列表
        private readonly AniForm mAniForm = new();
        private readonly SqzForm mSqzForm = new();
        private bool mDisableItemCheckedEvent = false;

        public void SetSourceDirectory(string directory)
        {
            this.Clear();

            //获取文件列表
            List<string> files = MusicaImageFactory.GetFiles(directory);
            ListBox lb = this.lbFiles;
            lb.BeginUpdate();
            lb.Items.AddRange(files.ConvertAll(s => Path.GetFileName(s)).ToArray());
            lb.EndUpdate();
            this.mFiles = files;
        }

        public async Task<ResultState<string>> ExtractAsync(IProgress<ProgressInfo>? progress)
        {
            List<IMusicaImage> imgs = this.GetSelectedImage();
            if (!imgs.Any())
            {
                return ResultState<string>.Fail("未选择图层");
            }

            bool result = await MusicaImageFactory.ExtractAsync(imgs, progress);
            if (!result)
            {
                return ResultState<string>.Fail("合成出错");
            }

            return ResultState<string>.Success();
        }

        public async Task<ResultState<string>> ExtractAllAsync(IProgress<ProgressInfo>? progress)
        {
            List<IMusicaImage> imgs = this.GetSelectedImage();
            if (!imgs.Any())
            {
                return ResultState<string>.Fail("未选择图层");
            }

            bool result = await MusicaImageFactory.ExtractAllAsync(imgs, progress);
            if (!result)
            {
                return ResultState<string>.Fail("合成出错");
            }

            return ResultState<string>.Success();
        }

        /// <summary>
        /// 获取已选图片
        /// </summary>
        private List<IMusicaImage> GetSelectedImage()
        {
            ListView lv = this.lvLayerStack;
            List<IMusicaImage> imgs = new(lv.Items.Count);
            for (int i = 0; i < lv.Items.Count; ++i)
            {
                ListViewItem lvi = lv.Items[i];
                if (lvi.Checked && lvi.Tag is IMusicaImage img)
                {
                    imgs.Add(img);
                }
            }
            return imgs;
        }

        /// <summary>
        /// 刷新图像
        /// </summary>
        private void RefreshImage()
        {
            List<IMusicaImage> imgs = this.GetSelectedImage();
            using Image<Rgba32>? pic = MusicaImageFactory.GetImage(imgs);

            using System.Drawing.Image org = this.picViewBox.Image;
            this.picViewBox.Image = pic?.ToBitmap();
        }

        /// <summary>
        /// 清空显示
        /// </summary>
        private void Clear()
        {
            this.lbFiles.Items.Clear();
            this.lvLayerStack.Items.Clear();

            using System.Drawing.Image org = this.picViewBox.Image;
            this.picViewBox.Image = null;
        }

        //文件列表框鼠标抬起
        private void LbFiles_OnMouseUp(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            //右键
            if (e.Button == MouseButtons.Right)
            {
                int idx = lb.IndexFromPoint(e.Location);
                if (idx >= 0)
                {
                    lb.SetSelected(idx, true);
                    this.cmsFileListBox.Show(lb, e.Location);
                }
            }
        }

        //添加图片按钮
        private void BtnAddImage_OnClick(object sender, EventArgs e)
        {
            int idx = this.lbFiles.SelectedIndex;
            string file = this.mFiles[idx];

            IMusicaImage? musicaImg = MusicaImageFactory.CreateInstance(file);
            if (musicaImg is null)
            {
                MessageBox.Show("图像加载失败", "Musica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.mDisableItemCheckedEvent = true;

            ListView lv = this.lvLayerStack;
            ListView.ColumnHeaderCollection cols = lv.Columns;
            lv.BeginUpdate();

            ListViewItem.ListViewSubItem[] items = new ListViewItem.ListViewSubItem[cols.Count - 1];

            items[cols["name"].Index - 1] = new() { Text = $"{musicaImg.Name}" };
            items[cols["type"].Index - 1] = new() { Text = $"{musicaImg.Type}" };

            //绑定图像对象
            ListViewItem lvi = new() { UseItemStyleForSubItems = true, Text = string.Empty, Tag = musicaImg };
            lvi.SubItems.AddRange(items);
            lv.Items.Add(lvi);

            lv.EndUpdate();

            this.mDisableItemCheckedEvent = false;
        }

        //图层列表鼠标抬起
        private void LvLayerStack_OnMouseUp(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            //右键
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = lv.HitTest(e.Location);
                if (info.Item is not null)
                {
                    info.Item.Selected = true;
                    this.cmsLayerOperator.Show(lv, e.Location);
                }
            }
        }

        //上移按钮
        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            ListView lv = this.lvLayerStack;
            ListViewItem lvi = lv.SelectedItems[0];

            int curIdx = lvi.Index;
            //最顶层
            if (curIdx == 0)
            {
                return;
            }

            this.mDisableItemCheckedEvent = true;
            lv.BeginUpdate();
            lv.Items.RemoveAt(curIdx);
            lv.Items.Insert(curIdx - 1, lvi);
            lv.EndUpdate();
            this.mDisableItemCheckedEvent = false;

            this.RefreshImage();
        }

        //下移按钮
        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            ListView lv = this.lvLayerStack;
            ListViewItem lvi = lv.SelectedItems[0];

            int curIdx = lvi.Index;
            //最底层
            if (curIdx == lv.Items.Count - 1)
            {
                return;
            }

            this.mDisableItemCheckedEvent = true;
            lv.BeginUpdate();
            lv.Items.RemoveAt(curIdx);
            lv.Items.Insert(curIdx + 1, lvi);
            lv.EndUpdate();
            this.mDisableItemCheckedEvent = false;

            this.RefreshImage();
        }

        //配置按钮
        private void BtnConfig_Click(object sender, EventArgs e)
        {
            if (this.lvLayerStack.SelectedItems[0].Tag is IMusicaImage musicaImage)
            {
                if (musicaImage is AniImage ani)
                {
                    this.mAniForm.ImageFrames = ani;
                    this.mAniForm.ShowDialog();

                    this.RefreshImage();
                }
                else if (musicaImage is SqzImage sqz)
                {
                    this.mSqzForm.ImageFrames = sqz;
                    this.mSqzForm.ShowDialog();

                    this.RefreshImage();
                }
            }
        }

        //删除按钮
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            this.mDisableItemCheckedEvent = true;
            ListView lv = this.lvLayerStack;
            lv.Items.RemoveAt(lv.SelectedIndices[0]);
            this.mDisableItemCheckedEvent = false;

            this.RefreshImage();
        }

        //复选框更改
        private void LvLayerStack_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (this.mDisableItemCheckedEvent)
            {
                return;
            }

            ListView lv = (ListView)sender;
            foreach (ListViewItem lvi in lv.Items)
            {
                if (lvi is null)
                {
                    return;
                }
            }
            this.RefreshImage();
        }

        //关闭窗体
        private void MusicaForm_OnFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Clear();
        }
    }
}
