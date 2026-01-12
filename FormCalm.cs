using iTextSharp.text;
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
    public partial class FormCalm : Form
    {
        public FormCalm()
        {
            InitializeComponent();
            comboStatus.Items.AddRange(new string[] { "ดำเนินการ", "รออนุมัติ", "อนุมัติแล้ว", "ไม่อนุมัติ" });
            comboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SqlConnection tempConnection = null; // สร้างตัวแปร connection ชั่วคราว
            try
            {
                currencyManager.EndCurrentEdit();

                // Create a new SqlCommand for updating only Calm_Status
                SqlCommand updateCommand = new SqlCommand("UPDATE Claim_Data SET Calm_Status = @Calm_Status WHERE Packaget_Num = @Packaget_Num", connection);
                updateCommand.Parameters.AddWithValue("@Calm_Status", comboStatus.SelectedItem?.ToString()); // Get selected item from comboStatus
                updateCommand.Parameters.AddWithValue("@Packaget_Num", textBox1.Text); // Assuming textBox1 holds the primary key

                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                {
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                    Encoding.GetEncoding("Windows-874"));
                }
                else
                {
                    MessageBox.Show("ไม่พบไฟล์กำหนดค่าการเชื่อมต่อ", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit if connection string file is missing
                }
                tempConnection = new SqlConnection(strConnectionString); // สร้าง connection ใหม่
                tempConnection.Open(); // เปิด connection ใหม่

                updateCommand.Connection = tempConnection; // กำหนด connection ให้กับ command

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("สถานะถูกบันทึกแล้ว", "บันทึก", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    // Optionally, refresh the DataTable to reflect the changes
                    DataTable.Clear();
                    adapter.Fill(DataTable);
                    DataTable.DefaultView.Sort = "Packaget_Num";
                    currencyManager.Position = DataTable.DefaultView.Find(textBox1.Text);
                }
                else
                {
                    MessageBox.Show("ไม่พบข้อมูลที่ต้องการบันทึก", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึกสถานะ: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (tempConnection != null && tempConnection.State == ConnectionState.Open)
                {
                    tempConnection.Close(); // ปิด connection เสมอใน finally block
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            currencyManager.CancelCurrentEdit();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้", "ลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;
            try
            {
                currencyManager.RemoveAt(currencyManager.Position);
                // You might need to update the database here as well if you want to permanently delete the record.
                // Consider adding code to execute a DELETE command on the database.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการลบข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormCalm_Load(object sender, EventArgs e)
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

                command = new SqlCommand("SELECT * FROM Claim_Data", connection);

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable = new DataTable();
                adapter.Fill(DataTable);

                textBox1.DataBindings.Add("Text", DataTable, "Packaget_Num");
                textBox2.DataBindings.Add("Text", DataTable, "MemberID");
                textBox3.DataBindings.Add("Text", DataTable, "Calm_Date");
                comboStatus.DataBindings.Add("SelectedItem", DataTable, "Calm_Status"); // Bind comboStatus

                currencyManager = (CurrencyManager)this.BindingContext[DataTable];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาดในการโหลดข้อมูล",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
                return;
            }
        }

        private void FormCalm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // No need to update the entire DataTable here, as the Save button handles individual updates.
            // We still close and dispose of the connection and other objects.
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
