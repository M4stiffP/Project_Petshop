namespace Project_Petshop
{
    partial class FormMain
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
            this.button_Custom = new System.Windows.Forms.Button();
            this.button_Employee = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Custom
            // 
            this.button_Custom.Location = new System.Drawing.Point(12, 12);
            this.button_Custom.Name = "button_Custom";
            this.button_Custom.Size = new System.Drawing.Size(304, 34);
            this.button_Custom.TabIndex = 0;
            this.button_Custom.Text = "ระบบลูกค้า";
            this.button_Custom.UseVisualStyleBackColor = true;
            this.button_Custom.Click += new System.EventHandler(this.button_Custom_Click);
            // 
            // button_Employee
            // 
            this.button_Employee.Location = new System.Drawing.Point(12, 52);
            this.button_Employee.Name = "button_Employee";
            this.button_Employee.Size = new System.Drawing.Size(304, 34);
            this.button_Employee.TabIndex = 0;
            this.button_Employee.Text = "ระบบพนักงาน";
            this.button_Employee.UseVisualStyleBackColor = true;
            this.button_Employee.Click += new System.EventHandler(this.button_Employee_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(333, 95);
            this.Controls.Add(this.button_Employee);
            this.Controls.Add(this.button_Custom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormMain";
            this.Text = "ระบบจัดการร้านขายอาหารสัตว์";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Custom;
        private System.Windows.Forms.Button button_Employee;
    }
}

