namespace GalleryViewerForm
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem btnFile;
            System.Windows.Forms.ToolStripMenuItem btnOpenDirectory;
            System.Windows.Forms.ToolStripSeparator separatorOpenToExport;
            System.Windows.Forms.ToolStripMenuItem btnExport;
            System.Windows.Forms.ToolStripMenuItem btnExportAll;
            System.Windows.Forms.ToolStripMenuItem btnView;
            System.Windows.Forms.ToolStripMenuItem btnMusica;
            System.Windows.Forms.ToolStripMenuItem btnSetting;
            System.Windows.Forms.ToolStripMenuItem btnExportFormat;
            System.Windows.Forms.ToolStripMenuItem btnWebpFormat;
            System.Windows.Forms.ToolStripMenuItem btnPngFormat;
            System.Windows.Forms.ToolStripMenuItem btnJpegFormat;
            System.Windows.Forms.ToolStripMenuItem btnTgaFormat;
            System.Windows.Forms.ToolStripMenuItem btnBmpFormat;
            System.Windows.Forms.ToolStripSeparator separatorFormatToRender;
            System.Windows.Forms.ToolStripMenuItem btnRender;
            System.Windows.Forms.ToolStripMenuItem btnScalarMode;
            System.Windows.Forms.ToolStripMenuItem btnVectorMode;
            System.Windows.Forms.StatusStrip statusStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tssLabelMode = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabalSourceDirectory = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabalStatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabelProgress = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabelRender = new System.Windows.Forms.ToolStripStatusLabel();
            tssLabelEncodeMode = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip = new System.Windows.Forms.MenuStrip();
            panelView = new System.Windows.Forms.Panel();
            btnFile = new System.Windows.Forms.ToolStripMenuItem();
            btnOpenDirectory = new System.Windows.Forms.ToolStripMenuItem();
            separatorOpenToExport = new System.Windows.Forms.ToolStripSeparator();
            btnExport = new System.Windows.Forms.ToolStripMenuItem();
            btnExportAll = new System.Windows.Forms.ToolStripMenuItem();
            btnView = new System.Windows.Forms.ToolStripMenuItem();
            btnMusica = new System.Windows.Forms.ToolStripMenuItem();
            btnSetting = new System.Windows.Forms.ToolStripMenuItem();
            btnExportFormat = new System.Windows.Forms.ToolStripMenuItem();
            btnWebpFormat = new System.Windows.Forms.ToolStripMenuItem();
            btnPngFormat = new System.Windows.Forms.ToolStripMenuItem();
            btnJpegFormat = new System.Windows.Forms.ToolStripMenuItem();
            btnTgaFormat = new System.Windows.Forms.ToolStripMenuItem();
            btnBmpFormat = new System.Windows.Forms.ToolStripMenuItem();
            separatorFormatToRender = new System.Windows.Forms.ToolStripSeparator();
            btnRender = new System.Windows.Forms.ToolStripMenuItem();
            btnScalarMode = new System.Windows.Forms.ToolStripMenuItem();
            btnVectorMode = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip = new System.Windows.Forms.StatusStrip();
            statusStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // btnFile
            // 
            btnFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnOpenDirectory, separatorOpenToExport, btnExport, btnExportAll });
            btnFile.Name = "btnFile";
            btnFile.ShowShortcutKeys = false;
            btnFile.Size = new System.Drawing.Size(44, 21);
            btnFile.Text = "文件";
            // 
            // btnOpenDirectory
            // 
            btnOpenDirectory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnOpenDirectory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnOpenDirectory.Name = "btnOpenDirectory";
            btnOpenDirectory.ShowShortcutKeys = false;
            btnOpenDirectory.Size = new System.Drawing.Size(128, 22);
            btnOpenDirectory.Text = "打开文件夹";
            btnOpenDirectory.Click += File_OpenDirectory;
            // 
            // separatorOpenToExport
            // 
            separatorOpenToExport.Name = "separatorOpenToExport";
            separatorOpenToExport.Size = new System.Drawing.Size(125, 6);
            // 
            // btnExport
            // 
            btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnExport.Name = "btnExport";
            btnExport.ShowShortcutKeys = false;
            btnExport.Size = new System.Drawing.Size(128, 22);
            btnExport.Text = "导出";
            btnExport.Click += File_Export;
            // 
            // btnExportAll
            // 
            btnExportAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnExportAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnExportAll.Name = "btnExportAll";
            btnExportAll.ShowShortcutKeys = false;
            btnExportAll.Size = new System.Drawing.Size(128, 22);
            btnExportAll.Text = "导出所有帧";
            btnExportAll.Click += File_ExportAll;
            // 
            // btnView
            // 
            btnView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnMusica });
            btnView.Name = "btnView";
            btnView.ShowShortcutKeys = false;
            btnView.Size = new System.Drawing.Size(44, 21);
            btnView.Text = "视图";
            // 
            // btnMusica
            // 
            btnMusica.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnMusica.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnMusica.Name = "btnMusica";
            btnMusica.ShowShortcutKeys = false;
            btnMusica.Size = new System.Drawing.Size(109, 22);
            btnMusica.Tag = "Musica";
            btnMusica.Text = "Musica";
            btnMusica.Click += SelectView_OnClick;
            // 
            // btnSetting
            // 
            btnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnExportFormat, separatorFormatToRender, btnRender });
            btnSetting.Name = "btnSetting";
            btnSetting.ShowShortcutKeys = false;
            btnSetting.Size = new System.Drawing.Size(44, 21);
            btnSetting.Text = "设置";
            // 
            // btnExportFormat
            // 
            btnExportFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnExportFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnExportFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnWebpFormat, btnPngFormat, btnJpegFormat, btnTgaFormat, btnBmpFormat });
            btnExportFormat.Name = "btnExportFormat";
            btnExportFormat.ShowShortcutKeys = false;
            btnExportFormat.Size = new System.Drawing.Size(116, 22);
            btnExportFormat.Text = "导出格式";
            // 
            // btnWebpFormat
            // 
            btnWebpFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnWebpFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnWebpFormat.Name = "btnWebpFormat";
            btnWebpFormat.ShowShortcutKeys = false;
            btnWebpFormat.Size = new System.Drawing.Size(102, 22);
            btnWebpFormat.Tag = "webp";
            btnWebpFormat.Text = "WEBP";
            btnWebpFormat.Click += BtnEncodeFormat_Click;
            // 
            // btnPngFormat
            // 
            btnPngFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnPngFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnPngFormat.Name = "btnPngFormat";
            btnPngFormat.ShowShortcutKeys = false;
            btnPngFormat.Size = new System.Drawing.Size(102, 22);
            btnPngFormat.Tag = "png";
            btnPngFormat.Text = "PNG";
            btnPngFormat.Click += BtnEncodeFormat_Click;
            // 
            // btnJpegFormat
            // 
            btnJpegFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnJpegFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnJpegFormat.Name = "btnJpegFormat";
            btnJpegFormat.ShowShortcutKeys = false;
            btnJpegFormat.Size = new System.Drawing.Size(102, 22);
            btnJpegFormat.Tag = "jpeg";
            btnJpegFormat.Text = "JPEG";
            btnJpegFormat.Click += BtnEncodeFormat_Click;
            // 
            // btnTgaFormat
            // 
            btnTgaFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnTgaFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnTgaFormat.Name = "btnTgaFormat";
            btnTgaFormat.ShowShortcutKeys = false;
            btnTgaFormat.Size = new System.Drawing.Size(102, 22);
            btnTgaFormat.Tag = "tga";
            btnTgaFormat.Text = "TGA";
            btnTgaFormat.Click += BtnEncodeFormat_Click;
            // 
            // btnBmpFormat
            // 
            btnBmpFormat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnBmpFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnBmpFormat.Name = "btnBmpFormat";
            btnBmpFormat.ShowShortcutKeys = false;
            btnBmpFormat.Size = new System.Drawing.Size(102, 22);
            btnBmpFormat.Tag = "bmp";
            btnBmpFormat.Text = "BMP";
            btnBmpFormat.Click += BtnEncodeFormat_Click;
            // 
            // separatorFormatToRender
            // 
            separatorFormatToRender.Name = "separatorFormatToRender";
            separatorFormatToRender.Size = new System.Drawing.Size(113, 6);
            // 
            // btnRender
            // 
            btnRender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnRender.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnRender.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { btnScalarMode, btnVectorMode });
            btnRender.Name = "btnRender";
            btnRender.ShowShortcutKeys = false;
            btnRender.Size = new System.Drawing.Size(116, 22);
            btnRender.Text = "渲染器";
            // 
            // btnScalarMode
            // 
            btnScalarMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnScalarMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnScalarMode.Name = "btnScalarMode";
            btnScalarMode.ShowShortcutKeys = false;
            btnScalarMode.Size = new System.Drawing.Size(186, 22);
            btnScalarMode.Tag = "scalar";
            btnScalarMode.Text = "IA32-AMD64[最精确]";
            btnScalarMode.Click += BtnRenderMode_Click;
            // 
            // btnVectorMode
            // 
            btnVectorMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnVectorMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnVectorMode.Name = "btnVectorMode";
            btnVectorMode.ShowShortcutKeys = false;
            btnVectorMode.Size = new System.Drawing.Size(186, 22);
            btnVectorMode.Tag = "vector";
            btnVectorMode.Text = "AVX256[速度最快]";
            btnVectorMode.Click += BtnRenderMode_Click;
            // 
            // statusStrip
            // 
            statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tssLabelMode, tssLabalSourceDirectory, tssLabalStatusMsg, tssLabelProgress, tssLabelRender, tssLabelEncodeMode });
            statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip.Location = new System.Drawing.Point(0, 840);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1584, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 1;
            // 
            // tssLabelMode
            // 
            tssLabelMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelMode.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            tssLabelMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelMode.Name = "tssLabelMode";
            tssLabelMode.Padding = new System.Windows.Forms.Padding(0, 0, 60, 0);
            tssLabelMode.Size = new System.Drawing.Size(64, 17);
            tssLabelMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssLabalSourceDirectory
            // 
            tssLabalSourceDirectory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabalSourceDirectory.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            tssLabalSourceDirectory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabalSourceDirectory.Name = "tssLabalSourceDirectory";
            tssLabalSourceDirectory.Padding = new System.Windows.Forms.Padding(0, 0, 60, 0);
            tssLabalSourceDirectory.Size = new System.Drawing.Size(64, 17);
            tssLabalSourceDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssLabalStatusMsg
            // 
            tssLabalStatusMsg.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            tssLabalStatusMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabalStatusMsg.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            tssLabalStatusMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabalStatusMsg.Name = "tssLabalStatusMsg";
            tssLabalStatusMsg.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            tssLabalStatusMsg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            tssLabalStatusMsg.Size = new System.Drawing.Size(24, 17);
            tssLabalStatusMsg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tssLabelProgress
            // 
            tssLabelProgress.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            tssLabelProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelProgress.Name = "tssLabelProgress";
            tssLabelProgress.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            tssLabelProgress.Size = new System.Drawing.Size(64, 17);
            // 
            // tssLabelRender
            // 
            tssLabelRender.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            tssLabelRender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelRender.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            tssLabelRender.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelRender.Name = "tssLabelRender";
            tssLabelRender.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            tssLabelRender.Size = new System.Drawing.Size(24, 17);
            // 
            // tssLabelEncodeMode
            // 
            tssLabelEncodeMode.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            tssLabelEncodeMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            tssLabelEncodeMode.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            tssLabelEncodeMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tssLabelEncodeMode.Name = "tssLabelEncodeMode";
            tssLabelEncodeMode.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            tssLabelEncodeMode.Size = new System.Drawing.Size(24, 17);
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnFile, btnView, btnSetting });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(1584, 25);
            menuStrip.TabIndex = 0;
            // 
            // panelView
            // 
            panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            panelView.Location = new System.Drawing.Point(0, 25);
            panelView.Name = "panelView";
            panelView.Size = new System.Drawing.Size(1584, 815);
            panelView.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1584, 862);
            Controls.Add(panelView);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            ImeMode = System.Windows.Forms.ImeMode.Disable;
            MainMenuStrip = menuStrip;
            MinimumSize = new System.Drawing.Size(800, 600);
            Name = "MainForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Galgame立绘合成 - 20241211";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelMode;
        private System.Windows.Forms.ToolStripStatusLabel tssLabalSourceDirectory;
        private System.Windows.Forms.ToolStripStatusLabel tssLabalStatusMsg;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelProgress;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelRender;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelEncodeMode;
        private System.Windows.Forms.ToolStripMenuItem btnBmpFormat;
    }
}