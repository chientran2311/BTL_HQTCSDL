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

namespace market_management
{
    public partial class employee : Form
    {
        public employee()
        {
            InitializeComponent();
        }

        private void employee_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void txtsdt_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtsdt_TextChanged(object sender, EventArgs e)
        {
            // Chuỗi kết nối đến cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    Sqlcon.Open();

                    // Lấy số điện thoại từ TextBox
                    string sdt = txtsdt.Text.Trim();

                    // Tạo câu lệnh SQL với điều kiện LIKE
                    string query = "SELECT * FROM NhanVien WHERE soDienThoai LIKE @sdt";

                    using (SqlCommand cmd = new SqlCommand(query, Sqlcon))
                    {
                        // Thêm tham số với giá trị tìm kiếm
                        cmd.Parameters.AddWithValue("@sdt", "%" + sdt + "%");

                        // Thực thi lệnh và lấy dữ liệu
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Tạo một DataTable để chứa dữ liệu
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Gán DataTable làm nguồn dữ liệu cho DataGridView
                            tableemploy.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Đã xảy ra lỗi kết nối với cơ sở dữ liệu!\nChi tiết: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            addEmploy addEmploy = new addEmploy();  
            addEmploy.Show();
        }

        private void tableemploy_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu có dòng được chọn trong DataGridView
                if (tableemploy.CurrentRow != null && tableemploy.CurrentRow.Index >= 0)
                {
                    // Lấy dòng được chọn
                    DataGridViewRow selectedRow = tableemploy.CurrentRow;

                    // Lấy dữ liệu từ các cột của dòng được chọn
                    string maNhanVien = selectedRow.Cells["maNhanvien"].Value?.ToString();
                    string tenNhanVien = selectedRow.Cells["tenNhanvien"].Value?.ToString();
                    string soDienThoai = selectedRow.Cells["soDienThoai"].Value?.ToString();
                    string email = selectedRow.Cells["email"].Value?.ToString();
                    string gioiTinh = selectedRow.Cells["gioiTinh"].Value?.ToString();
                    string luong = selectedRow.Cells["Luong"].Value?.ToString();
                    string diaChi = selectedRow.Cells["diaChi"].Value?.ToString();

                    // Chuyển đổi sang kiểu số nếu cần (ví dụ: lương)
                    decimal luongDecimal;
                    if (!decimal.TryParse(luong, out luongDecimal))
                    {
                        luongDecimal = 0;
                    }

                    // Tạo một instance của form editEmploy
                    editemploy editForm = new editemploy();

                    // Truyền giá trị vào các TextBox và RadioButton của form editEmploy
                    editForm.SetTextBoxValues(maNhanVien, tenNhanVien, soDienThoai, email, gioiTinh, luongDecimal, diaChi);

                    // Hiển thị form editEmploy
                    editForm.Show();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
