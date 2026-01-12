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
    public partial class FormAcceptOrder : Form
    {
        public FormAcceptOrder()
        {
            InitializeComponent();
        }
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        private CurrencyManager currencyManager;
        public int EmployeeID;

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

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (DataTable.Rows.Count > 0 && currencyManager.Current != null)
            {
                DataRowView currentRowView = (DataRowView)currencyManager.Current;
                DataRow currentRow = currentRowView.Row;

                try
                {
                    // Assuming your PurchaseOrders table has columns:
                    // Employee_ID (int), QuotationID (int), PurchaseOrdersID (int - auto-increment or you generate it), PurchaseOrdersDate (date)
                    int id = GenerateNextQuotationID();
                    // Create a new command for inserting into PurchaseOrders
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO PurchaseOrders (Employee__ID, QuotationID,PurchaseOrdersID, PurchaseOrdersDate) VALUES (@EmployeeID, @QuotationID,@PurchaseOrdersID, @PurchaseOrdersDate)", connection))
                    {
                        // Parameters for the insert command
                        insertCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID); // Use the EmployeeID of the current user
                        insertCommand.Parameters.AddWithValue("@QuotationID", currentRow["QuotationID"]);
                        insertCommand.Parameters.AddWithValue("@PurchaseOrdersID", id);
                        insertCommand.Parameters.AddWithValue("@PurchaseOrdersDate", DateTime.Now.Date); // Set the Purchase Order Date to today

                        // Open the connection if it's not already open (though it should be from Form_Load)
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        // Execute the insert command
                        insertCommand.ExecuteNonQuery();

                        MessageBox.Show($"สร้าง Purchase Order สำหรับ QuotationID: {currentRow["QuotationID"]} สำเร็จ", "สร้าง Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Optionally, you might want to remove the processed quotation from the Quotation_Data table
                        // or mark it as processed. Be careful with this part as it depends on your workflow.
                        // currentRowView.Delete();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"เกิดข้อผิดพลาดในการสร้าง Purchase Order: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("ไม่มี Quotation ที่จะสร้าง Purchase Order", "ข้อมูลว่าง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private int GenerateNextQuotationID()
        {
            int nextQuotationID = 1;

            try
            {
                string selectMaxIDQuery = "SELECT ISNULL(MAX(PurchaseOrdersID), 0) FROM [PurchaseOrders]";
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
        private void FormAcceptOrder_Load(object sender, EventArgs e)
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
        }

        private void FormAcceptOrder_FormClosing(object sender, FormClosingEventArgs e)
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
