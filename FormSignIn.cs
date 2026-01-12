using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Petshop
{
    public partial class FormSignIn : Form
    {
        public FormSignIn()
        {
            InitializeComponent();
        }
        SqlConnection logConnection;
        SqlCommand logCommand;
        SqlDataAdapter logAdapter;
        DataTable logTable;
        public int EmIDPublic;
        const string strConnStrFileName
        = "ConnectionString.ini";
        private Dictionary<string, string> _EmNames = new Dictionary<string, string>(); // เก็บเบอร์โทรศัพท์และชื่อสมาชิก
        private Dictionary<string, string> _EmID = new Dictionary<string, string>();
        private Dictionary<string, string> _EmPasswords = new Dictionary<string, string>(); // เก็บเบอร์โทรศัพท์และรหัสผ่าน

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string user_phone = textBoxPhone.Text.Trim();
            string password = textBoxPassword.Text; // ไม่ต้อง Trim เพราะอาจมีช่องว่างสำคัญ

            if (string.IsNullOrEmpty(user_phone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("กรุณากรอกเบอร์โทรศัพท์และรหัสผ่าน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_EmNames.ContainsKey(user_phone))
            {
                // พบเบอร์โทรศัพท์ในระบบ
                if (_EmPasswords.ContainsKey(user_phone) && _EmPasswords[user_phone] == password)
                {
                    // รหัสผ่านถูกต้อง
                    EmIDPublic = Convert.ToInt32(_EmID[user_phone]);
                    this.Hide(); // ซ่อน Form SignIn
                    FormSelectEmployeeFunc formSelectEmployeeFunc = new FormSelectEmployeeFunc();
                    formSelectEmployeeFunc.EmIDPublic = EmIDPublic;
                    formSelectEmployeeFunc.ShowDialog();
                }
                else
                {
                    // รหัสผ่านไม่ถูกต้อง
                    MessageBox.Show("รหัสผ่านไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPassword.Clear(); // ล้างรหัสผ่านเมื่อไม่ถูกต้อง
                    textBoxPassword.Focus(); // ให้เคอร์เซอร์ไปที่ช่องรหัสผ่าน
                }
            }
            else
            {
                // ไม่พบเบอร์โทรศัพท์ในระบบ
                MessageBox.Show($"ไม่พบเบอร์โทรศัพท์ '{user_phone}' ในระบบของเรา", "ไม่พบในระบบ!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                textBoxPhone.Clear(); // ล้างเบอร์โทรศัพท์เมื่อไม่พบ
                textBoxPhone.Focus(); // ให้เคอร์เซอร์ไปที่ช่องเบอร์โทรศัพท์
            }
        }

        private void FormSignIn_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));

                logConnection = new SqlConnection(strConnectionString);
                logConnection.Open();
                logCommand = new SqlCommand("Select * from Employee", logConnection);
                logAdapter = new SqlDataAdapter();
                logAdapter.SelectCommand = logCommand;
                logTable = new DataTable();
                logAdapter.Fill(logTable);
                List<string> Phonenum = new List<string>();
                List<string> MemberName = new List<string>();
                foreach (DataRow row in logTable.Rows)
                {
                    if (row.Table.Columns.Contains("Employee_PhoneNum") &&
                        row.Table.Columns.Contains("Employee_FName") &&
                        row.Table.Columns.Contains("Employee__ID") &&
                        row.Table.Columns.Contains("Employee_Password"))
                    {
                        string phone = row["Employee_PhoneNum"].ToString().Trim();
                        string firstName = row["Employee_FName"].ToString().Trim();
                        string ID = row["Employee__ID"].ToString().Trim();
                        string password = row["Employee_Password"].ToString().Trim();

                        _EmNames[phone] = firstName;
                        _EmID[phone] = ID;
                        _EmPasswords[phone] = password;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (logConnection != null && logConnection.State == ConnectionState.Open)
                    logConnection.Close();
            }

        }
    }
}
