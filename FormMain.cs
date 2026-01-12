using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Project_Petshop
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button_Custom_Click(object sender, EventArgs e)
        {
            FormCustomer my_custom_forn=new FormCustomer();
            my_custom_forn.ShowDialog(this);
        }

        private void button_Employee_Click(object sender, EventArgs e)
        {
            FormSignIn formEmployee=new FormSignIn();
            formEmployee.ShowDialog(this);
        }
    }
}
