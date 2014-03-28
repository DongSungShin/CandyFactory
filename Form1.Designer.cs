namespace CandyFactoryMapEditor
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ExportMapFile = new System.Windows.Forms.Button();
            this.LoadFile = new System.Windows.Forms.Button();
            this.SaveFile = new System.Windows.Forms.Button();
            this.MapOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Resize += new System.EventHandler(this.OnResize);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.Panel2.Controls.Add(this.ExportMapFile);
            this.splitContainer1.Panel2.Controls.Add(this.LoadFile);
            this.splitContainer1.Panel2.Controls.Add(this.SaveFile);
            this.splitContainer1.Panel2.Controls.Add(this.MapOpen);
            this.splitContainer1.Size = new System.Drawing.Size(1126, 624);
            this.splitContainer1.SplitterDistance = 817;
            this.splitContainer1.TabIndex = 0;
            // 
            // ExportMapFile
            // 
            this.ExportMapFile.Location = new System.Drawing.Point(15, 180);
            this.ExportMapFile.Name = "ExportMapFile";
            this.ExportMapFile.Size = new System.Drawing.Size(162, 31);
            this.ExportMapFile.TabIndex = 3;
            this.ExportMapFile.Text = "맵파일로저장";
            this.ExportMapFile.UseVisualStyleBackColor = true;
            this.ExportMapFile.Click += new System.EventHandler(this.ExportMapFile_Click);
            // 
            // LoadFile
            // 
            this.LoadFile.Location = new System.Drawing.Point(15, 120);
            this.LoadFile.Name = "LoadFile";
            this.LoadFile.Size = new System.Drawing.Size(162, 29);
            this.LoadFile.TabIndex = 2;
            this.LoadFile.Text = "파일 불러오기";
            this.LoadFile.UseVisualStyleBackColor = true;
            this.LoadFile.Click += new System.EventHandler(this.LoadFile_Click);
            // 
            // SaveFile
            // 
            this.SaveFile.Location = new System.Drawing.Point(15, 65);
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(162, 27);
            this.SaveFile.TabIndex = 1;
            this.SaveFile.Text = "파일 저장";
            this.SaveFile.UseVisualStyleBackColor = true;
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // MapOpen
            // 
            this.MapOpen.Location = new System.Drawing.Point(15, 12);
            this.MapOpen.Name = "MapOpen";
            this.MapOpen.Size = new System.Drawing.Size(162, 27);
            this.MapOpen.TabIndex = 0;
            this.MapOpen.Text = "맵 파일 가져오기";
            this.MapOpen.UseVisualStyleBackColor = true;
            this.MapOpen.Click += new System.EventHandler(this.MapOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 624);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button MapOpen;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button LoadFile;
        private System.Windows.Forms.Button SaveFile;
        private System.Windows.Forms.Button ExportMapFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;


    }
}

