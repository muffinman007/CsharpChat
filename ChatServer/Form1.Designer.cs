namespace ChatServer
{
    partial class Form1
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
			this.btnRun = new System.Windows.Forms.Button();
			this.tbLog = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.lbUpdateHack = new System.Windows.Forms.Label();
			this.btnUpdateHack = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(12, 12);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(75, 23);
			this.btnRun.TabIndex = 0;
			this.btnRun.Text = "&Run ";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// tbLog
			// 
			this.tbLog.Location = new System.Drawing.Point(12, 63);
			this.tbLog.MaxLength = 500000;
			this.tbLog.Multiline = true;
			this.tbLog.Name = "tbLog";
			this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbLog.Size = new System.Drawing.Size(305, 136);
			this.tbLog.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Connection Log:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(191, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Binding Port:";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(264, 15);
			this.tbPort.MaxLength = 5;
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(53, 20);
			this.tbPort.TabIndex = 2;
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(97, 12);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 1;
			this.btnStop.Text = "&Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// lbUpdateHack
			// 
			this.lbUpdateHack.AutoSize = true;
			this.lbUpdateHack.Enabled = false;
			this.lbUpdateHack.Location = new System.Drawing.Point(194, 35);
			this.lbUpdateHack.Name = "lbUpdateHack";
			this.lbUpdateHack.Size = new System.Drawing.Size(0, 13);
			this.lbUpdateHack.TabIndex = 4;
			this.lbUpdateHack.Visible = false;
			// 
			// btnUpdateHack
			// 
			this.btnUpdateHack.Enabled = false;
			this.btnUpdateHack.Location = new System.Drawing.Point(242, 37);
			this.btnUpdateHack.Name = "btnUpdateHack";
			this.btnUpdateHack.Size = new System.Drawing.Size(75, 23);
			this.btnUpdateHack.TabIndex = 5;
			this.btnUpdateHack.TabStop = false;
			this.btnUpdateHack.Text = "update hack";
			this.btnUpdateHack.UseVisualStyleBackColor = true;
			this.btnUpdateHack.Visible = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(330, 211);
			this.Controls.Add(this.btnUpdateHack);
			this.Controls.Add(this.lbUpdateHack);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.tbPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbLog);
			this.Controls.Add(this.btnRun);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximumSize = new System.Drawing.Size(346, 249);
			this.MinimumSize = new System.Drawing.Size(346, 249);
			this.Name = "Form1";
			this.Text = "Chat Server";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Label lbUpdateHack;
		private System.Windows.Forms.Button btnUpdateHack;
    }
}

