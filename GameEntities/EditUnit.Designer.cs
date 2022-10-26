namespace GameEntities
{
    partial class UnitsEdit
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.normalPic = new System.Windows.Forms.PictureBox();
            this.destroyedPic = new System.Windows.Forms.PictureBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.normalPicBtn = new System.Windows.Forms.Button();
            this.destroyedPicBtn = new System.Windows.Forms.Button();
            this.priceNum = new System.Windows.Forms.NumericUpDown();
            this.coolDownNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.addBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.updBtn = new System.Windows.Forms.Button();
            this.delBtn = new System.Windows.Forms.Button();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.unitsDGV = new System.Windows.Forms.DataGridView();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoolDownCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.normalPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.destroyedPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coolDownNum)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unitsDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // normalPic
            // 
            this.normalPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.normalPic.Location = new System.Drawing.Point(406, 2);
            this.normalPic.Name = "normalPic";
            this.normalPic.Size = new System.Drawing.Size(200, 200);
            this.normalPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.normalPic.TabIndex = 1;
            this.normalPic.TabStop = false;
            // 
            // destroyedPic
            // 
            this.destroyedPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.destroyedPic.Location = new System.Drawing.Point(628, 2);
            this.destroyedPic.Name = "destroyedPic";
            this.destroyedPic.Size = new System.Drawing.Size(200, 200);
            this.destroyedPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.destroyedPic.TabIndex = 2;
            this.destroyedPic.TabStop = false;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(67, 340);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(120, 20);
            this.nameBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 343);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name";
            // 
            // normalPicBtn
            // 
            this.normalPicBtn.Location = new System.Drawing.Point(406, 208);
            this.normalPicBtn.Name = "normalPicBtn";
            this.normalPicBtn.Size = new System.Drawing.Size(108, 23);
            this.normalPicBtn.TabIndex = 5;
            this.normalPicBtn.Text = "Normal Pic";
            this.normalPicBtn.UseVisualStyleBackColor = true;
            this.normalPicBtn.Click += new System.EventHandler(this.normalPicBtn_Click);
            // 
            // destroyedPicBtn
            // 
            this.destroyedPicBtn.Location = new System.Drawing.Point(628, 208);
            this.destroyedPicBtn.Name = "destroyedPicBtn";
            this.destroyedPicBtn.Size = new System.Drawing.Size(108, 23);
            this.destroyedPicBtn.TabIndex = 6;
            this.destroyedPicBtn.Text = "Destroyed Pic";
            this.destroyedPicBtn.UseVisualStyleBackColor = true;
            this.destroyedPicBtn.Click += new System.EventHandler(this.destroyedPicBtn_Click);
            // 
            // priceNum
            // 
            this.priceNum.Location = new System.Drawing.Point(67, 366);
            this.priceNum.Name = "priceNum";
            this.priceNum.Size = new System.Drawing.Size(120, 20);
            this.priceNum.TabIndex = 7;
            // 
            // coolDownNum
            // 
            this.coolDownNum.Location = new System.Drawing.Point(67, 392);
            this.coolDownNum.Name = "coolDownNum";
            this.coolDownNum.Size = new System.Drawing.Size(120, 20);
            this.coolDownNum.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 368);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Price";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 394);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Cooldown";
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(3, 3);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 11;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.addBtn);
            this.flowLayoutPanel1.Controls.Add(this.updBtn);
            this.flowLayoutPanel1.Controls.Add(this.delBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 418);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(243, 29);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // updBtn
            // 
            this.updBtn.Location = new System.Drawing.Point(84, 3);
            this.updBtn.Name = "updBtn";
            this.updBtn.Size = new System.Drawing.Size(75, 23);
            this.updBtn.TabIndex = 12;
            this.updBtn.Text = "Upd";
            this.updBtn.UseVisualStyleBackColor = true;
            this.updBtn.Click += new System.EventHandler(this.updBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Location = new System.Drawing.Point(165, 3);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(75, 23);
            this.delBtn.TabIndex = 13;
            this.delBtn.Text = "Del";
            this.delBtn.UseVisualStyleBackColor = true;
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // unitsDGV
            // 
            this.unitsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unitsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameCol,
            this.PriceCol,
            this.CoolDownCol});
            this.unitsDGV.Location = new System.Drawing.Point(12, 2);
            this.unitsDGV.Name = "unitsDGV";
            this.unitsDGV.ReadOnly = true;
            this.unitsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.unitsDGV.Size = new System.Drawing.Size(388, 328);
            this.unitsDGV.TabIndex = 13;
            this.unitsDGV.SelectionChanged += new System.EventHandler(this.unitsDGV_SelectionChanged);
            // 
            // NameCol
            // 
            this.NameCol.DataPropertyName = "Name";
            this.NameCol.HeaderText = "Name";
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            // 
            // PriceCol
            // 
            this.PriceCol.DataPropertyName = "Price";
            this.PriceCol.HeaderText = "Price";
            this.PriceCol.Name = "PriceCol";
            this.PriceCol.ReadOnly = true;
            // 
            // CoolDownCol
            // 
            this.CoolDownCol.DataPropertyName = "Cooldown";
            this.CoolDownCol.HeaderText = "CoolDown";
            this.CoolDownCol.Name = "CoolDownCol";
            this.CoolDownCol.ReadOnly = true;
            // 
            // UnitsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 455);
            this.Controls.Add(this.unitsDGV);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.coolDownNum);
            this.Controls.Add(this.priceNum);
            this.Controls.Add(this.destroyedPicBtn);
            this.Controls.Add(this.normalPicBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.destroyedPic);
            this.Controls.Add(this.normalPic);
            this.Name = "UnitsEdit";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.normalPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.destroyedPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coolDownNum)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.unitsDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox normalPic;
        private System.Windows.Forms.PictureBox destroyedPic;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button normalPicBtn;
        private System.Windows.Forms.Button destroyedPicBtn;
        private System.Windows.Forms.NumericUpDown priceNum;
        private System.Windows.Forms.NumericUpDown coolDownNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button updBtn;
        private System.Windows.Forms.Button delBtn;
        private System.Windows.Forms.ToolTip tip;
        private System.Windows.Forms.DataGridView unitsDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoolDownCol;
    }
}

