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
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();
        }
        const string strConnStrFileName = "ConnectionString.ini";
        SqlConnection booksConnection;
        SqlDataAdapter titlesAdapter;
        DataTable titlesTable;
        CurrencyManager titlesManager;
        string myState;
        int myBookmark;
        SqlCommand titlesCommand; // เพิ่ม titlesCommand
        SqlDataAdapter publishersAdapter; // เพิ่ม publishersAdapter
        DataTable publishersTable; // เพิ่ม publishersTable
        SqlCommand publishersCommand; // เพิ่ม publishersCommand

        private void FormProduct_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                {
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                        Encoding.GetEncoding("Windows-874"));
                }
                else { MessageBox.Show("NO"); return; } // legalized nuclear bombs
                booksConnection = new SqlConnection(strConnectionString);

                // Establish command object
                titlesCommand = new SqlCommand("SELECT * FROM Products", booksConnection); // ปรับปรุงคำสั่ง SQL และเพิ่ม ORDER BY

                // Establish data adapter/data table
                titlesAdapter = new SqlDataAdapter();
                titlesAdapter.SelectCommand = titlesCommand;

                titlesTable = new DataTable();
                titlesAdapter.Fill(titlesTable);

                // Bind controls to data table
                textProductID.DataBindings.Add("Text", titlesTable, "ProductID");
                textProductName.DataBindings.Add("Text", titlesTable, "ProductName");
                textProductPrice.DataBindings.Add("Text", titlesTable, "ProductPrice");
                textDescription.DataBindings.Add("Text", titlesTable, "Description");
                textProductUse.DataBindings.Add("Text", titlesTable, "ProductUse");
                //textProductPicture.DataBindings.Add("Text", titlesTable, "ProductPicture");
                textProductCost.DataBindings.Add("Text", titlesTable, "Cost");
                textPromotionID.DataBindings.Add("Text", titlesTable, "PromotionID");
                textStock.DataBindings.Add("Text", titlesTable, "Stock");

                // Establish currency manager
                titlesManager = (CurrencyManager)this.BindingContext[titlesTable];

                //Establish category table/combo box to pick category
                publishersCommand = new SqlCommand("SELECT Category_ID, Category_Name FROM Category ORDER BY Category_Name",
                    booksConnection);
                publishersAdapter = new SqlDataAdapter();
                publishersAdapter.SelectCommand = publishersCommand;
                publishersTable = new DataTable();
                publishersAdapter.Fill(publishersTable);
                comboCategory.DataSource = publishersTable;
                comboCategory.DisplayMember = "Category_Name";
                comboCategory.ValueMember = "Category_ID";
                comboCategory.DataBindings.Add("SelectedValue", titlesTable, "Category_ID");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาดในการสร้างการทำงานกับตารางสินค้าและหมวดหมู่",
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
                    //groupFindTitle.Enabled = true; // ไม่มี groupFindTitle ใน FormProduct
                    comboCategory.Enabled = false;
                    //textISBN.BackColor = Color.White; // ไม่มี textISBN ใน FormProduct
                    //textISBN.ForeColor = Color.Black; // ไม่มี textISBN ใน FormProduct
                    textProductID.ReadOnly = true; // key
                    textProductName.ReadOnly = true;
                    textProductPrice.ReadOnly = true;
                    textDescription.ReadOnly = true;
                    textProductUse.ReadOnly = true;
                    textProductPicture.ReadOnly = true;
                    textProductCost.ReadOnly = true;
                    textPromotionID.ReadOnly = true;
                    textStock.ReadOnly = true;
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
                    textProductName.Focus();
                    break;
                default: // other than view
                    //groupFindTitle.Enabled = false; // ไม่มี groupFindTitle ใน FormProduct
                    comboCategory.Enabled = true;
                    textProductName.ReadOnly = false;
                    textProductPrice.ReadOnly = false;
                    textProductID.ReadOnly = false; // ปลด ReadOnly เพื่อให้แก้ไขได้เมื่อ Add ใหม่
                    textProductPicture.ReadOnly = false;
                    if (myState.Equals("Edit"))
                    {
                        textProductID.ReadOnly = true;
                        textProductID.TabStop = false;
                    }
                    else
                    {
                        textProductID.TabStop = true;
                    }
                    textDescription.ReadOnly = false;
                    textProductUse.ReadOnly = false;
                    //textProductPicture.ReadOnly = false;
                    textProductCost.ReadOnly = false;
                    textPromotionID.ReadOnly = false;
                    textStock.ReadOnly = false;
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
                    textProductName.Focus();
                    break;
            }
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            titlesManager.Position = 0;
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (titlesManager.Position == 0)
                return;
            --titlesManager.Position;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (titlesManager.Position == titlesManager.Count - 1)
                return;
            ++titlesManager.Position;
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            titlesManager.Position = titlesManager.Count - 1;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            SetState("Edit");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string savedName = textProductName.Text;
            try
            {
                if (!int.TryParse(textProductPrice.Text, out int productPrice))
                {
                    MessageBox.Show("กรุณาป้อนราคาผลิตภัณฑ์เป็นตัวเลข", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // ทำการตรวจสอบและแปลงค่า Cost, PromotionID, Stock ในลักษณะเดียวกัน

                titlesManager.EndCurrentEdit();
                SqlCommandBuilder titlesAdapterCommands = new SqlCommandBuilder(titlesAdapter);
                titlesAdapter.Update(titlesTable);
                MessageBox.Show("ข้อมูลถูกบันทึกแล้ว", "บันทึก", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SetState("View");
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล\n" + ex.Message, "ข้อผิดพลาด",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            titlesManager.CancelCurrentEdit();
            if (myState == "Add")
            {
                titlesManager.Position = myBookmark;
            }
            SetState("View");
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                myBookmark = titlesManager.Position;
                titlesManager.AddNew();
                SetState("Add");
            }
            catch (Exception)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการเพิ่มข้อมูล", "ข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้", "ลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;
            try
            {
                titlesManager.RemoveAt(titlesManager.Position);
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

        private void FormProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myState == "Edit" || myState == "Add")
            {
                MessageBox.Show("คุณต้องแก้ไขข้อมูลปัจจุบันให้เสร็จก่อนที่จะหยุดโปรแกรม", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else
            {
                try
                {
                    SqlCommandBuilder titlesAdapterCommands = new SqlCommandBuilder(titlesAdapter);
                    titlesAdapter.Update(titlesTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกฐานข้อมูล: \r\n" + ex.Message, "ข้อผิดพลาดในการบันทึก",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                // Close the connection
                booksConnection.Close();
                // Dispose of the objects
                booksConnection.Dispose();
                titlesAdapter.Dispose();
                titlesTable.Dispose();
            }
        }
    }
}
