namespace GalleryViewerForm.Musica
{
    partial class MusicaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.SplitContainer scFileAndPic;
            System.Windows.Forms.SplitContainer scFileAndStack;
            System.Windows.Forms.ToolStripMenuItem btnAddImage;
            System.Windows.Forms.ToolStripMenuItem btnMoveUp;
            System.Windows.Forms.ToolStripMenuItem btnMoveDown;
            System.Windows.Forms.ToolStripMenuItem btnConfig;
            System.Windows.Forms.ToolStripMenuItem btnDelete;
            lbFiles = new System.Windows.Forms.ListBox();
            lvLayerStack = new System.Windows.Forms.ListView();
            picViewBox = new System.Windows.Forms.PictureBox();
            cmsFileListBox = new System.Windows.Forms.ContextMenuStrip(components);
            cmsLayerOperator = new System.Windows.Forms.ContextMenuStrip(components);
            scFileAndPic = new System.Windows.Forms.SplitContainer();
            scFileAndStack = new System.Windows.Forms.SplitContainer();
            btnAddImage = new System.Windows.Forms.ToolStripMenuItem();
            btnMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            btnMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            btnConfig = new System.Windows.Forms.ToolStripMenuItem();
            btnDelete = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)scFileAndPic).BeginInit();
            scFileAndPic.Panel1.SuspendLayout();
            scFileAndPic.Panel2.SuspendLayout();
            scFileAndPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scFileAndStack).BeginInit();
            scFileAndStack.Panel1.SuspendLayout();
            scFileAndStack.Panel2.SuspendLayout();
            scFileAndStack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picViewBox).BeginInit();
            cmsFileListBox.SuspendLayout();
            cmsLayerOperator.SuspendLayout();
            SuspendLayout();
            // 
            // scFileAndPic
            // 
            scFileAndPic.Dock = System.Windows.Forms.DockStyle.Fill;
            scFileAndPic.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            scFileAndPic.Location = new System.Drawing.Point(0, 0);
            scFileAndPic.Name = "scFileAndPic";
            // 
            // scFileAndPic.Panel1
            // 
            scFileAndPic.Panel1.Controls.Add(scFileAndStack);
            scFileAndPic.Panel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            scFileAndPic.Panel1MinSize = 100;
            // 
            // scFileAndPic.Panel2
            // 
            scFileAndPic.Panel2.AutoScroll = true;
            scFileAndPic.Panel2.Controls.Add(picViewBox);
            scFileAndPic.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            scFileAndPic.Panel2MinSize = 400;
            scFileAndPic.Size = new System.Drawing.Size(944, 502);
            scFileAndPic.SplitterDistance = 320;
            scFileAndPic.TabIndex = 0;
            scFileAndPic.TabStop = false;
            // 
            // scFileAndStack
            // 
            scFileAndStack.Dock = System.Windows.Forms.DockStyle.Fill;
            scFileAndStack.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            scFileAndStack.Location = new System.Drawing.Point(3, 3);
            scFileAndStack.Name = "scFileAndStack";
            scFileAndStack.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scFileAndStack.Panel1
            // 
            scFileAndStack.Panel1.Controls.Add(lbFiles);
            scFileAndStack.Panel1MinSize = 200;
            // 
            // scFileAndStack.Panel2
            // 
            scFileAndStack.Panel2.Controls.Add(lvLayerStack);
            scFileAndStack.Panel2MinSize = 100;
            scFileAndStack.Size = new System.Drawing.Size(317, 496);
            scFileAndStack.SplitterDistance = 340;
            scFileAndStack.TabIndex = 0;
            // 
            // lbFiles
            // 
            lbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            lbFiles.HorizontalScrollbar = true;
            lbFiles.IntegralHeight = false;
            lbFiles.ItemHeight = 17;
            lbFiles.Location = new System.Drawing.Point(0, 0);
            lbFiles.Margin = new System.Windows.Forms.Padding(0);
            lbFiles.Name = "lbFiles";
            lbFiles.ScrollAlwaysVisible = true;
            lbFiles.Size = new System.Drawing.Size(317, 340);
            lbFiles.TabIndex = 2;
            lbFiles.MouseUp += LbFiles_OnMouseUp;
            // 
            // lvLayerStack
            // 
            lvLayerStack.AutoArrange = false;
            lvLayerStack.CheckBoxes = true;
            lvLayerStack.Dock = System.Windows.Forms.DockStyle.Fill;
            lvLayerStack.FullRowSelect = true;
            lvLayerStack.GridLines = true;
            lvLayerStack.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lvLayerStack.LabelWrap = false;
            lvLayerStack.Location = new System.Drawing.Point(0, 0);
            lvLayerStack.Margin = new System.Windows.Forms.Padding(0);
            lvLayerStack.MultiSelect = false;
            lvLayerStack.Name = "lvLayerStack";
            lvLayerStack.ShowGroups = false;
            lvLayerStack.Size = new System.Drawing.Size(317, 152);
            lvLayerStack.TabIndex = 3;
            lvLayerStack.UseCompatibleStateImageBehavior = false;
            lvLayerStack.View = System.Windows.Forms.View.Details;
            lvLayerStack.ItemChecked += LvLayerStack_ItemChecked;
            lvLayerStack.MouseUp += LvLayerStack_OnMouseUp;
            // 
            // picViewBox
            // 
            picViewBox.Location = new System.Drawing.Point(0, 3);
            picViewBox.Margin = new System.Windows.Forms.Padding(0);
            picViewBox.Name = "picViewBox";
            picViewBox.Size = new System.Drawing.Size(100, 100);
            picViewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            picViewBox.TabIndex = 1;
            picViewBox.TabStop = false;
            // 
            // btnAddImage
            // 
            btnAddImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnAddImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnAddImage.Name = "btnAddImage";
            btnAddImage.ShowShortcutKeys = false;
            btnAddImage.Size = new System.Drawing.Size(91, 22);
            btnAddImage.Text = "添加图像";
            btnAddImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnAddImage.Click += BtnAddImage_OnClick;
            // 
            // btnMoveUp
            // 
            btnMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.ShowShortcutKeys = false;
            btnMoveUp.Size = new System.Drawing.Size(67, 22);
            btnMoveUp.Text = "上移";
            btnMoveUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnMoveUp.Click += BtnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.ShowShortcutKeys = false;
            btnMoveDown.Size = new System.Drawing.Size(67, 22);
            btnMoveDown.Text = "下移";
            btnMoveDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnMoveDown.Click += BtnMoveDown_Click;
            // 
            // btnConfig
            // 
            btnConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnConfig.Name = "btnConfig";
            btnConfig.ShowShortcutKeys = false;
            btnConfig.Size = new System.Drawing.Size(67, 22);
            btnConfig.Text = "配置";
            btnConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnConfig.Click += BtnConfig_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnDelete.Name = "btnDelete";
            btnDelete.ShowShortcutKeys = false;
            btnDelete.Size = new System.Drawing.Size(67, 22);
            btnDelete.Text = "删除";
            btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnDelete.Click += BtnDelete_Click;
            // 
            // cmsFileListBox
            // 
            cmsFileListBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cmsFileListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnAddImage });
            cmsFileListBox.Name = "cmsFileListBox";
            cmsFileListBox.ShowImageMargin = false;
            cmsFileListBox.ShowItemToolTips = false;
            cmsFileListBox.Size = new System.Drawing.Size(92, 26);
            // 
            // cmsLayerOperator
            // 
            cmsLayerOperator.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cmsLayerOperator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnMoveUp, btnMoveDown, btnConfig, btnDelete });
            cmsLayerOperator.Name = "cmsLayerOperator";
            cmsLayerOperator.ShowImageMargin = false;
            cmsLayerOperator.ShowItemToolTips = false;
            cmsLayerOperator.Size = new System.Drawing.Size(68, 92);
            // 
            // MusicaForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(944, 502);
            Controls.Add(scFileAndPic);
            DoubleBuffered = true;
            ImeMode = System.Windows.Forms.ImeMode.Disable;
            Name = "MusicaForm";
            ShowIcon = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "MusicaForm";
            FormClosed += MusicaForm_OnFormClosed;
            scFileAndPic.Panel1.ResumeLayout(false);
            scFileAndPic.Panel2.ResumeLayout(false);
            scFileAndPic.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scFileAndPic).EndInit();
            scFileAndPic.ResumeLayout(false);
            scFileAndStack.Panel1.ResumeLayout(false);
            scFileAndStack.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scFileAndStack).EndInit();
            scFileAndStack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picViewBox).EndInit();
            cmsFileListBox.ResumeLayout(false);
            cmsLayerOperator.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView lvLayerStack;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.PictureBox picViewBox;
        private System.Windows.Forms.ContextMenuStrip cmsFileListBox;
        private System.Windows.Forms.ContextMenuStrip cmsLayerOperator;
    }
}