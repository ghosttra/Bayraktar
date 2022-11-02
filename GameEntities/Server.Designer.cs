namespace GameEntities
{
    partial class ServerInterface
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
            this.editorBtn = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.ListBox();
            this.stopBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.configPanel = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.serverIpBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.userControlBtn = new System.Windows.Forms.Button();
            this.configPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portBox)).BeginInit();
            this.SuspendLayout();
            // 
            // editorBtn
            // 
            this.editorBtn.Location = new System.Drawing.Point(12, 12);
            this.editorBtn.Name = "editorBtn";
            this.editorBtn.Size = new System.Drawing.Size(96, 23);
            this.editorBtn.TabIndex = 0;
            this.editorBtn.Text = "Unit Editor";
            this.editorBtn.UseVisualStyleBackColor = true;
            this.editorBtn.Click += new System.EventHandler(this.editorBtn_Click);
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.FormattingEnabled = true;
            this.logBox.Location = new System.Drawing.Point(0, 145);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(434, 316);
            this.logBox.TabIndex = 2;
            // 
            // stopBtn
            // 
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(323, 93);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(93, 23);
            this.stopBtn.TabIndex = 13;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(323, 57);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(93, 23);
            this.startBtn.TabIndex = 12;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // configPanel
            // 
            this.configPanel.Controls.Add(this.label4);
            this.configPanel.Controls.Add(this.serverIpBox);
            this.configPanel.Controls.Add(this.portBox);
            this.configPanel.Controls.Add(this.label1);
            this.configPanel.Location = new System.Drawing.Point(12, 52);
            this.configPanel.Name = "configPanel";
            this.configPanel.Size = new System.Drawing.Size(305, 64);
            this.configPanel.TabIndex = 11;
            this.configPanel.TabStop = false;
            this.configPanel.Text = "Configuration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "IP";
            // 
            // serverIpBox
            // 
            this.serverIpBox.Location = new System.Drawing.Point(158, 24);
            this.serverIpBox.Name = "serverIpBox";
            this.serverIpBox.Size = new System.Drawing.Size(115, 20);
            this.serverIpBox.TabIndex = 2;
            this.serverIpBox.Text = "127.0.0.1";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(48, 24);
            this.portBox.Maximum = new decimal(new int[] {
            655535,
            0,
            0,
            0});
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(62, 20);
            this.portBox.TabIndex = 1;
            this.portBox.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // userControlBtn
            // 
            this.userControlBtn.Location = new System.Drawing.Point(114, 12);
            this.userControlBtn.Name = "userControlBtn";
            this.userControlBtn.Size = new System.Drawing.Size(96, 23);
            this.userControlBtn.TabIndex = 14;
            this.userControlBtn.Text = "User Control";
            this.userControlBtn.UseVisualStyleBackColor = true;
            this.userControlBtn.Click += new System.EventHandler(this.userControlBtn_Click);
            // 
            // ServerInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.userControlBtn);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.configPanel);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.editorBtn);
            this.MinimumSize = new System.Drawing.Size(450, 500);
            this.Name = "ServerInterface";
            this.Text = "Server";
            this.configPanel.ResumeLayout(false);
            this.configPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button editorBtn;
        private System.Windows.Forms.ListBox logBox;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.GroupBox configPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox serverIpBox;
        private System.Windows.Forms.NumericUpDown portBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button userControlBtn;
    }
}