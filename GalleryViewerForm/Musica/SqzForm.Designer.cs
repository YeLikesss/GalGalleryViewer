namespace GalleryViewerForm.Musica
{
    partial class SqzForm
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
            System.Windows.Forms.StatusStrip statusStrip;
            System.Windows.Forms.SplitContainer scFrameListAndReview;
            tssLabelName = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabelFrameSelected = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabelResolution = new System.Windows.Forms.ToolStripStatusLabel();
            lbFrames = new System.Windows.Forms.ListBox();
            picViewBox = new System.Windows.Forms.PictureBox();
            statusStrip = new System.Windows.Forms.StatusStrip();
            scFrameListAndReview = new System.Windows.Forms.SplitContainer();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scFrameListAndReview).BeginInit();
            scFrameListAndReview.Panel1.SuspendLayout();
            scFrameListAndReview.Panel2.SuspendLayout();
            scFrameListAndReview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picViewBox).BeginInit();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tssLabelName, tssLabelFrameSelected, tssLabelResolution });
            statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip.Location = new System.Drawing.Point(0, 746);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1424, 26);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 0;
            // 
            // tssLabelName
            // 
            tssLabelName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            tssLabelName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelName.Name = "tssLabelName";
            tssLabelName.Padding = new System.Windows.Forms.Padding(0, 0, 60, 0);
            tssLabelName.Size = new System.Drawing.Size(120, 21);
            tssLabelName.Text = "图像名称";
            tssLabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssLabelFrameSelected
            // 
            tssLabelFrameSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelFrameSelected.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            tssLabelFrameSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelFrameSelected.Name = "tssLabelFrameSelected";
            tssLabelFrameSelected.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            tssLabelFrameSelected.Size = new System.Drawing.Size(78, 21);
            tssLabelFrameSelected.Text = "帧选择";
            tssLabelFrameSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tssLabelFrameSelected.Click += TssLabelFrameSelected_OnClick;
            // 
            // tssLabelResolution
            // 
            tssLabelResolution.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelResolution.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            tssLabelResolution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelResolution.Name = "tssLabelResolution";
            tssLabelResolution.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            tssLabelResolution.Size = new System.Drawing.Size(78, 21);
            tssLabelResolution.Text = "分辨率";
            tssLabelResolution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scFrameListAndReview
            // 
            scFrameListAndReview.Dock = System.Windows.Forms.DockStyle.Fill;
            scFrameListAndReview.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            scFrameListAndReview.Location = new System.Drawing.Point(0, 0);
            scFrameListAndReview.Name = "scFrameListAndReview";
            // 
            // scFrameListAndReview.Panel1
            // 
            scFrameListAndReview.Panel1.Controls.Add(lbFrames);
            scFrameListAndReview.Panel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            scFrameListAndReview.Panel1MinSize = 100;
            // 
            // scFrameListAndReview.Panel2
            // 
            scFrameListAndReview.Panel2.AutoScroll = true;
            scFrameListAndReview.Panel2.Controls.Add(picViewBox);
            scFrameListAndReview.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            scFrameListAndReview.Panel2MinSize = 400;
            scFrameListAndReview.Size = new System.Drawing.Size(1424, 746);
            scFrameListAndReview.SplitterDistance = 200;
            scFrameListAndReview.TabIndex = 1;
            // 
            // lbFrames
            // 
            lbFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            lbFrames.HorizontalScrollbar = true;
            lbFrames.IntegralHeight = false;
            lbFrames.ItemHeight = 17;
            lbFrames.Location = new System.Drawing.Point(3, 3);
            lbFrames.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            lbFrames.Name = "lbFrames";
            lbFrames.ScrollAlwaysVisible = true;
            lbFrames.Size = new System.Drawing.Size(197, 740);
            lbFrames.TabIndex = 3;
            lbFrames.SelectedIndexChanged += LbFrames_OnSelectedIndexChanged;
            // 
            // picViewBox
            // 
            picViewBox.Location = new System.Drawing.Point(0, 3);
            picViewBox.Margin = new System.Windows.Forms.Padding(0);
            picViewBox.Name = "picViewBox";
            picViewBox.Size = new System.Drawing.Size(100, 100);
            picViewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            picViewBox.TabIndex = 0;
            picViewBox.TabStop = false;
            // 
            // SqzForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            ClientSize = new System.Drawing.Size(1424, 772);
            Controls.Add(scFrameListAndReview);
            Controls.Add(statusStrip);
            DoubleBuffered = true;
            ImeMode = System.Windows.Forms.ImeMode.Disable;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(800, 600);
            Name = "SqzForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Sqz配置";
            Load += SqzForm_OnLoad;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            scFrameListAndReview.Panel1.ResumeLayout(false);
            scFrameListAndReview.Panel2.ResumeLayout(false);
            scFrameListAndReview.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scFrameListAndReview).EndInit();
            scFrameListAndReview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picViewBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tssLabelName;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelFrameSelected;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelResolution;
        private System.Windows.Forms.ListBox lbFrames;
        private System.Windows.Forms.PictureBox picViewBox;
    }
}