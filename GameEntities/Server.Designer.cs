namespace GameEntities
{
    partial class Server
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
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.editorBtn);
            this.Name = "Server";
            this.Text = "Server";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button editorBtn;
    }
}