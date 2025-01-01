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
    public partial class addEmploy : Form
    {
        public addEmploy()
        {
            InitializeComponent();
        }

        private void LuuEmploy_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ các TextBox
            string tenNhanVien = txtTennv.Text.Trim();
            string soDienThoai = txtsdtnv.Text.Trim();
            string email = txtemailnv.Text.Trim();
            string diaChi = txtdiachi.Text.Trim();
            string gioiTinh = GetGioiTinh();
            float luong;

            // Kiểm tra lương có phải là số hợp lệ
            if (!float.TryParse(txtLuong.Text.Trim(), out luong))
            {
                MessageBox.Show("Vui lòng nhập lương hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuỗi kết nối
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    Sqlcon.Open();

                    // Câu lệnh INSERT
                    string query = "INSERT INTO NhanVien (tenNhanvien, soDienThoai, email, gioiTinh, Luong, diaChi) " +
                                   "VALUES (@tenNhanvien, @soDienThoai, @email, @gioiTinh, @Luong, @diaChi)";

                    using (SqlCommand cmd = new SqlCommand(query, Sqlcon))
                    {
                        // Gán giá trị tham số
                        cmd.Parameters.AddWithValue("@tenNhanvien", tenNhanVien);
                        cmd.Parameters.AddWithValue("@soDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@gioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@Luong", luong);
                        cmd.Parameters.AddWithValue("@diaChi", diaChi);

                        // Thực thi lệnh
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm nhân viên. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        // Hàm để lấy giá trị giới tính từ RadioButton
        private string GetGioiTinh()
        {
            if (radio_nam.Checked)
            {
                return "Nam";
            }
            else if (radio_nu.Checked)
            {
                return "Nữ";
            }
            else
            {
                return null; // Không chọn
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
