namespace CrazyDancer
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.picKey1 = new System.Windows.Forms.PictureBox();
            this.btnDance = new System.Windows.Forms.Button();
            this.picKey2 = new System.Windows.Forms.PictureBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.picBall1 = new System.Windows.Forms.PictureBox();
            this.picBall2 = new System.Windows.Forms.PictureBox();
            this.labIndicator = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picKey1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKey2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBall1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBall2)).BeginInit();
            this.SuspendLayout();
            // 
            // picKey1
            // 
            this.picKey1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picKey1.Location = new System.Drawing.Point(12, 12);
            this.picKey1.Name = "picKey1";
            this.picKey1.Size = new System.Drawing.Size(380, 50);
            this.picKey1.TabIndex = 0;
            this.picKey1.TabStop = false;
            // 
            // btnDance
            // 
            this.btnDance.Location = new System.Drawing.Point(51, 195);
            this.btnDance.Name = "btnDance";
            this.btnDance.Size = new System.Drawing.Size(300, 60);
            this.btnDance.TabIndex = 1;
            this.btnDance.Text = "Dance!";
            this.btnDance.UseVisualStyleBackColor = true;
            this.btnDance.Click += new System.EventHandler(this.btnDance_Click);
            // 
            // picKey2
            // 
            this.picKey2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picKey2.Location = new System.Drawing.Point(12, 68);
            this.picKey2.Name = "picKey2";
            this.picKey2.Size = new System.Drawing.Size(380, 50);
            this.picKey2.TabIndex = 2;
            this.picKey2.TabStop = false;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(410, 12);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(104, 288);
            this.txtLog.TabIndex = 3;
            // 
            // picBall1
            // 
            this.picBall1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBall1.Location = new System.Drawing.Point(12, 124);
            this.picBall1.Name = "picBall1";
            this.picBall1.Size = new System.Drawing.Size(177, 23);
            this.picBall1.TabIndex = 4;
            this.picBall1.TabStop = false;
            // 
            // picBall2
            // 
            this.picBall2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBall2.Location = new System.Drawing.Point(195, 124);
            this.picBall2.Name = "picBall2";
            this.picBall2.Size = new System.Drawing.Size(177, 23);
            this.picBall2.TabIndex = 5;
            this.picBall2.TabStop = false;
            // 
            // labIndicator
            // 
            this.labIndicator.BackColor = System.Drawing.Color.Red;
            this.labIndicator.Location = new System.Drawing.Point(378, 124);
            this.labIndicator.Name = "labIndicator";
            this.labIndicator.Size = new System.Drawing.Size(14, 23);
            this.labIndicator.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 312);
            this.Controls.Add(this.labIndicator);
            this.Controls.Add(this.picBall2);
            this.Controls.Add(this.picBall1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.picKey2);
            this.Controls.Add(this.btnDance);
            this.Controls.Add(this.picKey1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CrazyDancer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picKey1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKey2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBall1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBall2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.PictureBox picKey1;
        private System.Windows.Forms.Button btnDance;
        private System.Windows.Forms.PictureBox picKey2;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.PictureBox picBall1;
        private System.Windows.Forms.PictureBox picBall2;
        private System.Windows.Forms.Label labIndicator;
    }
}

