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
    public partial class FormEditMem : Form
    {
        public FormEditMem()
        {
            InitializeComponent();
        }
        const string strConnStrFileName
            = "ConnectionString.ini";
        SqlConnection MemberConnection;
        SqlCommand MemberCommand;
        SqlDataAdapter MemberAdapter;
        DataTable MemberTable;
        CurrencyManager MemberManager;
        public int MemberID;
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
            {
                return;
            }
            

            if (MemberID == 0)
            {
                int MemberIdGen = GenerateNextMemberID();
                SqlTransaction transaction = null;
                try
                {
                    if (MemberConnection.State != ConnectionState.Open)
                    {
                        MemberConnection.Open();
                    }
                    transaction = MemberConnection.BeginTransaction();
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO Member VALUES(@ID,@FirstName,@LastName,@Email,@Telephone,@Address,@Subdistrict,@District,@Province,@Zipcode,@Password,@Soi,@Road,@Village)", MemberConnection, transaction);

                    insertCommand.Parameters.AddWithValue("@ID", MemberIdGen);
                    insertCommand.Parameters.AddWithValue("@FirstName", textBoxFirstName.Text);
                    insertCommand.Parameters.AddWithValue("@LastName", textBoxLastName.Text);
                    insertCommand.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                    insertCommand.Parameters.AddWithValue("@Telephone", textBoxTel.Text);
                    insertCommand.Parameters.AddWithValue("@Address", textBoxAddress.Text);
                    insertCommand.Parameters.AddWithValue("@Subdistrict", textBoxSubdis.Text);
                    insertCommand.Parameters.AddWithValue("@District", textBoxDis.Text);
                    insertCommand.Parameters.AddWithValue("@Province", textBoxProvince.Text);
                    insertCommand.Parameters.AddWithValue("@Zipcode", textBoxZipcode.Text);
                    insertCommand.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                    insertCommand.Parameters.AddWithValue("@Soi", textBoxZoi.Text);
                    insertCommand.Parameters.AddWithValue("@Road", textBoxRoad.Text);
                    insertCommand.Parameters.AddWithValue("@Village", textBoxVilliage.Text);
                    insertCommand.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก: " + ex.Message, "ข้อผิดพลาด");

                    // ยกเลิกการเปลี่ยนแปลงหากเกิดข้อผิดพลาด
                    transaction?.Rollback();
                    MemberTable.RejectChanges();
                }

            }
            else
            {
                try
                {
                    MemberManager.EndCurrentEdit();

                    SqlCommand updateCommand = new SqlCommand("UPDATE Member SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Telephone = @Telephone, Address = @Address, Subdistrict = @Subdistrict, District = @District, Province = @Province, Zipcode = @Zipcode, Password = @Password, Soi = @Soi, Road = @Road, Village = @Village WHERE MemberID = @Original_MemberID", MemberConnection);
                    
                    updateCommand.Parameters.AddWithValue("@FirstName", textBoxFirstName.Text);
                    updateCommand.Parameters.AddWithValue("@LastName", textBoxLastName.Text);
                    updateCommand.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                    updateCommand.Parameters.AddWithValue("@Telephone", textBoxTel.Text);
                    updateCommand.Parameters.AddWithValue("@Address", textBoxAddress.Text);
                    updateCommand.Parameters.AddWithValue("@Subdistrict", textBoxSubdis.Text);
                    updateCommand.Parameters.AddWithValue("@District", textBoxDis.Text);
                    updateCommand.Parameters.AddWithValue("@Province", textBoxProvince.Text);
                    updateCommand.Parameters.AddWithValue("@Zipcode", textBoxZipcode.Text);
                    updateCommand.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                    updateCommand.Parameters.AddWithValue("@Soi", textBoxZoi.Text);
                    updateCommand.Parameters.AddWithValue("@Road", textBoxRoad.Text);
                    updateCommand.Parameters.AddWithValue("@Village", textBoxVilliage.Text);
                    updateCommand.Parameters.AddWithValue("@Original_MemberID", MemberID);
                    MemberAdapter.UpdateCommand = updateCommand;

                    SqlCommandBuilder MemberAdapterCommands
                        = new SqlCommandBuilder(MemberAdapter);
                    // ... (ส่วนของการตรวจสอบ RowState)
                    MemberAdapter.Update(MemberTable); // ทำการอัปเดตฐานข้อมูล
                    MemberTable.AcceptChanges();
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึก: " + ex.Message, "ข้อผิดพลาด");

                    // ยกเลิกการเปลี่ยนแปลงหากเกิดข้อผิดพลาด

                    MemberTable.RejectChanges();
                }
            }
                

        }
        private int GenerateNextMemberID()
        {
            int nextOrderID = 1; // ค่าเริ่มต้นถ้ายังไม่มี Order ใน Table
            try
            {
                string selectMaxIDQuery = "SELECT ISNULL(MAX(MemberID), 0) FROM Member";
                SqlCommand selectMaxIDCommand = new SqlCommand(selectMaxIDQuery, MemberConnection);
                if (MemberConnection.State != ConnectionState.Open)
                {
                    MemberConnection.Open();
                }
                int maxOrderID = Convert.ToInt32(selectMaxIDCommand.ExecuteScalar());
                nextOrderID = maxOrderID + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการสร้าง OrderID: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (MemberConnection != null && MemberConnection.State == ConnectionState.Open)
                {
                    MemberConnection.Close();
                }
            }

            return nextOrderID;
        }

        private void FormEditMem_Load(object sender, EventArgs e)
        {
            
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));
                // Connect to books database
                MemberConnection = new SqlConnection(strConnectionString);
                // Connect to books database   
                // Open the connection   
                MemberConnection.Open();
                // Establish command object
                MemberCommand = new SqlCommand("Select * from Member WHERE MemberID = @ID",
                                       MemberConnection);
                MemberCommand.Parameters.AddWithValue("@ID", MemberID);
                // Establish data adapter/data table
                MemberAdapter = new SqlDataAdapter();
                MemberAdapter.SelectCommand = MemberCommand;
                MemberTable = new DataTable();
                MemberAdapter.Fill(MemberTable);
                // Bind controls to data table
                textBoxFirstName.DataBindings.Add("Text", MemberTable, "FirstName");
                textBoxLastName.DataBindings.Add("Text", MemberTable, "LastName");
                textBoxEmail.DataBindings.Add("Text", MemberTable, "Email");
                textBoxAddress.DataBindings.Add("Text", MemberTable, "Address");
                textBoxTel.DataBindings.Add("Text", MemberTable, "Telephone");
                textBoxSubdis.DataBindings.Add("Text", MemberTable, "Subdistrict");
                textBoxDis.DataBindings.Add("Text", MemberTable, "District");
                textBoxProvince.DataBindings.Add("Text", MemberTable, "Province");
                textBoxZipcode.DataBindings.Add("Text", MemberTable, "Zipcode");
                textBoxPassword.DataBindings.Add ("Text", MemberTable, "Password");
                textBoxZoi.DataBindings.Add("Text", MemberTable, "Soi");
                textBoxVilliage.DataBindings.Add("Text", MemberTable, "Village");
                textBoxRoad.DataBindings.Add("Text",MemberTable, "Road");
                // Establish currency manager
                MemberManager = (CurrencyManager)
                this.BindingContext[MemberTable];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาดในการสร้างการทำงานกับตารางผู้แต่ง",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            this.Show();
        }

        private bool ValidateData()
        {
            string message = "";
            bool allOK = true;
            // Check for name
            if (textBoxFirstName.Text.Trim().Equals(""))
            {
                message = "คุณต้องป้อนชื่อ" + "\r\n";
                textBoxFirstName.Focus();
                allOK = false;
            }
            // Check length and range on Year Born
            if (textBoxLastName.Text.Trim().Equals(""))
            {
                message = "คุณต้องป้อนนามสกุล" + "\r\n";
                textBoxLastName.Focus();
                allOK = false;
            }
            if (textBoxEmail.Text.Trim().Equals(""))
            {
                message = "คุณต้องป้อน Email" + "\r\n";
                textBoxEmail.Focus();
                allOK = false;
            }
            if (!textBoxTel.Text.Trim().Equals(""))
            {
                if (!int.TryParse(textBoxTel.Text, out int number))
                {
                    message = "กรุณาป้อนเบอร์โทรศัพท์เป็นตัวเลข" + "\r\n";
                    textBoxTel.Focus();
                    allOK = false;
                }
            }
            else
            {
                message = "คุณต้องป้อนเบอร์โทรศัพท์" + "\r\n";
                textBoxTel.Focus();
                allOK = false;
            }
            if (textBoxPassword.Text.Trim().Equals(""))
            {
                message = "คุณต้องป้อนรหัสผ่าน" + "\r\n";
                textBoxPassword.Focus();
                allOK = false;
            }
            if (textBoxAddress.Text.Trim().Equals("")||textBoxDis.Text.Trim().Equals("")||textBoxProvince.Text.Trim().Equals("")||textBoxProvince.Text.Trim().Equals(""))
            {
                message = "คุณต้องป้อนที่อยู่" + "\r\n";
                allOK = false;
            }

            if (!allOK)
            {
                MessageBox.Show(message, "ข้อผิดพลาดในการตรวจสอบ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            return (allOK);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
