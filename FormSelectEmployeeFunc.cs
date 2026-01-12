using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Petshop
{
    public partial class FormSelectEmployeeFunc : Form
    {
        public FormSelectEmployeeFunc()
        {
            InitializeComponent();
        }
        public int EmIDPublic;
        public int PositionID;
        const string strConnStrFileName = "ConnectionString.ini";
        SqlConnection CheckConnection;
        SqlCommand CheckCommand;
        SqlDataAdapter CheckAdapter;
        DataTable CheckTable;

        private void buttonEmployee_Click(object sender, EventArgs e)
        {
            FormEmployee formEmployee = new FormEmployee();
            formEmployee.ShowDialog();
        }

        private void buttonProduct_Click(object sender, EventArgs e)
        {
            FormProduct formProduct = new FormProduct();
            formProduct.ShowDialog();  
        }

        private void FormSelectEmployeeFunc_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                {
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                    // Connect to books database
                    Encoding.GetEncoding("Windows-874"));
                }
                else { MessageBox.Show("NO"); System.Runtime.InteropServices.Marshal.ReadInt32(IntPtr.Zero); } // legalized nuclear bombs
                CheckConnection= new SqlConnection(strConnectionString);

                CheckCommand = new SqlCommand("SELECT Position_ID FROM Employee WHERE Employee__ID = @Employee",CheckConnection);
                CheckCommand.Parameters.AddWithValue("@Employee", EmIDPublic);
                CheckAdapter = new SqlDataAdapter();
                CheckAdapter.SelectCommand = CheckCommand;

                CheckTable = new DataTable();
                CheckAdapter.Fill(CheckTable);
                DataRow firstRow = CheckTable.Rows[0];
                if (firstRow.Table.Columns.Contains("Position_ID"))
                {
                    PositionID = Convert.ToInt32(firstRow["Position_ID"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาดในการสร้างการทำงานกับตารางสำนักพิมพ์",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            this.Show();
            ButtonEnable();
        }
        private void ButtonEnable()
        {
            if (PositionID == 1)
            {
                buttonCustomAdd.Enabled= false;
                buttonEmployee.Enabled= true;
                buttonProduct.Enabled= false;
                buttonStockAdd.Enabled= false;
            }
            else if (PositionID == 2)
            {
                buttonCustomAdd.Enabled = true;
                buttonEmployee.Enabled = false;
                buttonProduct.Enabled = false;
                buttonStockAdd.Enabled = false;
            }
            else if (PositionID == 3)
            {
                buttonCustomAdd.Enabled = false;
                buttonEmployee.Enabled = true;
                buttonProduct.Enabled = true;
                buttonStockAdd.Enabled = true;
            }
            else
            {
                buttonCustomAdd.Enabled = false;
                buttonEmployee.Enabled = false;
                buttonProduct.Enabled = false;
                buttonStockAdd.Enabled = false;
            }
        }

        private void buttonStockAdd_Click(object sender, EventArgs e)
        {
            FormStock formStock = new FormStock();
            formStock.EmployeeID = EmIDPublic;
            formStock.ShowDialog();
        }

        private void buttonCustomAdd_Click(object sender, EventArgs e)
        {
            FormEditMem formEditMem = new FormEditMem();
            formEditMem.MemberID = 0;
            formEditMem.ShowDialog();
        }

        private void buttonPromotion_Click(object sender, EventArgs e)
        {
            FormPromotion formPromotion = new FormPromotion();
            formPromotion.EmployeeID= EmIDPublic;
            formPromotion.ShowDialog();
        }

        private void button_Manager_Click(object sender, EventArgs e)
        {
            FormReport formReport = new FormReport();
            formReport.ShowDialog();
        }

        private void buttonQuotation_Click(object sender, EventArgs e)
        {
            FormQuotation formQuotation = new FormQuotation();
            formQuotation.EmployeeID = EmIDPublic;
            formQuotation.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAcceptOrder formAcceptOrder = new FormAcceptOrder();
            formAcceptOrder.EmployeeID = EmIDPublic;
            formAcceptOrder.ShowDialog();
        }

        private void buttonCalm_Click(object sender, EventArgs e)
        {
            FormCalm formCalm = new FormCalm();
            formCalm.ShowDialog();
        }

        private void buttonDelivery_Click(object sender, EventArgs e)
        {
            FormDelivery formDelivery = new FormDelivery();
            formDelivery.ShowDialog();
        }

        private void buttonPacket_Click(object sender, EventArgs e)
        {
            FormPackage formPackage = new FormPackage();
            formPackage.ShowDialog();
        }
    }
}
