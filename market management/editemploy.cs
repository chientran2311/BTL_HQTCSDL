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
    public partial class editemploy : Form
    {
        public editemploy()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy các giá trị từ các TextBox và RadioButton
                string maNhanVien = txtMaNhanvien.Text;
                string tenNhanVien = txtTennhanvien.Text;
                string soDienThoai = txtsodienthoai.Text;
                string email = Txtemail.Text;
                string gioiTinh = radio_nam.Checked ? "Nam" : "Nữ"; // Lấy giới tính từ radio button
                decimal luong = 0;
                if (!decimal.TryParse(txtLuong.Text, out luong))
                {
                    MessageBox.Show("Lương không hợp lệ. Vui lòng nhập lại.");
                    return; // Nếu lương không hợp lệ, dừng lại và thông báo lỗi
                }
                string diaChi = txtdiachi.Text;

                // Xác nhận rằng mã nhân viên không trống
                if (string.IsNullOrWhiteSpace(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không thể trống!");
                    return;
                }

                // Chuỗi kết nối tới cơ sở dữ liệu
                string connectionString = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

                // Câu lệnh SQL để cập nhật thông tin nhân viên
                string updateQuery = @"
            UPDATE NhanVien
            SET 
                tenNhanvien = @tenNhanVien,
                soDienThoai = @soDienThoai,
                email = @Email,
                gioiTinh = @GioiTinh,
                Luong = @Luong,
                diaChi = @DiaChi
            WHERE maNhanvien = @MaNhanVien";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Mở kết nối tới cơ sở dữ liệu
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        // Thêm các tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd.Parameters.AddWithValue("@tenNhanVien", tenNhanVien);
                        cmd.Parameters.AddWithValue("@soDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@Luong", luong);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);

                        // Thực thi câu lệnh cập nhật
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thông tin nhân viên đã được cập nhật thành công.");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên với mã đã nhập.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetTextBoxValues(string maNhanVien, string tenNhanVien, string soDienThoai, string email, string gioiTinh, decimal luong, string diaChi)
        {
            // Gán giá trị cho các TextBox
            txtMaNhanvien.Text = maNhanVien;
            txtTennhanvien.Text = tenNhanVien;
            txtsodienthoai.Text = soDienThoai;
            Txtemail.Text = email;
            txtLuong.Text = luong.ToString();
            txtdiachi.Text = diaChi;

            // Chọn radio button theo giới tính
            if (gioiTinh != null)
            {
                if (gioiTinh.Equals("Nam", StringComparison.OrdinalIgnoreCase) || gioiTinh.Equals("nam", StringComparison.OrdinalIgnoreCase))
                {
                    radio_nam.Checked = true;
                    radio_nu.Checked = false;
                }
                else if (gioiTinh.Equals("Nữ", StringComparison.OrdinalIgnoreCase) || gioiTinh.Equals("nữ", StringComparison.OrdinalIgnoreCase))
                {
                    radio_nu.Checked = true;
                    radio_nam.Checked = false;
                }
            }
        }

        private void editemploy_Load(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã nhân viên từ textbox
                string maNhanVien = txtMaNhanvien.Text;

                // Kiểm tra nếu mã nhân viên rỗng hoặc null
                if (string.IsNullOrWhiteSpace(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không thể trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận người dùng có chắc chắn muốn xóa không
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                {
                    return; // Nếu người dùng không chọn Yes, thoát
                }

                // Chuỗi kết nối với cơ sở dữ liệu
                string connectionString = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

                // Câu lệnh SQL để xóa nhân viên
                string deleteQuery = @"
            DELETE FROM NhanVien
            WHERE maNhanvien = @MaNhanVien";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Mở kết nối với cơ sở dữ liệu
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                        // Thực thi câu lệnh xóa
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Nhân viên đã được xóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên với mã đã nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra trong quá trình kết nối và thực thi câu lệnh SQL
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
