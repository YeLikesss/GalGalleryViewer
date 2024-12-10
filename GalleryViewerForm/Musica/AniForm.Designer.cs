namespace GalleryViewerForm.Musica
{
    partial class AniForm
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
            System.Windows.Forms.SplitContainer scPicAndFrames;
            tssLabelName = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabelResolution = new System.Windows.Forms.ToolStripStatusLabel();
            picViewBox = new System.Windows.Forms.PictureBox();
            lvFrames = new System.Windows.Forms.ListView();
            statusStrip = new System.Windows.Forms.StatusStrip();
            scPicAndFrames = new System.Windows.Forms.SplitContainer();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scPicAndFrames).BeginInit();
            scPicAndFrames.Panel1.SuspendLayout();
            scPicAndFrames.Panel2.SuspendLayout();
            scPicAndFrames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picViewBox).BeginInit();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tssLabelName, tssLabelResolution });
            statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip.Location = new System.Drawing.Point(0, 746);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1424, 26);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 1;
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
            // scPicAndFrames
            // 
            scPicAndFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            scPicAndFrames.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            scPicAndFrames.Location = new System.Drawing.Point(0, 0);
            scPicAndFrames.Name = "scPicAndFrames";
            scPicAndFrames.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scPicAndFrames.Panel1
            // 
            scPicAndFrames.Panel1.AutoScroll = true;
            scPicAndFrames.Panel1.BackColor = System.Drawing.SystemColors.Control;
            scPicAndFrames.Panel1.Controls.Add(picViewBox);
            scPicAndFrames.Panel1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            scPicAndFrames.Panel1MinSize = 400;
            // 
            // scPicAndFrames.Panel2
            // 
            scPicAndFrames.Panel2.Controls.Add(lvFrames);
            scPicAndFrames.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            scPicAndFrames.Panel2MinSize = 160;
            scPicAndFrames.Size = new System.Drawing.Size(1424, 746);
            scPicAndFrames.SplitterDistance = 612;
            scPicAndFrames.TabIndex = 5;
            // 
            // picViewBox
            // 
            picViewBox.Location = new System.Drawing.Point(3, 3);
            picViewBox.Margin = new System.Windows.Forms.Padding(0);
            picViewBox.Name = "picViewBox";
            picViewBox.Size = new System.Drawing.Size(100, 100);
            picViewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            picViewBox.TabIndex = 0;
            picViewBox.TabStop = false;
            // 
            // lvFrames
            // 
            lvFrames.AutoArrange = false;
            lvFrames.CheckBoxes = true;
            lvFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            lvFrames.FullRowSelect = true;
            lvFrames.GridLines = true;
            lvFrames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lvFrames.LabelWrap = false;
            lvFrames.Location = new System.Drawing.Point(3, 0);
            lvFrames.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            lvFrames.MultiSelect = false;
            lvFrames.Name = "lvFrames";
            lvFrames.ShowGroups = false;
            lvFrames.Size = new System.Drawing.Size(1418, 127);
            lvFrames.TabIndex = 4;
            lvFrames.UseCompatibleStateImageBehavior = false;
            lvFrames.View = System.Windows.Forms.View.Details;
            lvFrames.ItemChecked += LvFrames_OnItemChecked;
            // 
            // AniForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            ClientSize = new System.Drawing.Size(1424, 772);
            Controls.Add(scPicAndFrames);
            Controls.Add(statusStrip);
            DoubleBuffered = true;
            ImeMode = System.Windows.Forms.ImeMode.Disable;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(800, 600);
            Name = "AniForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Ani配置";
            Load += AniForm_OnLoad;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            scPicAndFrames.Panel1.ResumeLayout(false);
            scPicAndFrames.Panel1.PerformLayout();
            scPicAndFrames.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scPicAndFrames).EndInit();
            scPicAndFrames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picViewBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tssLabelName;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelResolution;
        private System.Windows.Forms.PictureBox picViewBox;
        private System.Windows.Forms.ListView lvFrames;
    }
}