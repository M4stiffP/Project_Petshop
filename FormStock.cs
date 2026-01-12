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
    public partial class FormStock : Form
    {
        public FormStock()
        {
            InitializeComponent();
        }
        public int EmployeeID;

        private void FormStock_Load(object sender, EventArgs e)
        {
            try
            {
                const string strConnStrFileName = "ConnectionString.ini";
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                {
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                    // Connect to books database
                    Encoding.GetEncoding("Windows-874"));
                }
                else { MessageBox.Show("NO"); System.Runtime.InteropServices.Marshal.ReadInt32(IntPtr.Zero); } // legalized nuclear bombs
                // ใช้ `using` เพื่อให้แน่ใจว่า Connection ปิดอัตโนมัติ
                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    string query = "SELECT ProductID, ProductName FROM Products ORDER BY ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable productsTable = new DataTable();
                        adapter.Fill(productsTable);

                        // กำหนดข้อมูลให้ ComboBox
                        comboboxProductName.DataSource = productsTable;
                        comboboxProductName.DisplayMember = "ProductName"; // แสดงชื่อสินค้า
                        comboboxProductName.ValueMember = "ProductID"; // ใช้ Product_ID เป็นค่า
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลสินค้า: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateStock(int productID,int addStock)
        {
            try
            {
                string query = "UPDATE Products SET Stock = Stock+@Add_Stock WHERE ProductID = @Product_ID";
                const string strConnStrFileName = "ConnectionString.ini";
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                {
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                    // Connect to books database
                    Encoding.GetEncoding("Windows-874"));
                }
                else { MessageBox.Show("NO"); System.Runtime.InteropServices.Marshal.ReadInt32(IntPtr.Zero); } // legalized nuclear bombs
                // ใช้ `using` เพื่อให้แน่ใจว่า Connection ปิดอัตโนมัติ
                using (SqlConnection connection = new SqlConnection(strConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Product_ID", productID);
                    cmd.Parameters.AddWithValue("@Add_Stock", addStock);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            try
            {
                // ตรวจสอบค่าที่เลือกใน ComboBox
                if (comboboxProductName.SelectedValue == null)
                {
                    MessageBox.Show("กรุณาเลือกสินค้า", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ตรวจสอบค่าที่ป้อนใน textAddStock และแปลงเป็นตัวเลข
                if (!int.TryParse(textAddStock.Text, out int addStock) || addStock <= 0)
                {
                    MessageBox.Show("กรุณากรอกจำนวนสินค้าให้ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ตรวจสอบค่า EmployeeID ว่ามีค่าหรือไม่
                if (EmployeeID <= 0)
                {
                    MessageBox.Show("ไม่พบรหัสพนักงาน กรุณาล็อกอินใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ดึงค่าจาก ComboBox
                int productID = Convert.ToInt32(comboboxProductName.SelectedValue);
                DateTime dateAdd = DateTime.Now;


                string query = "INSERT INTO AddNewStock (ProductID, Add_Stock, Date_Add, Employee__ID) " +
                               "VALUES (@Product_ID, @Add_Stock, @Date_Add, @Employee_ID)";
                const string strConnStrFileName = "ConnectionString.ini";
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                {
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                    // Connect to books database
                    Encoding.GetEncoding("Windows-874"));
                }
                else { MessageBox.Show("NO"); System.Runtime.InteropServices.Marshal.ReadInt32(IntPtr.Zero); } // legalized nuclear bombs
                // ใช้ `using` เพื่อให้แน่ใจว่า Connection ปิดอัตโนมัติ
                using (SqlConnection connection = new SqlConnection(strConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Product_ID", productID);
                    cmd.Parameters.AddWithValue("@Add_Stock", addStock);
                    cmd.Parameters.AddWithValue("@Date_Add", dateAdd);
                    cmd.Parameters.AddWithValue("@Employee_ID", EmployeeID);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        UpdateStock(productID,addStock);
                        MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
