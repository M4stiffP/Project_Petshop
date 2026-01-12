using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Project_Petshop
{
    public partial class FormEmployee : Form
    {
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        private CurrencyManager currencyManager;

        private string mystate;
        private int positionmark;

        public FormEmployee()
        {
            InitializeComponent();
        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {
            string strConnectionString = "";
            if (File.Exists(strConnStrFileName))
                strConnectionString = File.ReadAllText(strConnStrFileName,
                                                Encoding.GetEncoding("Windows-874"));
            connection = new SqlConnection(strConnectionString);
            connection.Open();
            command = new SqlCommand("SELECT * FROM Employee ORDER BY Employee__ID", connection);
            adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            textEmployeeID.DataBindings.Add("Text", DataTable, "Employee__ID");
            textEmployeeFName.DataBindings.Add("Text", DataTable, "Employee_FName");
            textEmployeeSName.DataBindings.Add("Text", DataTable, "Employee_LName");
            textEmployeePNum.DataBindings.Add("Text", DataTable, "Employee_PhoneNum");
            textEmployeeBirthDay.DataBindings.Add("Text", DataTable, "Employee_BirthDate");
            textEmployeeAddress.DataBindings.Add("Text", DataTable, "Employee_Address");
            textBoxPositionID.DataBindings.Add("Text", DataTable, "Position_ID");
            textBoxPassword.DataBindings.Add("Text", DataTable, "Employee_Password");
            currencyManager = (CurrencyManager)this.BindingContext[DataTable];
            setstate("View");
        }
        private void setstate(string appstate)
        {
            mystate = appstate;
            if (appstate == "View")
            {
                textEmployeeID.ReadOnly = true;
                textEmployeeFName.ReadOnly = true;
                textEmployeeSName.ReadOnly = true;
                textEmployeePNum.ReadOnly = true;
                textEmployeeBirthDay.ReadOnly = true;
                textEmployeeAddress.ReadOnly = true;
                textBoxPassword.ReadOnly = true;
                textBoxPositionID.ReadOnly = true;
                buttonFirst.Enabled = true;
                buttonPrevious.Enabled = true;
                buttonNext.Enabled = true;
                buttonLast.Enabled = true;
                buttonEdit.Enabled = true;
                buttonSave.Enabled = false;
                buttonCancel.Enabled = false;
                buttonAddNew.Enabled = true;
                buttonDelete.Enabled = true;
                buttonDone.Enabled = true;
            }
            else
            {
                textEmployeeID.ReadOnly = false;
                textEmployeeFName.ReadOnly = false;
                textEmployeeSName.ReadOnly = false;
                textEmployeePNum.ReadOnly = false;
                textEmployeeBirthDay.ReadOnly = false;
                textEmployeeAddress.ReadOnly = false;
                textBoxPositionID.ReadOnly = false;
                textBoxPassword.ReadOnly = false;
                buttonFirst.Enabled = false;
                buttonPrevious.Enabled = false;
                buttonNext.Enabled = false;
                buttonLast.Enabled = false;
                buttonEdit.Enabled = false;
                buttonSave.Enabled = true;
                buttonCancel.Enabled = true;
                buttonAddNew.Enabled = false;
                buttonDelete.Enabled = false;
                buttonDone.Enabled = false;
            }
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            currencyManager.Position = 0;
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (currencyManager.Position == 0)
                return;
            --currencyManager.Position;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (currencyManager.Position == currencyManager.Count - 1)
                return;
            ++currencyManager.Position;
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            currencyManager.Position = currencyManager.Count - 1;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            setstate("Edit");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string text = textEmployeeID.Text;
                currencyManager.EndCurrentEdit();
                DataTable.DefaultView.Sort = "Employee__ID";
                currencyManager.Position = DataTable.DefaultView.Find(text);
                int num = (int)MessageBox.Show("ข้อมูลถูกบันทึกแล้ว", "บันทึก", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                setstate("View");
            }
            catch (Exception)
            {
                int num = (int)MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            currencyManager.CancelCurrentEdit();
            if (mystate == "Add")
            {
                currencyManager.Position = positionmark;
            }
            setstate("View");
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                positionmark = currencyManager.Position;
                currencyManager.AddNew();
                setstate("Add");
            }
            catch (Exception)
            {
                int num = (int)MessageBox.Show("เกิดข้อผิดพลาดในการเพิ่มข้อมูล", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้", "ลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;
            try
            {
                currencyManager.RemoveAt(currencyManager.Position);
            }
            catch (Exception)
            {
                int num = (int)MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mystate.Equals("Edit") || mystate.Equals("Add"))
            {
                int num = (int)MessageBox.Show("คุณต้องแก้ไขข้อมูลปัจจุบันให้เสร็จก่อนที่จะหยุดโปรแกรม", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                e.Cancel = true;
            }
            else
            {
                try
                {
                    SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(DataTable);
                }
                catch (Exception ex)
                {
                    int num = (int)MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกฐานข้อมูล: \r\n" + ex.Message, "ข้อผิดพลาดในการบันทึก", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                connection.Close();
                connection.Dispose();
                command.Dispose();
                adapter.Dispose();
                DataTable.Dispose();
            }
        }
    }
}
