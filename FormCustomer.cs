using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.IO;
using static System.Windows.Forms.AxHost;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace Project_Petshop
{
    public partial class FormCustomer : Form
    {
        const string strConnStrFileName
                = "ConnectionString.ini";
        SqlConnection testConnection;
        SqlCommand testCommand;
        SqlDataAdapter testAdapter;
        DataTable testTable;
        SqlConnection logConnection;
        SqlCommand logCommand;
        SqlDataAdapter logAdapter;
        DataTable logTable;
        SqlDataAdapter payadapter;
        DataTable paymentMethodsTable;
        SqlDataAdapter Catadapter;
        DataTable CatagoryTable;
        SqlDataAdapter Newdapter;
        DataTable NewTable;
        private string[] _sarMenu;
        private string[] _sarDis;
        private double[] _darCost;

        private Dictionary<string, string> _memberNames = new Dictionary<string, string>(); // เก็บเบอร์โทรศัพท์และชื่อสมาชิก
        private Dictionary<string, string> _memberID = new Dictionary<string, string>();
        public int memberID;
        public string _loggedInMemberName = "";


        private List<OrderItem> _currentOrderItems = new List<OrderItem>();
        private string _imageFolderPath = "D:\\DataProgram\\Project_Petshop\\images\\items"; // ระบุพาธโฟลเดอร์ที่เก็บรูปภาพ
        private double _dTotalAmount = 0.0;
        private double _currentProductDiscount = 0;

        public object Resource { get; private set; }

        public FormCustomer()
        {
            InitializeComponent();
        }
        
        private void formMyCoffee_Load(object sender, EventArgs e)
        {
            try
            {
                // โหลดข้อมูลจากฐานข้อมูลเข้าสู่ testTable
                LoadProductDataTable();
                LoadPaymentMethods();
                // แยกข้อมูลจาก testTable โดยส่งให้กับ LoadProductData
                LoadProductData(testTable);
                if (listMenu.Items.Count > 0)
                {
                    listMenu.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดฟอร์ม: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Show();
            labelMember.Text ="";
        }
        private void LoadProductDataTable()
        {
            listMenu.Items.Clear();

            List<string> storeNames = new List<string>();
            List<string> sarDiscirptList = new List<string>();
            List<double> costList = new List<double>();

            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));

                testConnection = new SqlConnection(strConnectionString);
                testConnection.Open();
                testCommand = new SqlCommand("Select * from Products", testConnection);
                testAdapter = new SqlDataAdapter();
                testAdapter.SelectCommand = testCommand;
                testTable = new DataTable();
                testAdapter.Fill(testTable);
                foreach (DataRow row in testTable.Rows)
                {
                    if (row.Table.Columns.Contains("ProductName"))
                    {
                        storeNames.Add(row["ProductName"].ToString());
                    }
                    if (row.Table.Columns.Contains("Description"))
                    {
                        sarDiscirptList.Add(row["Description"].ToString());

                    }
                    else
                    {
                        sarDiscirptList.Add("");
                    }
                    if (row.Table.Columns.Contains("ProductPrice") && double.TryParse(row["ProductPrice"].ToString(), out double price))
                    {
                        costList.Add(price);
                    }
                    else
                    {
                        costList.Add(0);
                    }
                }
                _sarMenu = storeNames.ToArray();
                _sarDis = sarDiscirptList.ToArray();
                _darCost = costList.ToArray();
                listMenu.Items.AddRange(_sarMenu);
                listMenu.SelectedIndex = 0;
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
        private void LoadPaymentMethods()
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));

                testConnection = new SqlConnection(strConnectionString);
                testConnection.Open();
                testAdapter.SelectCommand = testCommand;
                string query = "SELECT * FROM Payment";
                payadapter = new SqlDataAdapter(query, testConnection);
                paymentMethodsTable = new DataTable();
                payadapter.Fill(paymentMethodsTable);

                // กำหนด DataSource, DisplayMember และ ValueMember ของ ComboBox
                comboBoxSelect.DataSource = paymentMethodsTable;
                comboBoxSelect.DisplayMember = "PaymentMethod"; // คอลัมน์ที่จะแสดงใน ComboBox
                comboBoxSelect.ValueMember = "PaymentMethod"; // คอลัมน์ที่จะใช้เป็นค่า (ในกรณีนี้เหมือนกับ DisplayMember)
                LoadComboCate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดวิธีการชำระเงิน: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private void LoadComboCate()
        {
            string strConnectionString = "";
            if (File.Exists(strConnStrFileName))
                strConnectionString = File.ReadAllText(strConnStrFileName,
                                                Encoding.GetEncoding("Windows-874"));

            testConnection = new SqlConnection(strConnectionString);
            testConnection.Open();
            testAdapter.SelectCommand = testCommand;
            string query = "SELECT * FROM Category";
            Catadapter = new SqlDataAdapter(query, testConnection);
            CatagoryTable = new DataTable();
            Catadapter.Fill(CatagoryTable);
            comboCategory.DataSource = CatagoryTable;
            comboCategory.DisplayMember = "Category_Name";
            comboCategory.ValueMember = "Category_Name";
            comboCategory.SelectedValue = "";
        }
        private void LoadProductImage(string productName)
        {
            try
            {
                if (testConnection.State != ConnectionState.Open)
                {
                    testConnection.Open();
                }

                string selectImageQuery = "SELECT ProductPicture FROM Products WHERE ProductName = @ProductName"; // ดึงชื่อไฟล์จาก ProductPicture
                SqlCommand selectImageCommand = new SqlCommand(selectImageQuery, testConnection);
                selectImageCommand.Parameters.AddWithValue("@ProductName", productName);

                using (SqlDataReader reader = selectImageCommand.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        string imageFileName = reader["ProductPicture"].ToString() + ".jpg"; // ใช้ชื่อคอลัมน์ที่ถูกต้องคือ ProductPicture
                        string imageFullPath = Path.Combine(_imageFolderPath, imageFileName); // สร้างพาธเต็ม

                        if (File.Exists(imageFullPath))
                        {
                            pictDemo.Image = Image.FromFile(imageFullPath);
                            pictDemo.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else
                        {
                            pictDemo.Image = null;
                            MessageBox.Show($"ไม่พบไฟล์รูปภาพ: {imageFullPath}", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        pictDemo.Image = null; // เคลียร์ภาพถ้าไม่พบข้อมูลในฐานข้อมูล
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดภาพ: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictDemo.Image = null;
            }
            finally
            {

            }
        }
        private void LoadProductData(DataTable productTable)
        {
            listMenu.Items.Clear();
            List<string> storeNames = new List<string>();
            List<string> sarDiscirptList = new List<string>();
            List<double> costList = new List<double>();
            if (productTable != null && productTable.Rows.Count > 0)
            {
                foreach (DataRow row in productTable.Rows)
                {
                    if (row.Table.Columns.Contains("ProductName"))
                    {
                        storeNames.Add(row["ProductName"].ToString());
                    }
                    if (row.Table.Columns.Contains("Description"))
                    {
                        sarDiscirptList.Add(row["Description"].ToString());
                    }
                    else
                    {
                        sarDiscirptList.Add("");
                    }
                    if (row.Table.Columns.Contains("ProductPrice") && double.TryParse(row["ProductPrice"].ToString(), out double price))
                    {
                        costList.Add(price);
                    }
                    else
                    {
                        costList.Add(0);
                    }
                }

                _sarMenu = storeNames.ToArray();
                listMenu.Items.Clear();
                listMenu.Items.AddRange(_sarMenu);
                _sarDis = sarDiscirptList.ToArray();
                _darCost = costList.ToArray();

            }
            else
            {
                _sarMenu = new string[0];
                _sarDis = new string[0];
                _darCost = new double[0];
                listMenu.Items.Clear();
                MessageBox.Show("ไม่พบข้อมูลสินค้าใน DataTable", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void listMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMenu.SelectedIndex >= 0 && listMenu.SelectedIndex < _sarDis.Length)
            {
                // ดึง Index ของรายการที่ถูกเลือก
                int selectedIndex = listMenu.SelectedIndex;
                // นำข้อมูลไปแสดงใน textBoxDiscript (ตัวอย่าง: แสดงแต่ละตัวเลือกในบรรทัดใหม่)
                LabelDiscript.Text = _sarDis[selectedIndex].ToString();
                labelCost.Text= _darCost[selectedIndex].ToString("N2");
                if (listMenu.SelectedItem != null)
                {
                    LoadProductImage(listMenu.SelectedItem.ToString());
                }
                UpdateStockCountLabel(selectedIndex);

                if (testTable.Columns.Contains("PromotionID") && testTable.Rows[selectedIndex]["PromotionID"] != DBNull.Value)
                {
                    int promotionID = Convert.ToInt32(testTable.Rows[selectedIndex]["PromotionID"]);
                    LoadAndDisplayDiscount(promotionID);
                }
                else
                {
                    if (NewTable != null && NewTable.Rows.Count > 0 && NewTable.Columns.Contains("PromotionID") && NewTable.Rows.Count > selectedIndex && NewTable.Rows[selectedIndex]["PromotionID"] != DBNull.Value)
                    {
                        int promotionID = Convert.ToInt32(NewTable.Rows[selectedIndex]["PromotionID"]);
                        LoadAndDisplayDiscount(promotionID);
                    }
                    else
                    {
                        labelDiscount.Text = "ไม่มีส่วนลด";
                        _currentProductDiscount = 0;
                    }
                }
                
            }
            else
            {
                // หากไม่มีการเลือก หรือ Index ไม่ถูกต้อง ให้เคลียร์ textBoxDiscript
                LabelDiscript.Text = "";
                labelCost.Text = "";
                if (this.Controls.ContainsKey("lblStockCount"))
                {
                    ((Label)this.Controls["lblStockCount"]).Text = "";
                }
            }
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {

            if (listMenu.SelectedIndex != -1)
            {
                int selectedIndex = listMenu.SelectedIndex;
                int quantityToOrder = (int)numQuantity.Value;

                if (quantityToOrder >= 1)
                {
                    if (testTable.Columns.Contains("Stock"))
                    {
                        if (int.TryParse(testTable.Rows[selectedIndex]["Stock"].ToString(), out int currentStock))
                        {
                            if (quantityToOrder <= currentStock)
                            {
                                AddItemToBasket(selectedIndex, quantityToOrder);
                                testTable.Rows[selectedIndex]["Stock"] = currentStock - quantityToOrder;
                                UpdateStockCountLabel(selectedIndex);
                            }
                            else
                            {
                                MessageBox.Show($"สินค้า {_sarMenu[selectedIndex]} มีอยู่ในสต็อกเพียง {currentStock} ชิ้น กรุณาลดจำนวน", "สินค้าคงคลังไม่เพียงพอ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"ไม่สามารถอ่านจำนวนสินค้าคงคลังสำหรับ {_sarMenu[selectedIndex]} ได้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        AddItemToBasket(selectedIndex, quantityToOrder);
                    }
                }
                else
                {
                    MessageBox.Show("กรุณากรอกจำนวนสินค้าเป็นตัวเลขที่มากกว่า 0", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกรายการสินค้า", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }
        private void AddItemToOrder(int selectedIndex, int quantity)
        {
            double unitPrice = _darCost[selectedIndex];
            double totalCost = (quantity * unitPrice)-(_currentProductDiscount* (quantity * unitPrice)/100);
            string productName = _sarMenu[selectedIndex];
            int productID;
            try
            {
                testConnection.Open();
                string prouctQuery = "SELECT ProductID FROM Products WHERE ProductName = @ProductName";
                SqlCommand productCommand = new SqlCommand(prouctQuery, testConnection);
                productCommand.Parameters.AddWithValue("@ProductName", productName);

                // ExecuteScalar จะคืนค่าแรกของ Row แรก หรือ null ถ้าไม่พบ
                object result = productCommand.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    productID = Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show($"ไม่พบ ProductID สำหรับสินค้า '{productName}' ในฐานข้อมูล", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // ไม่อนุญาตให้เพิ่มสินค้าถ้าไม่พบ ProductID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการดึง ProductID: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (testConnection != null && testConnection.State == System.Data.ConnectionState.Open)
                {
                    testConnection.Close();
                }
            }

            _currentOrderItems.Add(new OrderItem { ProductID = productID, ProductName = productName, Quantity = quantity, UnitPrice = unitPrice, Subtotal = totalCost });
            //UpdateOrderListBox();
        }

        private void AddItemToBasket(int selectedIndex, int quantity)
        {
            string productNameFromMenu = _sarMenu[selectedIndex];
            int productID = -1; // Initialize productID
            decimal unitPriceFromDB = 0;
            string actualProductName = "";

            try
            {

                // ดึงข้อมูลสินค้า
                string productQuery = "SELECT ProductID, ProductName, ProductPrice FROM Products WHERE ProductName = @ProductName";
                SqlCommand productCommand = new SqlCommand(productQuery, testConnection);
                productCommand.Parameters.AddWithValue("@ProductName", productNameFromMenu);

                using (SqlDataReader productReader = productCommand.ExecuteReader())
                {
                    if (productReader.Read())
                    {
                        productID = Convert.ToInt32(productReader["ProductID"]);
                        actualProductName = productReader["ProductName"].ToString();
                        unitPriceFromDB = Convert.ToDecimal(productReader["ProductPrice"]);
                    }
                    else
                    {
                        MessageBox.Show($"ไม่พบ ProductID สำหรับสินค้า '{productNameFromMenu}' ในฐานข้อมูล", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                } // productReader ถูกปิดแล้ว

                if (productID != -1)
                {
                    // คำนวณ totalCost
                    double totalCost = (quantity * (double)unitPriceFromDB) - (_currentProductDiscount * (quantity * (double)unitPriceFromDB) / 100);

                    // เพิ่มรายการลงในตาราง Basket_Data
                    string insertQuery = "INSERT INTO Basket_Data (MemberID, ProductID, Quantity, Subtotal) " +
                                         "VALUES (@MemberID, @ProductID, @Quantity, @Subtotal)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, testConnection)) // สร้าง SqlCommand ใหม่
                    {
                        insertCommand.Parameters.AddWithValue("@MemberID", memberID);
                        insertCommand.Parameters.AddWithValue("@ProductID", productID);
                        insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                        insertCommand.Parameters.AddWithValue("@Subtotal", totalCost);

                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"เพิ่ม '{actualProductName}' จำนวน {quantity} รายการลงในตะกร้าสินค้าแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UpdateOrderListBox(); // เรียก UpdateOrderListBox เพื่อแสดงรายการใหม่
                        }
                        else
                        {
                            MessageBox.Show($"ไม่สามารถเพิ่ม '{actualProductName}' ลงในตะกร้าสินค้าได้", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการจัดการตะกร้าสินค้า: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {

            }

        }
        private void UpdateOrderListBox()
        {
            listOrder.Items.Clear();
            _currentOrderItems.Clear();
            try
            {

                string selectQuery = "SELECT p.ProductName, bd.Quantity, p.ProductPrice, bd.Subtotal, bd.ProductID " + // ดึง ProductID ด้วย
                                     "FROM Basket_Data bd " +
                                     "INNER JOIN Products p ON bd.ProductID = p.ProductID " +
                                     "WHERE bd.MemberID = @MemberID"; // เพิ่มเงื่อนไข MemberID
                SqlCommand selectCommand = new SqlCommand(selectQuery, testConnection);
                selectCommand.Parameters.AddWithValue("@MemberID", memberID); // ใช้ MemberID ปัจจุบัน

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productName = reader["ProductName"].ToString();
                        int quantity = Convert.ToInt32(reader["Quantity"]);
                        decimal unitPrice = Convert.ToDecimal(reader["ProductPrice"]);
                        decimal subtotal = Convert.ToDecimal(reader["Subtotal"]);
                        int productID = Convert.ToInt32(reader["ProductID"]); // อ่าน ProductID จากฐานข้อมูล

                        listOrder.Items.Add($"{productName}*{quantity} @ {unitPrice:N2} = {subtotal:N2}");

                        // สร้าง OrderItem และเพิ่มลงใน _currentOrderItems
                        _currentOrderItems.Add(new OrderItem
                        {
                            ProductID = productID,
                            ProductName = productName,
                            Quantity = quantity,
                            UnitPrice = (double)unitPrice, // แปลง decimal เป็น double
                            Subtotal = (double)subtotal   // แปลง decimal เป็น double
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการดึงข้อมูลตะกร้าสินค้า: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }

        }
        private void UpdateStockCountLabel(int selectedIndex)
        {
            if (testTable.Columns.Contains("Stock"))
            {
                if (int.TryParse(testTable.Rows[selectedIndex]["Stock"].ToString(), out int updatedStock))
                {
                    lblStockCount.Text = $"คงเหลือ: {updatedStock} ชิ้น";
                }
                else
                {
                    lblStockCount.Text = "ไม่ทราบจำนวนคงเหลือ";
                }
            }
        }

        private void buttonRemoveItem_Click(object sender, EventArgs e)
        {
            if (listOrder.SelectedIndex == -1)
                return;

            string selectedItemText = listOrder.SelectedItem.ToString();
            string productNameToRemove = selectedItemText.Split('*')[0];
            OrderItem itemToRemove = _currentOrderItems.FirstOrDefault(item => item.ProductName == productNameToRemove);

            if (itemToRemove != null)
            {
                try
                {

                    // 1. ลบรายการออกจาก Basket_Data โดยใช้ ProductID และ MemberID
                    string deleteQuery = "DELETE FROM Basket_Data WHERE ProductID = @ProductID AND MemberID = @MemberID";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, testConnection);
                    deleteCommand.Parameters.AddWithValue("@ProductID", itemToRemove.ProductID);
                    deleteCommand.Parameters.AddWithValue("@MemberID", memberID); // ใช้ memberID ของผู้ใช้ปัจจุบัน
                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // 2. คืนจำนวนสินค้าเข้าสต็อก (ถ้ามีการจัดการสต็อก)
                        if (testTable.Columns.Contains("Stock"))
                        {
                            int productIndex = Array.IndexOf(_sarMenu, itemToRemove.ProductName);
                            if (productIndex != -1 && int.TryParse(testTable.Rows[productIndex]["Stock"].ToString(), out int currentStock))
                            {
                                testTable.Rows[productIndex]["Stock"] = currentStock + itemToRemove.Quantity;
                                UpdateStockCountLabel(productIndex); // อัปเดต Label แสดงสต็อก
                            }
                        }

                        // 3. ลบ OrderItem ออกจาก _currentOrderItems
                        _currentOrderItems.Remove(itemToRemove);

                        // 4. อัปเดต listOrder และราคารวม
                        UpdateOrderListBox();
                    }
                    else
                    {
                        MessageBox.Show($"ไม่พบสินค้า '{productNameToRemove}' ในตะกร้าสินค้าในฐานข้อมูล", "ข้อผิดพลาดในการลบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"เกิดข้อผิดพลาดในการลบสินค้าออกจากตะกร้าสินค้า: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {

                }
            }
            else
            {
                // แสดงข้อความแจ้งว่าไม่พบสินค้าที่จะลบใน _currentOrderItems
                MessageBox.Show($"ไม่พบสินค้า '{productNameToRemove}' ในรายการสั่งซื้อภายใน", "ข้อผิดพลาดในการลบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonClearItem_Click(object sender, EventArgs e)
        {
            try
            {

                // 1. ลบรายการทั้งหมดออกจาก Basket_Data สำหรับ MemberID ปัจจุบัน
                string deleteQuery = "DELETE FROM Basket_Data WHERE MemberID = @MemberID";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, testConnection);
                deleteCommand.Parameters.AddWithValue("@MemberID", memberID); // ใช้ memberID ของผู้ใช้ปัจจุบัน
                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // แสดงจำนวนรายการที่ถูกลบ (สำหรับ Debug)
                Console.WriteLine($"ลบ {rowsAffected} รายการออกจาก Basket_Data");

                // 2. คืนจำนวนสินค้าทั้งหมดเข้าสต็อก (ถ้ามีการจัดการสต็อก)
                if (testTable.Columns.Contains("Stock"))
                {
                    foreach (var item in _currentOrderItems)
                    {
                        int productIndex = Array.IndexOf(_sarMenu, item.ProductName);
                        if (productIndex != -1 && int.TryParse(testTable.Rows[productIndex]["Stock"].ToString(), out int currentStock))
                        {
                            testTable.Rows[productIndex]["Stock"] = currentStock + item.Quantity;
                            UpdateStockCountLabel(productIndex); // อัปเดต Label แสดงสต็อก
                        }
                    }
                }

                // 3. เคลียร์ _currentOrderItems และ listOrder
                _currentOrderItems.Clear();
                listOrder.Items.Clear();

                // 4. อัปเดตราคารวม
                if (this.Controls.ContainsKey("lblTotalAmount"))
                {
                    ((Label)this.Controls["lblTotalAmount"]).Text = $"ราคารวม: {0:N2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการเคลียร์ตะกร้าสินค้า: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonCalTotalAmount_Click(object sender, EventArgs e)
        {
            listOrder.Items.Clear();
            foreach (var item in _currentOrderItems)
            {
                listOrder.Items.Add($"{item.ProductName}*{item.Quantity} @ {item.UnitPrice:N2} = {item.Subtotal:N2}");
            }
            double totalOrderAmount = _currentOrderItems.Sum(item => item.Subtotal);
            labelTotalAmount.Text = totalOrderAmount.ToString("#,##0.00");
            _dTotalAmount = totalOrderAmount;
        }

        //เมธอดนี้สำหรับกำหนดสถานะของปุ่มว่าให้สามารถใช้งานได้หรือไม่
        private void EnabledButton()
        {
            if (labelMember.Text != "")
            {
                buttonConfirmOrder.Enabled = true;
            }
            else
            {
                buttonConfirmOrder.Enabled = false;
            }
        }

        //เมธอดนี้กำหนดชนิดพารามิเตอร์เป็น TextBox เพื่อให้สามารถตรวจสอบกับ TextBox อันใดก็ได้
        
        private void textGotAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonCalChange_Click(object sender, EventArgs e)
        {

        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {

        }

        private void formMyCoffee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.E)
            {
                Close();
            }
            else
            {
                if (!(e.Control & e.KeyCode == Keys.L))
                    return;
            }
        }

        private void formMyCoffee_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            
        }

        private void formMyCoffee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("คุณต้องการปิดโปรแกรมใช่ไหม", "จบการทำงาน", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.No)
                return;
            e.Cancel = true;
        }

        private void จบการทำงานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolAddItem_Click(object sender, EventArgs e)
        {
            buttonAddItem_Click(sender, e);
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ลบรายการToolStripMenuItem.Click += new EventHandler(buttonRemoveItem_Click);
            ลบรายการทงหมดToolStripMenuItem.Click += new EventHandler(buttonClearItem_Click);
            คำนวณToolStripMenuItem.Click += new EventHandler(buttonCalTotalAmount_Click);
            ออกจากระบบToolStripMenuItem.Click += new EventHandler(buttonLogout_Click);
            
        }

        private void เพมรายการToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonAddItem_Click(sender, e);
        }

        private void buttonConfirmOrder_Click(object sender, EventArgs e)
        {
            if (_currentOrderItems.Count > 0)
            {
                double totalAmountOrder = _currentOrderItems.Sum(item => item.Subtotal);
                int orderId = GenerateNextOrderID();
                int selectedPaymentId = comboBoxSelect.SelectedIndex +1 ;
                string tax = $"TAX-{DateTime.Now.Year}-{orderId:D4}";


                try
                {

                    using (SqlTransaction transaction = testConnection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert ข้อมูลลงใน Table Order
                            string insertOrderQuery = "INSERT INTO [Orders] VALUES (@OrderID,@MemberID, @DateBuy, @Tax, @PaymentID, @QuantitySell,@ProductID);";
                            SqlCommand insertOrderCommand = new SqlCommand(insertOrderQuery, testConnection, transaction);

                            foreach (var item in _currentOrderItems)
                            {
                                insertOrderCommand.Parameters.Clear();
                                insertOrderCommand.Parameters.AddWithValue("@OrderID", orderId);
                                insertOrderCommand.Parameters.AddWithValue("@MemberID", memberID);
                                insertOrderCommand.Parameters.AddWithValue("@DateBuy", DateTime.Now);
                                insertOrderCommand.Parameters.AddWithValue("@Tax", tax); // สมมติว่ามีฟังก์ชัน CalculateTax()
                                insertOrderCommand.Parameters.AddWithValue("@PaymentID", selectedPaymentId); // สมมติว่ามีฟังก์ชัน GetSelectedPaymentID()
                                insertOrderCommand.Parameters.AddWithValue("@QuantitySell", item.Quantity);
                                insertOrderCommand.Parameters.AddWithValue("@ProductID", item.ProductID);
                                insertOrderCommand.ExecuteNonQuery();
                            }
                            // 3. Update stock in TesterTutorial table
                            string updateStockQuery = "UPDATE Products SET Stock = Stock - @Quantity WHERE ProductName = @ProductName";
                            SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, testConnection, transaction);

                            foreach (var item in _currentOrderItems)
                            {
                                updateStockCommand.Parameters.Clear();
                                updateStockCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                                updateStockCommand.Parameters.AddWithValue("@ProductName", item.ProductName);
                                updateStockCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show($"บันทึกคำสั่งซื้อหมายเลข {orderId} สำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            buttonClearItem.Click += buttonClearItem_Click;
                            _currentOrderItems.Clear();
                            buttonClearItem_Click(sender, e);
                            UpdateOrderListBox();
                            LoadProductDataTable(); // Reload to update stock in UI
                            

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึกคำสั่งซื้อ: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
            else
            {
                MessageBox.Show("ไม่มีรายการสินค้าในคำสั่งซื้อ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void สมครสมาชกToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditMem formEditMem = new FormEditMem();
            formEditMem.MemberID = 0;
            formEditMem.ShowDialog();
            Logout();
        }

        private void เขาสToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login();
        }
        private void Login()
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                    Encoding.GetEncoding("Windows-874"));

                logConnection = new SqlConnection(strConnectionString);
                logConnection.Open();
                logCommand = new SqlCommand("Select * from Member", logConnection);
                logAdapter = new SqlDataAdapter();
                logAdapter.SelectCommand = logCommand;
                logTable = new DataTable();
                logAdapter.Fill(logTable);
                List<string> Phonenum = new List<string>();
                List<string> MemberName = new List<string>();
                foreach (DataRow row in logTable.Rows)
                {
                    if (row.Table.Columns.Contains("Telephone") && row.Table.Columns.Contains("FirstName"))
                    {
                        string phone = row["Telephone"].ToString().Trim();
                        string firstName = row["FirstName"].ToString().Trim();
                        string ID = row["MemberID"].ToString().Trim();
                        _memberNames[phone] = firstName;
                        _memberID[phone] = ID;
                    }
                }
            }
            catch(Exception ex)  
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

            bool flag = false;
            while (!flag)
            {
                string user_phone = Interaction.InputBox("กรุณากรอกเบอร์ผู้ใช้งาน", "เบอร์ผู้ใช้งาน");
                if (user_phone != "")
                {
                    if (_memberNames.ContainsKey(user_phone))
                    {
                        // พบเบอร์โทรศัพท์ในระบบ
                        _loggedInMemberName = _memberNames[user_phone];
                        memberID = Convert.ToInt32(_memberID[user_phone]);
                        labelMember.Text = $"ยินดีต้อนรับคุณ {_loggedInMemberName}";
                        flag = true;
                        // คุณอาจต้องการเปิดใช้งานส่วนอื่นๆ ของ Form หรือเปลี่ยนไป Form อื่นที่นี่
                    }
                    else
                    {
                        MessageBox.Show($"ไม่พบเบอร์โทรศัพท์ '{user_phone}' ในระบบของเรา", "ไม่พบในระบบ!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        // วนลูปเพื่อให้ผู้ใช้กรอกใหม่อีกครั้ง
                    }
                }
                else
                {
                    // ผู้ใช้ยกเลิกการกรอกเบอร์โทรศัพท์
                    flag = true;
                    Close();
                }
            }
            UpdateOrderListBox();
            EnabledButton();
        }

        private void Logout()
        {
            _loggedInMemberName = ""; // ล้างชื่อสมาชิกที่ล็อกอิน
            memberID = 0; // รีเซ็ต MemberID
            labelMember.Text = ""; // ล้างข้อความต้อนรับบน Label
            EnabledButton();
        }
        private int GenerateNextOrderID()
        {
            int nextOrderID = 1; // ค่าเริ่มต้นถ้ายังไม่มี Order ใน Table

            try
            {
                string selectMaxIDQuery = "SELECT ISNULL(MAX(OrderID), 0) FROM [Orders]";
                SqlCommand selectMaxIDCommand = new SqlCommand(selectMaxIDQuery, testConnection);
                int maxOrderID = Convert.ToInt32(selectMaxIDCommand.ExecuteScalar());
                nextOrderID = maxOrderID + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการสร้าง OrderID: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return nextOrderID;
        }

        private async void LoadAndDisplayDiscount(int promotionID)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName,
                                                                        Encoding.GetEncoding("Windows-874"));

                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT Discount FROM Promotion WHERE PromotionID = @PromotionID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PromotionID", promotionID);
                        object result = await command.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            if (double.TryParse(result.ToString(), out double discount))
                            {
                                labelDiscount.Text = $"ส่วนลด: {discount} %";
                                _currentProductDiscount = discount;
                            }
                            else
                            {
                                labelDiscount.Text = "ข้อมูลส่วนลดไม่ถูกต้อง";
                                _currentProductDiscount = 0;
                            }
                        }
                        else
                        {
                            labelDiscount.Text = "ไม่มีส่วนลด";
                            _currentProductDiscount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดส่วนลด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelDiscount.Text = "เกิดข้อผิดพลาด";
                _currentProductDiscount = 0;
            }
        }

        private void buttonCanSearch_Click(object sender, EventArgs e)
        {
            LoadProductDataTable();
            textNameFind.Text = "";
            comboCategory.SelectedIndex = -1;
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (comboCategory.SelectedValue != null)
            {
                if (textNameFind.Text == "")
                {
                    try
                    {
                        string strConnectionString = "";
                        if (File.Exists(strConnStrFileName))
                            strConnectionString = File.ReadAllText(strConnStrFileName, Encoding.GetEncoding("Windows-874"));

                        using (SqlConnection connection = new SqlConnection(strConnectionString))
                        {
                            connection.Open();
                            string selectedCategoryName = comboCategory.SelectedValue.ToString();
                            string query = "SELECT * FROM Products WHERE Category_ID IN (SELECT Category_ID FROM Category WHERE Category_Name = @CategoryName) AND ProductPrice BETWEEN @Low AND @High";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                if (decimal.TryParse(LowestPiece.Text, out decimal lowestPrice) &&
                                    decimal.TryParse(HighestPiece.Text, out decimal highestPrice))
                                {
                                    command.Parameters.AddWithValue("@CategoryName", selectedCategoryName);
                                    command.Parameters.AddWithValue("@Low", lowestPrice);
                                    command.Parameters.AddWithValue("@High", highestPrice);

                                    Newdapter = new SqlDataAdapter(command);
                                    NewTable = new DataTable();
                                    Newdapter.Fill(NewTable);

                                    LoadProductData(NewTable);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาดในการโหลด Products: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                else
                {
                    try
                    {
                        string strConnectionString = "";
                        if (File.Exists(strConnStrFileName))
                            strConnectionString = File.ReadAllText(strConnStrFileName, Encoding.GetEncoding("Windows-874"));

                        using (SqlConnection connection = new SqlConnection(strConnectionString))
                        {
                            connection.Open();
                            string selectedCategoryName = comboCategory.SelectedValue.ToString();
                            string query = "SELECT * FROM Products WHERE Category_ID IN (SELECT Category_ID FROM Category WHERE Category_Name = @CategoryName) AND ProductPrice BETWEEN @Low AND @High AND ProductName LIKE @ProductName";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                if (decimal.TryParse(LowestPiece.Text, out decimal lowestPrice) &&
                                    decimal.TryParse(HighestPiece.Text, out decimal highestPrice))
                                {
                                    command.Parameters.AddWithValue("@CategoryName", selectedCategoryName);
                                    command.Parameters.AddWithValue("@Low", lowestPrice);
                                    command.Parameters.AddWithValue("@High", highestPrice);
                                    command.Parameters.AddWithValue("@ProductName", "%" + textNameFind.Text + "%");

                                    Newdapter = new SqlDataAdapter(command);
                                    NewTable = new DataTable();
                                    Newdapter.Fill(NewTable);

                                    LoadProductData(NewTable);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาดในการโหลด Products: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            else
            {
                if (textNameFind.Text == "")
                {
                    try
                    {
                        string strConnectionString = "";
                        if (File.Exists(strConnStrFileName))
                            strConnectionString = File.ReadAllText(strConnStrFileName, Encoding.GetEncoding("Windows-874"));

                        using (SqlConnection connection = new SqlConnection(strConnectionString))
                        {
                            connection.Open();
                            string query = "SELECT * FROM Products WHERE ProductPrice BETWEEN @Low AND @High";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                if (decimal.TryParse(LowestPiece.Text, out decimal lowestPrice) &&
                                    decimal.TryParse(HighestPiece.Text, out decimal highestPrice))
                                {
                                    command.Parameters.AddWithValue("@Low", lowestPrice);
                                    command.Parameters.AddWithValue("@High", highestPrice);

                                    Newdapter = new SqlDataAdapter(command);
                                    NewTable = new DataTable();
                                    Newdapter.Fill(NewTable);

                                    LoadProductData(NewTable);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาดในการโหลด Products: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        string strConnectionString = "";
                        if (File.Exists(strConnStrFileName))
                            strConnectionString = File.ReadAllText(strConnStrFileName, Encoding.GetEncoding("Windows-874"));

                        using (SqlConnection connection = new SqlConnection(strConnectionString))
                        {
                            connection.Open();
                            string query = "SELECT * FROM Products WHERE ProductPrice BETWEEN @Low AND @High AND ProductName LIKE @ProductName";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                if (decimal.TryParse(LowestPiece.Text, out decimal lowestPrice) &&
                                    decimal.TryParse(HighestPiece.Text, out decimal highestPrice))
                                {
                                    command.Parameters.AddWithValue("@Low", lowestPrice);
                                    command.Parameters.AddWithValue("@High", highestPrice);
                                    command.Parameters.AddWithValue("@ProductName", "%" + textNameFind.Text + "%");

                                    Newdapter = new SqlDataAdapter(command);
                                    NewTable = new DataTable();
                                    Newdapter.Fill(NewTable);

                                    LoadProductData(NewTable);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาดในการโหลด Products: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
        }

        private void buttonPromotion_Click(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = "";
                if (File.Exists(strConnStrFileName))
                    strConnectionString = File.ReadAllText(strConnStrFileName, Encoding.GetEncoding("Windows-874"));

                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Products WHERE PromotionID IS NOT NULL ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                            Newdapter = new SqlDataAdapter(command);
                            NewTable = new DataTable();
                            Newdapter.Fill(NewTable);

                            LoadProductData(NewTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลด Products: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void แกไขขอมลToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_loggedInMemberName == "")
            {
                MessageBox.Show("กรุณาเข้าสู่ระบบก่อน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FormEditMem formEditMem = new FormEditMem();
                formEditMem.MemberID = memberID;
                formEditMem.ShowDialog();
                Logout();
            }

        }

        private void ออกจากระบบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void ตรวจสอบOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOrder formOrder = new FormOrder();
            formOrder.MemberID = memberID;
            formOrder.ShowDialog();
        }
    }
}

