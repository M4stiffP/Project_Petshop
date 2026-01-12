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
using System.Security.Cryptography;

namespace Project_Petshop
{
    public partial class FormPackage : Form
    {
        public FormPackage()
        {
            InitializeComponent();
        }
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private string GeneratePackageNumber()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[5]; // 5 bytes will give a large enough number
                rng.GetBytes(randomNumber);
                int value = BitConverter.ToInt32(randomNumber, 0);
                // Ensure the number is positive and has 10 digits
                long packageNumLong = Math.Abs((long)value % 10000000000L);
                return packageNumLong.ToString("D10"); // Format as 10-digit number with leading zeros
            }
        }
        private void buttonEntry_Click(object sender, EventArgs e)
        {
            string orderID = textBoxOrderID.Text;
            string packageNum = GeneratePackageNumber(); // Generate package number
            DateTime sendDate = dateTimePicker1.Value;
            DateTime garunteeDate = sendDate.AddDays(60);
            string strConnectionString = "";

            string query = "INSERT INTO Packaget_Data (OrderID, GarunteeDate, Packaget_Num, Send_Date) " +
                           "VALUES (@OrderID, @GarunteeDate, @Packaget_Num, @Send_Date)";
            if (File.Exists(strConnStrFileName))
            {
                strConnectionString = File.ReadAllText(strConnStrFileName,
                // Connect to books database
                Encoding.GetEncoding("Windows-874"));
            }
            else { MessageBox.Show("ไม่พบไฟล์กำหนดค่าการเชื่อมต่อ", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; } // Improved message
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    command.Parameters.AddWithValue("@GarunteeDate", garunteeDate);
                    command.Parameters.AddWithValue("@Packaget_Num", packageNum);
                    command.Parameters.AddWithValue("@Send_Date", sendDate);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"บันทึกข้อมูลลง Database สำเร็จ\nหมายเลขพัสดุ: {packageNum}"); // Display generated package number
                            textBoxOrderID.Clear(); 
                            // dateTimePicker1.Value = DateTime.Now; // รีเซ็ตวันที่ (ถ้าต้องการ)
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึก: {ex.Message}");
                    }
                }
            }
        }
    }
    
}
