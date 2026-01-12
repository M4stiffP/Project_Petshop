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
using static System.Windows.Forms.AxHost;
using System.IO;

namespace Project_Petshop
{
    public partial class FormQuotation : Form
    {
        public FormQuotation()
        {
            InitializeComponent();
        }
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        private DataRowView currentRowView;
        private CurrencyManager currencyManager;
        private string myState;
        private int myBookmark;
        public int EmployeeID;
        private void FormQuotation_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));
                connection = new SqlConnection(strConnectionString);

                connection.Open();

                command = new SqlCommand("SELECT * FROM Quotation_Data", connection);

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable = new DataTable();
                adapter.Fill(DataTable);

                textBoxProductID.DataBindings.Add("Text", DataTable, "ProductID");
                textBoxDis.DataBindings.Add("Text", DataTable, "Quotation_Discirpt");
                textBoxQuantity.DataBindings.Add("Text", DataTable, "Quotation_Quantity");
                textBoxID.DataBindings.Add("Text", DataTable, "QuotationID");
                textBoxSubtotal.DataBindings.Add("Text", DataTable, "QuotationSubtotal");
                currencyManager = (CurrencyManager)this.BindingContext[DataTable];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            this.Show();
            SetState("View");
        }
        private void SetState(string appState = "View")
        /* Only set to other value than view to edit */
        {
            myState = appState;
            switch (appState)
            {
                case "View":
                    textBoxProductID.ReadOnly = true; // key
                    textBoxDis.ReadOnly = true;
                    textBoxQuantity.ReadOnly = true;
                    textBoxSubtotal.ReadOnly = true;
                    buttonFirst.Enabled = true;
                    buttonPrevious.Enabled = true;
                    buttonNext.Enabled = true;
                    buttonLast.Enabled = true;
                    buttonAddNew.Enabled = true;
                    buttonSave.Enabled = false;
                    buttonCancel.Enabled = false;
                    buttonEdit.Enabled = true;
                    buttonDelete.Enabled = true;
                    buttonDone.Enabled = true;
                    
                    break;
                default: // other than view
                    textBoxProductID.ReadOnly = false; // key
                    textBoxDis.ReadOnly = false;
                    buttonFirst.Enabled = false;
                    buttonPrevious.Enabled = false;
                    buttonNext.Enabled = false;
                    buttonLast.Enabled = false;
                    buttonAddNew.Enabled = false;
                    buttonSave.Enabled = true;
                    buttonCancel.Enabled = true;
                    buttonEdit.Enabled = false;
                    buttonDelete.Enabled = false;
                    buttonDone.Enabled = false;
                    textBoxQuantity.ReadOnly = false;
                    textBoxSubtotal.ReadOnly = false;
                    break;
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
            SetState("Edit");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow currentRow = ((DataRowView)currencyManager.Current).Row;
                if (currentRow.RowState == DataRowState.Added || myState == "Edit")
                {
                    if (DataTable.Columns.Contains("Employee__ID"))
                    {
                        currentRow["Employee__ID"] = EmployeeID;
                    }
                    else
                    {
                        MessageBox.Show("ไม่พบคอลัมน์ Employee_ID ใน DataTable", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                string text = textBoxID.Text;
                currencyManager.EndCurrentEdit();
                DataTable.DefaultView.Sort = "QuotationID";
                currencyManager.Position = DataTable.DefaultView.Find(text);
                int num = (int)MessageBox.Show("ข้อมูลถูกบันทึกแล้ว", "บันทึก", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SetState("View");
            }
            catch (Exception)
            {
                int num = (int)MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            currencyManager.CancelCurrentEdit();
            if (myState == "Add")
            {
                currencyManager.Position = myBookmark;
            }
            SetState("View");
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                myBookmark = currencyManager.Position;
                DataRow newRow = DataTable.NewRow();

                // กำหนดค่า Employee_ID เมื่อเพิ่ม Record ใหม่
                if (DataTable.Columns.Contains("Employee__ID"))
                {
                    newRow["Employee__ID"] = EmployeeID;
                }

                // Generate QuotationID
                newRow["QuotationID"] = GenerateNextQuotationID();

                // เพิ่ม Row ใหม่ลงใน DataTable
                DataTable.Rows.Add(newRow);

                // เลื่อนไปยัง Row ใหม่
                currencyManager.Position = DataTable.Rows.Count - 1;
                currentRowView = (DataRowView)currencyManager.Current; // อัปเดต currentRowView

                SetState("Add");

                // Clear TextBox เพื่อให้ผู้ใช้ป้อนข้อมูลใหม่
                textBoxProductID.Text = "";
                textBoxDis.Text = "";
                textBoxQuantity.Text = "";
                textBoxID.Text = "";
                textBoxSubtotal.Text = "";
                // Clear TextBox อื่นๆ ที่เกี่ยวข้องด้วย

                // Focus ไปที่ TextBox แรกเพื่อให้ผู้ใช้เริ่มป้อนข้อมูลได้ทันที
                textBoxProductID.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการเพิ่มข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
        private int GenerateNextQuotationID()
        {
            int nextQuotationID = 1;

            try
            {
                string selectMaxIDQuery = "SELECT ISNULL(MAX(QuotationID), 0) FROM [Quotation_Data]";
                SqlCommand selectMaxIDCommand = new SqlCommand(selectMaxIDQuery, connection); // ตรวจสอบว่า testConnection ถูกต้อง
                int maxQuotationID = Convert.ToInt32(selectMaxIDCommand.ExecuteScalar());
                nextQuotationID = maxQuotationID + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการสร้าง QuotationID: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nextQuotationID;
        }

        private void FormQuotation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myState.Equals("Edit") || myState.Equals("Add"))
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
