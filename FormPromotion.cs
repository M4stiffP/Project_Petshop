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
using static System.Windows.Forms.AxHost;

namespace Project_Petshop
{
    public partial class FormPromotion : Form
    {
        public FormPromotion()
        {
            InitializeComponent();
        }
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        private CurrencyManager currencyManager;
        private string myState;
        private int myBookmark;
        public int EmployeeID;

        private void FormPromotion_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));
                connection = new SqlConnection(strConnectionString);

                connection.Open();

                command = new SqlCommand("SELECT * FROM Promotion", connection);

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable = new DataTable();
                adapter.Fill(DataTable);

                textPromotionID.DataBindings.Add("Text", DataTable, "PromotionID");
                textPromotionName.DataBindings.Add("Text", DataTable, "PromotionName");
                textStartdate.DataBindings.Add("Text", DataTable, "Startdate");
                textEnddate.DataBindings.Add("Text", DataTable, "Enddate");
                textDiscount.DataBindings.Add("Text", DataTable, "Discount");


                currencyManager = (CurrencyManager)this.BindingContext[DataTable];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาดในการสร้างการทำงานกับตารางสำนักพิมพ์",
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
                    textPromotionID.ReadOnly = true; // key
                    textPromotionName.ReadOnly = true;
                    textPromotionName.ReadOnly = true;
                    textStartdate.ReadOnly = true;
                    textEnddate.ReadOnly = true;
                    textDiscount.ReadOnly = true;
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
                    textPromotionName.Focus();
                    break;
                default: // other than view
                    textPromotionID.ReadOnly = false; // key
                    textPromotionName.ReadOnly = false;
                    textPromotionName.ReadOnly = false;
                    textStartdate.ReadOnly = false;
                    textEnddate.ReadOnly = false;
                    textDiscount.ReadOnly = false;

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
                    textPromotionName.Focus();
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
                string text = textPromotionID.Text;
                currencyManager.EndCurrentEdit();
                DataTable.DefaultView.Sort = "PromotionID";
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
                currencyManager.AddNew();
                DataRow newRow = ((DataRowView)currencyManager.Current).Row;
                if (DataTable.Columns.Contains("Employee__ID"))
                {
                    newRow["Employee__ID"] = EmployeeID;
                }
                SetState("Add");
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
