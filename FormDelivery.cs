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
    public partial class FormDelivery : Form
    {
        public FormDelivery()
        {
            InitializeComponent();
            comboBoxDStatus.Items.AddRange(new string[] { "เตรียมจัดส่ง", "กำลังจัดส่ง", "จัดส่งสำเร็จ", "มีปัญหาในการจัดส่ง" }); // Add your delivery statuses
            comboBoxDStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        private CurrencyManager currencyManager;

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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            currencyManager.CancelCurrentEdit();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SqlConnection tempConnection = null;
            try
            {
                currencyManager.EndCurrentEdit();

                // Create a new SqlCommand for updating only Delivery_Status and Delivery_Date
                SqlCommand updateCommand = new SqlCommand("UPDATE Delivery_Data SET Delivery_Status = @Delivery_Status, Delivery_Date = @Delivery_Date WHERE Packaget_Num = @Packaget_Num", connection);
                updateCommand.Parameters.AddWithValue("@Delivery_Status", comboBoxDStatus.SelectedItem?.ToString());
                updateCommand.Parameters.AddWithValue("@Delivery_Date", dateTimeDelivery.Value);
                updateCommand.Parameters.AddWithValue("@Packaget_Num", textBox14.Text); // Assuming textBox14 holds the primary key

                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                        Encoding.GetEncoding("Windows-874"));
                else
                {
                    MessageBox.Show("ไม่พบไฟล์กำหนดค่าการเชื่อมต่อ", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                tempConnection = new SqlConnection(strConnectionString);
                tempConnection.Open();

                updateCommand.Connection = tempConnection;

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("สถานะการจัดส่งถูกบันทึกแล้ว", "บันทึก", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    // Find the corresponding row in the DataTable and update it
                    DataRow[] foundRows = DataTable.Select($"Packaget_Num = '{textBox14.Text}'");
                    if (foundRows.Length > 0)
                    {
                        foundRows[0]["Delivery_Status"] = comboBoxDStatus.SelectedItem?.ToString();
                        foundRows[0]["Delivery_Date"] = dateTimeDelivery.Value;
                        DataTable.AcceptChanges(); // Accept the changes in the DataTable
                    }
                }
                else
                {
                    MessageBox.Show("ไม่พบข้อมูลการจัดส่งที่ต้องการบันทึก", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึกสถานะการจัดส่ง: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (tempConnection != null && tempConnection.State == ConnectionState.Open)
                {
                    tempConnection.Close();
                }
            }
        }


        private void buttonDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormDelivery_Load(object sender, EventArgs e)
        {

            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                        Encoding.GetEncoding("Windows-874"));
                else
                {
                    MessageBox.Show("ไม่พบไฟล์กำหนดค่าการเชื่อมต่อ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                connection = new SqlConnection(strConnectionString);

                connection.Open();

                command = new SqlCommand(@"SELECT
                                                     m.*,
                                                     dd.Packaget_Num,
                                                     dd.Delivery_Status,
                                                     dd.Delivery_Date
                                                 FROM
                                                     Member m
                                                 LEFT JOIN
                                                     Orders o ON m.MemberID = o.MemberID
                                                 LEFT JOIN
                                                     Packaget_Data pd ON o.OrderID = pd.OrderID
                                                 LEFT JOIN
                                                     Delivery_Data dd ON pd.Packaget_Num = dd.Packaget_Num;", connection);

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable = new DataTable();
                adapter.Fill(DataTable);

                textBoxMemberID.DataBindings.Add("Text", DataTable, "MemberID");
                textBoxFN.DataBindings.Add("Text", DataTable, "FirstName");
                textBoxLN.DataBindings.Add("Text", DataTable, "LastName");
                textBoxEM.DataBindings.Add("Text", DataTable, "Email");
                textBoxTL.DataBindings.Add("Text", DataTable, "Telephone");
                textBoxADD.DataBindings.Add("Text", DataTable, "Address");
                textBoxSUB.DataBindings.Add("Text", DataTable, "Subdistrict");
                textBoxDIS.DataBindings.Add("Text", DataTable, "District");
                textBoxPRO.DataBindings.Add("Text", DataTable, "Province");
                textBoxZIP.DataBindings.Add("Text", DataTable, "Zipcode");
                textBoxSOI.DataBindings.Add("Text", DataTable, "Soi");
                textBoxROAD.DataBindings.Add("Text", DataTable, "Road");
                textBoxV.DataBindings.Add("Text", DataTable, "Village");
                textBox14.DataBindings.Add("Text", DataTable, "Packaget_Num");
                comboBoxDStatus.DataBindings.Add("SelectedItem", DataTable, "Delivery_Status"); // Bind comboBoxDStatus
                dateTimeDelivery.DataBindings.Add("Value", DataTable, "Delivery_Date");     // Bind dateTimeDelivery

                currencyManager = (CurrencyManager)this.BindingContext[DataTable];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาดในการสร้างการทำงานกับตาราง",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
                return;
            }
            this.Show();
        }

        private void FormDelivery_FormClosing(object sender, FormClosingEventArgs e)
        {
            // No need to update the entire DataTable here.
            // The Save button handles individual updates.
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection?.Dispose();
            command?.Dispose();
            adapter?.Dispose();
            DataTable?.Dispose();
        }
    }
}
