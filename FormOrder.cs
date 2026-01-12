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
    public partial class FormOrder : Form
    {
        public FormOrder()
        {
            InitializeComponent();
        }
        const string strConnStrFileName = "ConnectionString.ini";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        private CurrencyManager currencyManager;
        public int MemberID;
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

        private void FormOrder_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));
                connection = new SqlConnection(strConnectionString);

                connection.Open();

                command = new SqlCommand(@"SELECT o.OrderID,
                                           pd.Packaget_Num AS PackageNumber,
                                           pd.GarunteeDate,
                                           dd.Delivery_Status,
                                           dd.Delivery_Date,
                                           pd.Send_Date
                                    FROM Member m
                                    LEFT JOIN Orders o ON m.MemberID = o.MemberID
                                    LEFT JOIN Packaget_Data pd ON o.OrderID = pd.OrderID
                                    LEFT JOIN Delivery_Data dd ON pd.Packaget_Num = dd.Packaget_Num
                                    WHERE m.MemberID = @member;", connection);
                command.Parameters.AddWithValue("@member", MemberID);
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable = new DataTable();
                adapter.Fill(DataTable);

                textBoxOrderID.DataBindings.Add("Text", DataTable, "OrderID"); // เปลี่ยนเป็น "OrderID"
                textBoxPackage.DataBindings.Add("Text", DataTable, "PackageNumber");
                textBoxGDate.DataBindings.Add("Text", DataTable, "GarunteeDate"); // เปลี่ยนเป็น "GarunteeDate"
                textBoxDStatus.DataBindings.Add("Text", DataTable, "Delivery_Status"); // เปลี่ยนเป็น "Delivery_Status"
                textBoxDDate.DataBindings.Add("Text", DataTable, "Delivery_Date"); // เปลี่ยนเป็น "Delivery_Date"
                textBoxSendDate.DataBindings.Add("Text", DataTable, "Send_Date"); // เปลี่ยนเป็น "Send_Date"


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
        }

        private void buttonClaim_Click(object sender, EventArgs e)
        {
            string packageNumber = textBoxPackage.Text;
            
            DateTime calmDate = DateTime.Now;
            string calmStatus = "รอตรวจสอบ"; // หรือได้จาก Dropdown

            
            string query = "INSERT INTO Claim_Data (Packaget_Num, MemberID, Calm_Date, Calm_Status) " +
                           "VALUES (@packageNum, @memberID, @calmDate, @calmStatus);";

            
            
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@packageNum", packageNumber);
                    command.Parameters.AddWithValue("@memberID", MemberID);
                    command.Parameters.AddWithValue("@calmDate", calmDate);
                    command.Parameters.AddWithValue("@calmStatus", calmStatus);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("บันทึกข้อมูลการเคลมสำเร็จ");
                        }
                        else
                        {
                            MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูลการเคลม");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                    }
                }
            
        }
    }
}
