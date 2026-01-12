namespace Project_Petshop
{
    partial class FormStock
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
            this.comboboxProductName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textAddStock = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEnter = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboboxProductName
            // 
            this.comboboxProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboboxProductName.FormattingEnabled = true;
            this.comboboxProductName.Location = new System.Drawing.Point(137, 14);
            this.comboboxProductName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboboxProductName.Name = "comboboxProductName";
            this.comboboxProductName.Size = new System.Drawing.Size(348, 28);
            this.comboboxProductName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "ProductName";
            // 
            // textAddStock
            // 
            this.textAddStock.Location = new System.Drawing.Point(137, 50);
            this.textAddStock.Name = "textAddStock";
            this.textAddStock.Size = new System.Drawing.Size(348, 26);
            this.textAddStock.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "จำนวนสินค้าที่เพิ่ม";
            // 
            // buttonEnter
            // 
            this.buttonEnter.Location = new System.Drawing.Point(505, 4);
            this.buttonEnter.Name = "buttonEnter";
            this.buttonEnter.Size = new System.Drawing.Size(111, 47);
            this.buttonEnter.TabIndex = 3;
            this.buttonEnter.Text = "บันทึก";
            this.buttonEnter.UseVisualStyleBackColor = true;
            this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(505, 57);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(111, 47);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "เสร็จสิ้น";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 116);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonEnter);
            this.Controls.Add(this.textAddStock);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboboxProductName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormStock";
            this.Text = "FormStock";
            this.Load += new System.EventHandler(this.FormStock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboboxProductName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textAddStock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonEnter;
        private System.Windows.Forms.Button buttonClose;
    }
}