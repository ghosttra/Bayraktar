namespace GameServerInterface
{
    partial class UsersControl
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
            this.components = new System.ComponentModel.Container();
            this.usersDgv = new System.Windows.Forms.DataGridView();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.loginBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.dbButtons = new GameServerInterface.DbButtons();
            this.loginCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.usersDgv)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // usersDgv
            // 
            this.usersDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usersDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.loginCol,
            this.passwordCol});
            this.usersDgv.Dock = System.Windows.Forms.DockStyle.Top;
            this.usersDgv.Location = new System.Drawing.Point(0, 0);
            this.usersDgv.Name = "usersDgv";
            this.usersDgv.ReadOnly = true;
            this.usersDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.usersDgv.Size = new System.Drawing.Size(776, 291);
            this.usersDgv.TabIndex = 0;
            this.usersDgv.SelectionChanged += new System.EventHandler(this.usersDgv_SelectionChanged);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(68, 38);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(159, 20);
            this.passwordBox.TabIndex = 7;
            // 
            // loginBox
            // 
            this.loginBox.Location = new System.Drawing.Point(68, 13);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(159, 20);
            this.loginBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Login";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.passwordBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.loginBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 329);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 61);
            this.panel1.TabIndex = 8;
            // 
            // dbButtons
            // 
            this.dbButtons.BackColor = System.Drawing.Color.Transparent;
            this.dbButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.dbButtons.Location = new System.Drawing.Point(0, 291);
            this.dbButtons.Name = "dbButtons";
            this.dbButtons.Size = new System.Drawing.Size(776, 38);
            this.dbButtons.TabIndex = 1;
            this.dbButtons.Load += new System.EventHandler(this.dbButtons_Load);
            // 
            // loginCol
            // 
            this.loginCol.DataPropertyName = "Login";
            this.loginCol.HeaderText = "Login";
            this.loginCol.Name = "loginCol";
            this.loginCol.ReadOnly = true;
            // 
            // passwordCol
            // 
            this.passwordCol.DataPropertyName = "Password";
            this.passwordCol.HeaderText = "Password";
            this.passwordCol.Name = "passwordCol";
            this.passwordCol.ReadOnly = true;
            // 
            // UsersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 406);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dbButtons);
            this.Controls.Add(this.usersDgv);
            this.MinimumSize = new System.Drawing.Size(275, 435);
            this.Name = "UsersControl";
            this.Text = "UserControl";
            ((System.ComponentModel.ISupportInitialize)(this.usersDgv)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView usersDgv;
        private DbButtons dbButtons;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox loginBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip tip;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordCol;
    }
}