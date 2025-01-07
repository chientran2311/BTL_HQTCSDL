using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace market_management
{
    public partial class Form_Them : Form
    {
        public Form_Them()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLBHDataSet2.SanPham' table. You can move, or remove it, as needed.
            this.sanPhamTableAdapter.Fill(this.qLBHDataSet2.SanPham);
            // TODO: This line of code loads data into the 'qLBHDataSet2.KhoHang' table. You can move, or remove it, as needed.
            this.khoHangTableAdapter.Fill(this.qLBHDataSet2.KhoHang);

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn đủ các giá trị chưa
            if (guna2ComboBox1.SelectedItem == null || guna2ComboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select items from both ComboBoxes!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy dữ liệu từ các ComboBox
                DataRowView selectedRow1 = (DataRowView)guna2ComboBox1.SelectedItem;
                DataRowView selectedRow2 = (DataRowView)guna2ComboBox2.SelectedItem;
                var maKhoHang = selectedRow1["maKhoHang"];
                var tenSanPham = selectedRow2["tenSanPham"];
                var ngayNhap = DateTime.Now;

                // Lấy số lượng từ TextBox
                int soLuong;
                if (int.TryParse(guna2TextBox1.Text, out soLuong))
                {
                    // Mở kết nối và gọi Stored Procedure để thêm dữ liệu
                    using (SqlConnection conn = new SqlConnection("Data Source=HUY;Initial Catalog=QLBH;Integrated Security=True;"))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand("sp_InsertKhoHang", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ngayNhap", ngayNhap);
                            cmd.Parameters.AddWithValue("@tenSanPham", tenSanPham);
                            cmd.Parameters.AddWithValue("@soLuong", soLuong);
                            cmd.Parameters.AddWithValue("@maKhoHang", maKhoHang);

                            // Thực thi Stored Procedure
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Product successfully added to the warehouse!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Làm mới dữ liệu trong DataGridView ở Warehouse.cs
                    // Tạo đối tượng Warehouse và gọi phương thức RefreshDataGridView
                    if (Application.OpenForms["Warehouse"] is warehouse warehouseForm)
                    {
                        warehouseForm.RefreshDataGridView();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for Quantity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
    

