namespace ChatApplicaiton
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
			this.label1 = new System.Windows.Forms.Label();
			this.tbIP = new System.Windows.Forms.TextBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.tbChatWindow = new System.Windows.Forms.TextBox();
			this.tbMessage = new System.Windows.Forms.TextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.tbNickList = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbNick = new System.Windows.Forms.TextBox();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.lbCounter = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server IP:";
			// 
			// tbIP
			// 
			this.tbIP.Location = new System.Drawing.Point(66, 6);
			this.tbIP.Name = "tbIP";
			this.tbIP.Size = new System.Drawing.Size(100, 20);
			this.tbIP.TabIndex = 0;
			// 
			// btnConnect
			// 
			this.btnConnect.Enabled = false;
			this.btnConnect.Location = new System.Drawing.Point(433, 4);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 23);
			this.btnConnect.TabIndex = 3;
			this.btnConnect.Text = "&Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// tbChatWindow
			// 
			this.tbChatWindow.BackColor = System.Drawing.Color.White;
			this.tbChatWindow.Location = new System.Drawing.Point(12, 33);
			this.tbChatWindow.MaxLength = 1000000;
			this.tbChatWindow.Multiline = true;
			this.tbChatWindow.Name = "tbChatWindow";
			this.tbChatWindow.ReadOnly = true;
			this.tbChatWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbChatWindow.Size = new System.Drawing.Size(458, 375);
			this.tbChatWindow.TabIndex = 5;
			// 
			// tbMessage
			// 
			this.tbMessage.Location = new System.Drawing.Point(12, 414);
			this.tbMessage.MaxLength = 512;
			this.tbMessage.Multiline = true;
			this.tbMessage.Name = "tbMessage";
			this.tbMessage.Size = new System.Drawing.Size(443, 87);
			this.tbMessage.TabIndex = 7;
			this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
			//this.tbMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMessage_KeyPress);
			// 
			// btnSend
			// 
			this.btnSend.Enabled = false;
			this.btnSend.Location = new System.Drawing.Point(476, 414);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 8;
			this.btnSend.Text = "&Send";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// tbNickList
			// 
			this.tbNickList.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbNickList.BackColor = System.Drawing.Color.White;
			this.tbNickList.Location = new System.Drawing.Point(476, 32);
			this.tbNickList.MaxLength = 500000;
			this.tbNickList.Multiline = true;
			this.tbNickList.Name = "tbNickList";
			this.tbNickList.ReadOnly = true;
			this.tbNickList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbNickList.Size = new System.Drawing.Size(156, 375);
			this.tbNickList.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(166, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(10, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = ":";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(176, 6);
			this.tbPort.MaxLength = 5;
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(40, 20);
			this.tbPort.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(237, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Nick:";
			// 
			// tbNick
			// 
			this.tbNick.Location = new System.Drawing.Point(270, 6);
			this.tbNick.MaxLength = 12;
			this.tbNick.Name = "tbNick";
			this.tbNick.Size = new System.Drawing.Size(140, 20);
			this.tbNick.TabIndex = 2;
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Enabled = false;
			this.btnDisconnect.Location = new System.Drawing.Point(514, 3);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
			this.btnDisconnect.TabIndex = 4;
			this.btnDisconnect.Text = "&Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// lbCounter
			// 
			this.lbCounter.AutoSize = true;
			this.lbCounter.Location = new System.Drawing.Point(473, 488);
			this.lbCounter.Name = "lbCounter";
			this.lbCounter.Size = new System.Drawing.Size(13, 13);
			this.lbCounter.TabIndex = 12;
			this.lbCounter.Text = "0";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(644, 510);
			this.Controls.Add(this.lbCounter);
			this.Controls.Add(this.btnDisconnect);
			this.Controls.Add(this.tbNick);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbNickList);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.tbMessage);
			this.Controls.Add(this.tbChatWindow);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.tbIP);
			this.Controls.Add(this.label1);
			this.MaximumSize = new System.Drawing.Size(660, 548);
			this.MinimumSize = new System.Drawing.Size(660, 548);
			this.Name = "Form1";
			this.Text = "Chat Window";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbChatWindow;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.TextBox tbNickList;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbNick;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Label lbCounter;
    }
}

