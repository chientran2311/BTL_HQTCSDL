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
    public partial class login2 : Form
    {
        public login2()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Chuỗi kết nối tới cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Lấy giá trị từ các TextBox
            string username = txtusername.Text.Trim();
            string password = txtpassword.Text.Trim();

            // Kiểm tra nếu một trong hai ô trống
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    // Tạo câu lệnh SQL để kiểm tra tài khoản và mật khẩu
                    string query = "SELECT maNhanvien, quyenQuanTri FROM TaiKhoan WHERE tenTaiKhoan = @username AND matKhau = @password";

                    using (SqlCommand command = new SqlCommand(query, Sqlcon))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        // Mở kết nối đến cơ sở dữ liệu
                        Sqlcon.Open();

                        // Thực thi câu lệnh và lấy dữ liệu
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            // Đọc dữ liệu từ cơ sở dữ liệu
                            reader.Read();

                            int maNhanvien = Convert.ToInt32(reader["maNhanvien"]);
                            int quyenQuanTri = Convert.ToInt32(reader["quyenQuanTri"]);

                            // Kiểm tra quyền
                            if (quyenQuanTri == 1)
                            {
                                // Nếu là quản trị viên, mở form Dashboard
                                dashboard frmDashboard = new dashboard();
                                frmDashboard.Show();
                            }
                            else if (quyenQuanTri == 0)
                            {
                                // Nếu là nhân viên bán hàng, mở form Sale
                                salesforemploy frmSale = new salesforemploy();
                                frmSale.Show();
                            }

                            // Đóng form login hoặc ẩn form hiện tại nếu cần
                            this.Hide(); // Hoặc sử dụng this.Close() nếu không muốn form cũ hiển thị lại
                        }
                        else
                        {
                            // Nếu không tìm thấy tài khoản và mật khẩu trùng khớp
                            MessageBox.Show("Tài khoản hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Xử lý lỗi SQL
                MessageBox.Show("Đã xảy ra lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
