namespace Project_Petshop
{
    partial class FormPromotion
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
            this.textDiscount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textEnddate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonLast = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAddNew = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.textStartdate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textPromotionName = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.textPromotionID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textDiscount
            // 
            this.textDiscount.BackColor = System.Drawing.Color.White;
            this.textDiscount.Location = new System.Drawing.Point(220, 221);
            this.textDiscount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textDiscount.Name = "textDiscount";
            this.textDiscount.ReadOnly = true;
            this.textDiscount.Size = new System.Drawing.Size(250, 26);
            this.textDiscount.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 49;
            this.label2.Text = "Discount";
            // 
            // textEnddate
            // 
            this.textEnddate.BackColor = System.Drawing.Color.White;
            this.textEnddate.Location = new System.Drawing.Point(220, 178);
            this.textEnddate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textEnddate.Name = "textEnddate";
            this.textEnddate.ReadOnly = true;
            this.textEnddate.Size = new System.Drawing.Size(250, 26);
            this.textEnddate.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 50;
            this.label4.Text = "Enddate";
            // 
            // buttonLast
            // 
            this.buttonLast.Location = new System.Drawing.Point(421, 280);
            this.buttonLast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(112, 46);
            this.buttonLast.TabIndex = 42;
            this.buttonLast.TabStop = false;
            this.buttonLast.Text = ">>";
            this.buttonLast.UseVisualStyleBackColor = true;
            this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(300, 280);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(112, 46);
            this.buttonNext.TabIndex = 41;
            this.buttonNext.TabStop = false;
            this.buttonNext.Text = "=>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(179, 280);
            this.buttonPrevious.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(112, 46);
            this.buttonPrevious.TabIndex = 40;
            this.buttonPrevious.TabStop = false;
            this.buttonPrevious.Text = "<=";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(421, 412);
            this.buttonDone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(112, 46);
            this.buttonDone.TabIndex = 48;
            this.buttonDone.TabStop = false;
            this.buttonDone.Text = "เ&สร็จสิ้น";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(240, 412);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(112, 46);
            this.buttonDelete.TabIndex = 47;
            this.buttonDelete.TabStop = false;
            this.buttonDelete.Text = "&ลบออก";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(420, 357);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 46);
            this.buttonCancel.TabIndex = 45;
            this.buttonCancel.TabStop = false;
            this.buttonCancel.Text = "&ยกเลิก";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonAddNew
            // 
            this.buttonAddNew.Location = new System.Drawing.Point(59, 412);
            this.buttonAddNew.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAddNew.Name = "buttonAddNew";
            this.buttonAddNew.Size = new System.Drawing.Size(112, 46);
            this.buttonAddNew.TabIndex = 46;
            this.buttonAddNew.TabStop = false;
            this.buttonAddNew.Text = "เ&พิ่มใหม่";
            this.buttonAddNew.UseVisualStyleBackColor = true;
            this.buttonAddNew.Click += new System.EventHandler(this.buttonAddNew_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(239, 357);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(112, 46);
            this.buttonSave.TabIndex = 44;
            this.buttonSave.TabStop = false;
            this.buttonSave.Text = "&บันทึก";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(57, 357);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(112, 46);
            this.buttonEdit.TabIndex = 43;
            this.buttonEdit.TabStop = false;
            this.buttonEdit.Text = "แ&ก้ไข";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonFirst
            // 
            this.buttonFirst.Location = new System.Drawing.Point(57, 280);
            this.buttonFirst.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(112, 46);
            this.buttonFirst.TabIndex = 39;
            this.buttonFirst.TabStop = false;
            this.buttonFirst.Text = "<<";
            this.buttonFirst.UseVisualStyleBackColor = true;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // textStartdate
            // 
            this.textStartdate.BackColor = System.Drawing.Color.White;
            this.textStartdate.Location = new System.Drawing.Point(218, 135);
            this.textStartdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textStartdate.Name = "textStartdate";
            this.textStartdate.ReadOnly = true;
            this.textStartdate.Size = new System.Drawing.Size(252, 26);
            this.textStartdate.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 33;
            this.label3.Text = "Startdate";
            // 
            // textPromotionName
            // 
            this.textPromotionName.BackColor = System.Drawing.Color.White;
            this.textPromotionName.Location = new System.Drawing.Point(218, 86);
            this.textPromotionName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textPromotionName.Name = "textPromotionName";
            this.textPromotionName.ReadOnly = true;
            this.textPromotionName.Size = new System.Drawing.Size(252, 26);
            this.textPromotionName.TabIndex = 37;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(93, 89);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(123, 20);
            this.label30.TabIndex = 34;
            this.label30.Text = "PromotionName";
            // 
            // textPromotionID
            // 
            this.textPromotionID.BackColor = System.Drawing.Color.White;
            this.textPromotionID.Location = new System.Drawing.Point(218, 37);
            this.textPromotionID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textPromotionID.Name = "textPromotionID";
            this.textPromotionID.ReadOnly = true;
            this.textPromotionID.Size = new System.Drawing.Size(252, 26);
            this.textPromotionID.TabIndex = 35;
            this.textPromotionID.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "PromotionID";
            // 
            // FormPromotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 507);
            this.Controls.Add(this.textDiscount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textEnddate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonLast);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAddNew);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonFirst);
            this.Controls.Add(this.textStartdate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textPromotionName);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.textPromotionID);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormPromotion";
            this.Text = "FormPromotion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmployee_FormClosing);
            this.Load += new System.EventHandler(this.FormPromotion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textDiscount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textEnddate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonLast;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAddNew;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonFirst;
        private System.Windows.Forms.TextBox textStartdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textPromotionName;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox textPromotionID;
        private System.Windows.Forms.Label label1;
    }
}