namespace Project_Petshop
{
    partial class FormReport
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
            this.buttonSaleDay = new System.Windows.Forms.Button();
            this.dataGridViewReport = new System.Windows.Forms.DataGridView();
            this.buttonSaleMonth = new System.Windows.Forms.Button();
            this.buttonSaleYear = new System.Windows.Forms.Button();
            this.buttonTotalDay = new System.Windows.Forms.Button();
            this.buttonTotalMonth = new System.Windows.Forms.Button();
            this.buttonTotalYear = new System.Windows.Forms.Button();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.buttonPDF = new System.Windows.Forms.Button();
            this.buttonStocklife = new System.Windows.Forms.Button();
            this.YearPurchase = new System.Windows.Forms.Button();
            this.DayPurchase = new System.Windows.Forms.Button();
            this.MonthPurchase = new System.Windows.Forms.Button();
            this.dateTimePickerDaySelect = new System.Windows.Forms.DateTimePicker();
            this.buttonMember = new System.Windows.Forms.Button();
            this.buttonMostMember = new System.Windows.Forms.Button();
            this.buttonEmployee = new System.Windows.Forms.Button();
            this.buttonBestSeller = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "วันที่เลือก";
            // 
            // buttonSaleDay
            // 
            this.buttonSaleDay.Location = new System.Drawing.Point(520, 342);
            this.buttonSaleDay.Name = "buttonSaleDay";
            this.buttonSaleDay.Size = new System.Drawing.Size(149, 37);
            this.buttonSaleDay.TabIndex = 2;
            this.buttonSaleDay.Text = "สรุปยอดขายรายวัน";
            this.buttonSaleDay.UseVisualStyleBackColor = true;
            this.buttonSaleDay.Click += new System.EventHandler(this.buttonSaleDay_Click);
            // 
            // dataGridViewReport
            // 
            this.dataGridViewReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReport.Location = new System.Drawing.Point(12, 130);
            this.dataGridViewReport.Name = "dataGridViewReport";
            this.dataGridViewReport.Size = new System.Drawing.Size(497, 469);
            this.dataGridViewReport.TabIndex = 3;
            // 
            // buttonSaleMonth
            // 
            this.buttonSaleMonth.Location = new System.Drawing.Point(675, 342);
            this.buttonSaleMonth.Name = "buttonSaleMonth";
            this.buttonSaleMonth.Size = new System.Drawing.Size(149, 37);
            this.buttonSaleMonth.TabIndex = 2;
            this.buttonSaleMonth.Text = "สรุปยอดขายรายเดือน";
            this.buttonSaleMonth.UseVisualStyleBackColor = true;
            this.buttonSaleMonth.Click += new System.EventHandler(this.buttonSaleMonth_Click);
            // 
            // buttonSaleYear
            // 
            this.buttonSaleYear.Location = new System.Drawing.Point(675, 385);
            this.buttonSaleYear.Name = "buttonSaleYear";
            this.buttonSaleYear.Size = new System.Drawing.Size(149, 37);
            this.buttonSaleYear.TabIndex = 2;
            this.buttonSaleYear.Text = "สรุปยอดขายรายปี";
            this.buttonSaleYear.UseVisualStyleBackColor = true;
            this.buttonSaleYear.Click += new System.EventHandler(this.buttonSaleYear_Click);
            // 
            // buttonTotalDay
            // 
            this.buttonTotalDay.Location = new System.Drawing.Point(520, 385);
            this.buttonTotalDay.Name = "buttonTotalDay";
            this.buttonTotalDay.Size = new System.Drawing.Size(149, 37);
            this.buttonTotalDay.TabIndex = 2;
            this.buttonTotalDay.Text = "สรุปยอดกำไรรายวัน";
            this.buttonTotalDay.UseVisualStyleBackColor = true;
            this.buttonTotalDay.Click += new System.EventHandler(this.buttonTotalDay_Click);
            // 
            // buttonTotalMonth
            // 
            this.buttonTotalMonth.Location = new System.Drawing.Point(520, 428);
            this.buttonTotalMonth.Name = "buttonTotalMonth";
            this.buttonTotalMonth.Size = new System.Drawing.Size(149, 37);
            this.buttonTotalMonth.TabIndex = 2;
            this.buttonTotalMonth.Text = "สรุปกำไรรายเดือน";
            this.buttonTotalMonth.UseVisualStyleBackColor = true;
            this.buttonTotalMonth.Click += new System.EventHandler(this.buttonTotalMonth_Click);
            // 
            // buttonTotalYear
            // 
            this.buttonTotalYear.Location = new System.Drawing.Point(675, 428);
            this.buttonTotalYear.Name = "buttonTotalYear";
            this.buttonTotalYear.Size = new System.Drawing.Size(149, 37);
            this.buttonTotalYear.TabIndex = 2;
            this.buttonTotalYear.Text = "สรุปยอดกำไรรายปี";
            this.buttonTotalYear.UseVisualStyleBackColor = true;
            this.buttonTotalYear.Click += new System.EventHandler(this.buttonTotalYear_Click);
            // 
            // buttonExcel
            // 
            this.buttonExcel.Location = new System.Drawing.Point(520, 557);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(154, 42);
            this.buttonExcel.TabIndex = 4;
            this.buttonExcel.Text = "Excel";
            this.buttonExcel.UseVisualStyleBackColor = true;
            this.buttonExcel.Click += new System.EventHandler(this.buttonExcel_Click);
            // 
            // buttonPDF
            // 
            this.buttonPDF.Location = new System.Drawing.Point(680, 557);
            this.buttonPDF.Name = "buttonPDF";
            this.buttonPDF.Size = new System.Drawing.Size(154, 42);
            this.buttonPDF.TabIndex = 4;
            this.buttonPDF.Text = "PDF";
            this.buttonPDF.UseVisualStyleBackColor = true;
            this.buttonPDF.Click += new System.EventHandler(this.buttonPDF_Click);
            // 
            // buttonStocklife
            // 
            this.buttonStocklife.Location = new System.Drawing.Point(520, 471);
            this.buttonStocklife.Name = "buttonStocklife";
            this.buttonStocklife.Size = new System.Drawing.Size(149, 37);
            this.buttonStocklife.TabIndex = 5;
            this.buttonStocklife.Text = "สินค้าคงคลัง";
            this.buttonStocklife.UseVisualStyleBackColor = true;
            this.buttonStocklife.Click += new System.EventHandler(this.buttonStocklife_Click);
            // 
            // YearPurchase
            // 
            this.YearPurchase.Location = new System.Drawing.Point(520, 514);
            this.YearPurchase.Name = "YearPurchase";
            this.YearPurchase.Size = new System.Drawing.Size(149, 37);
            this.YearPurchase.TabIndex = 5;
            this.YearPurchase.Text = "ยอดซื้อรายปี";
            this.YearPurchase.UseVisualStyleBackColor = true;
            this.YearPurchase.Click += new System.EventHandler(this.YearPurchase_Click);
            // 
            // DayPurchase
            // 
            this.DayPurchase.Location = new System.Drawing.Point(675, 471);
            this.DayPurchase.Name = "DayPurchase";
            this.DayPurchase.Size = new System.Drawing.Size(149, 37);
            this.DayPurchase.TabIndex = 5;
            this.DayPurchase.Text = "ยอดซื้อรายวัน";
            this.DayPurchase.UseVisualStyleBackColor = true;
            this.DayPurchase.Click += new System.EventHandler(this.DayPurchase_Click);
            // 
            // MonthPurchase
            // 
            this.MonthPurchase.Location = new System.Drawing.Point(675, 514);
            this.MonthPurchase.Name = "MonthPurchase";
            this.MonthPurchase.Size = new System.Drawing.Size(149, 37);
            this.MonthPurchase.TabIndex = 5;
            this.MonthPurchase.Text = "ยอดซื้อรายเดือน";
            this.MonthPurchase.UseVisualStyleBackColor = true;
            this.MonthPurchase.Click += new System.EventHandler(this.MonthPurchase_Click);
            // 
            // dateTimePickerDaySelect
            // 
            this.dateTimePickerDaySelect.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDaySelect.Location = new System.Drawing.Point(97, 10);
            this.dateTimePickerDaySelect.Name = "dateTimePickerDaySelect";
            this.dateTimePickerDaySelect.Size = new System.Drawing.Size(246, 26);
            this.dateTimePickerDaySelect.TabIndex = 6;
            // 
            // buttonMember
            // 
            this.buttonMember.Location = new System.Drawing.Point(520, 299);
            this.buttonMember.Name = "buttonMember";
            this.buttonMember.Size = new System.Drawing.Size(149, 37);
            this.buttonMember.TabIndex = 2;
            this.buttonMember.Text = "รายชื่อสมาชิก";
            this.buttonMember.UseVisualStyleBackColor = true;
            this.buttonMember.Click += new System.EventHandler(this.buttonMember_Click);
            // 
            // buttonMostMember
            // 
            this.buttonMostMember.Location = new System.Drawing.Point(675, 299);
            this.buttonMostMember.Name = "buttonMostMember";
            this.buttonMostMember.Size = new System.Drawing.Size(149, 37);
            this.buttonMostMember.TabIndex = 2;
            this.buttonMostMember.Text = "ซื้อของมากที่สุด";
            this.buttonMostMember.UseVisualStyleBackColor = true;
            this.buttonMostMember.Click += new System.EventHandler(this.buttonMostMember_Click);
            // 
            // buttonEmployee
            // 
            this.buttonEmployee.Location = new System.Drawing.Point(520, 256);
            this.buttonEmployee.Name = "buttonEmployee";
            this.buttonEmployee.Size = new System.Drawing.Size(149, 37);
            this.buttonEmployee.TabIndex = 2;
            this.buttonEmployee.Text = "รายชื่อพนักงาน";
            this.buttonEmployee.UseVisualStyleBackColor = true;
            this.buttonEmployee.Click += new System.EventHandler(this.buttonEmployee_Click);
            // 
            // buttonBestSeller
            // 
            this.buttonBestSeller.Location = new System.Drawing.Point(675, 256);
            this.buttonBestSeller.Name = "buttonBestSeller";
            this.buttonBestSeller.Size = new System.Drawing.Size(149, 37);
            this.buttonBestSeller.TabIndex = 2;
            this.buttonBestSeller.Text = "สินค้าขายดี";
            this.buttonBestSeller.UseVisualStyleBackColor = true;
            this.buttonBestSeller.Click += new System.EventHandler(this.buttonBestSeller_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(520, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 37);
            this.button1.TabIndex = 2;
            this.button1.Text = "รายชื่อโปรโมชั่น";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 611);
            this.Controls.Add(this.dateTimePickerDaySelect);
            this.Controls.Add(this.MonthPurchase);
            this.Controls.Add(this.DayPurchase);
            this.Controls.Add(this.YearPurchase);
            this.Controls.Add(this.buttonStocklife);
            this.Controls.Add(this.buttonPDF);
            this.Controls.Add(this.buttonExcel);
            this.Controls.Add(this.dataGridViewReport);
            this.Controls.Add(this.buttonTotalYear);
            this.Controls.Add(this.buttonSaleYear);
            this.Controls.Add(this.buttonTotalMonth);
            this.Controls.Add(this.buttonSaleMonth);
            this.Controls.Add(this.buttonTotalDay);
            this.Controls.Add(this.buttonBestSeller);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonEmployee);
            this.Controls.Add(this.buttonMostMember);
            this.Controls.Add(this.buttonMember);
            this.Controls.Add(this.buttonSaleDay);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormReport";
            this.Text = "FormReport";
            this.Load += new System.EventHandler(this.FormReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSaleDay;
        private System.Windows.Forms.DataGridView dataGridViewReport;
        private System.Windows.Forms.Button buttonSaleMonth;
        private System.Windows.Forms.Button buttonSaleYear;
        private System.Windows.Forms.Button buttonTotalDay;
        private System.Windows.Forms.Button buttonTotalMonth;
        private System.Windows.Forms.Button buttonTotalYear;
        private System.Windows.Forms.Button buttonExcel;
        private System.Windows.Forms.Button buttonPDF;
        private System.Windows.Forms.Button buttonStocklife;
        private System.Windows.Forms.Button YearPurchase;
        private System.Windows.Forms.Button DayPurchase;
        private System.Windows.Forms.Button MonthPurchase;
        private System.Windows.Forms.DateTimePicker dateTimePickerDaySelect;
        private System.Windows.Forms.Button buttonMember;
        private System.Windows.Forms.Button buttonMostMember;
        private System.Windows.Forms.Button buttonEmployee;
        private System.Windows.Forms.Button buttonBestSeller;
        private System.Windows.Forms.Button button1;
    }
}