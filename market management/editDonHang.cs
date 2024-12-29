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
using static market_management.receipt;

namespace market_management
{
    public partial class editDonHang : Form
    {
        public editDonHang()
        {
            InitializeComponent();
        }
        // Thay đổi phạm vi của các điều khiển thành public


        // Hoặc bạn có thể tạo các phương thức setter
        public void SetTextBoxValues(string ma, string ten, string thanhTien, string hinhThucThanhToan, string ghiChu, int maDonHang)
        {
            txtMa.Text = ma;
            txtTen.Text = ten;
            txtThanhtien.Text = thanhTien;
            cb_hinhthucthanhtoan.Text = hinhThucThanhToan;
            txtghichu.Text = ghiChu;

            // Retrieve additional information (quantity and price) from the database
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    Sqlcon.Open();

                    // Query to get quantity and price from SP_DonHang and SanPham
                    string query = @"
                SELECT SPD.soLuong, SP.giaBanSanPham
                FROM SP_DonHang SPD
                INNER JOIN SanPham SP ON SPD.maSanPham = SP.maSanPham
                WHERE SPD.maDonHang = @maDonHang";

                    using (SqlCommand command = new SqlCommand(query, Sqlcon))
                    {
                        // Add parameter for maDonHang
                        command.Parameters.AddWithValue("@maDonHang", maDonHang);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fill the additional TextBoxes with the fetched values
                                txtSoLuong.Text = reader["soLuong"].ToString();
                                txtgia.Text = reader["giaBanSanPham"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Product information not found for this order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving product information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadComboBoxes()
        {
            // Chuỗi kết nối
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    Sqlcon.Open();

                    // Tải danh sách nhân viên vào ComboBox cb_nhanvien
                    using (SqlCommand commandNhanVien = new SqlCommand("SELECT maNhanvien, tenNhanvien FROM NhanVien", Sqlcon))
                    {
                        using (SqlDataReader reader = commandNhanVien.ExecuteReader())
                        {
                            // Dọn sạch dữ liệu cũ
                            cb_nhanvien.Items.Clear();

                            while (reader.Read())
                            {
                                var item = new ComboBoxItem
                                {
                                    Value = reader["maNhanvien"].ToString(),
                                    Text = reader["tenNhanvien"].ToString()
                                };
                                cb_nhanvien.Items.Add(item);
                            }
                        }
                    }

                    // Tải danh sách công ty vào ComboBox cb_congty
                    using (SqlCommand commandCongTy = new SqlCommand("SELECT maCongTy, tenCongTy FROM CongTyGiaoHang", Sqlcon))
                    {
                        using (SqlDataReader reader = commandCongTy.ExecuteReader())
                        {
                            // Dọn sạch dữ liệu cũ
                            cb_congty.Items.Clear();

                            while (reader.Read())
                            {
                                var item = new ComboBoxItem
                                {
                                    Value = reader["maCongTy"].ToString(),
                                    Text = reader["tenCongTy"].ToString()
                                };
                                cb_congty.Items.Add(item);
                            }
                        }
                    }

                    // Tải danh sách hình thức thanh toán vào ComboBox cb_hinhthucthanhtoan
                    using (SqlCommand commandHinhThucThanhToan = new SqlCommand("SELECT DISTINCT hinhThucThanhToan FROM DonHang", Sqlcon))
                    {
                        using (SqlDataReader reader = commandHinhThucThanhToan.ExecuteReader())
                        {
                            // Dọn sạch dữ liệu cũ
                            cb_hinhthucthanhtoan.Items.Clear();

                            while (reader.Read())
                            {
                                // Thêm từng hình thức thanh toán vào ComboBox
                                cb_hinhthucthanhtoan.Items.Add(reader["hinhThucThanhToan"].ToString());
                            }
                        }
                    }

                    // Tải danh sách sản phẩm vào ComboBox cb_sanpham
               
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadSanPhamByMaDonHang()
        {
            // Chuỗi kết nối
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                // Lấy mã đơn hàng từ TextBox txtMa
                int maDonHang = int.Parse(txtMa.Text); // Đảm bảo txtMa có giá trị hợp lệ

                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    Sqlcon.Open();

                    // Truy vấn để lấy tên sản phẩm theo mã đơn hàng
                    string query = @"
                SELECT SP.maSanPham, SP.tenSanPham
                FROM SanPham SP
                INNER JOIN SP_DonHang SPD ON SP.maSanPham = SPD.maSanPham
                WHERE SPD.maDonHang = @MaDonHang";

                    using (SqlCommand commandSanPham = new SqlCommand(query, Sqlcon))
                    {
                        // Thêm tham số mã đơn hàng vào câu lệnh SQL
                        commandSanPham.Parameters.AddWithValue("@MaDonHang", maDonHang);

                        using (SqlDataReader reader = commandSanPham.ExecuteReader())
                        {
                            // Dọn sạch dữ liệu cũ trong ComboBox
                            cb_sanpham.Items.Clear();

                            while (reader.Read())
                            {
                                var item = new ComboBoxItem
                                {
                                    Value = reader["maSanPham"].ToString(),
                                    Text = reader["tenSanPham"].ToString()
                                };
                                cb_sanpham.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class ComboBoxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void editDonHang_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            LoadSanPhamByMaDonHang();
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            calculator();
        }

        private void txtgia_TextChanged(object sender, EventArgs e)
        {
            calculator();
        }

        public void calculator()
        {
            // Check if both fields have valid numeric values
            float a, b;

            // Try to parse the values from the textboxes
            if (float.TryParse(txtSoLuong.Text, out a) && float.TryParse(txtgia.Text, out b))
            {
                // Calculate the total price
                float total = a * b;

                // Update the txtThanhtien with the calculated value
                txtThanhtien.Text = total.ToString();
            }
            else
            {
                // If input is invalid, clear the txtThanhtien or set it to 0
                txtThanhtien.Text = "0";
            }
        }

        private void cb_sanpham_SelectedValueChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có lựa chọn sản phẩm không
            if (cb_sanpham.SelectedItem != null)
            {
                // Lấy tên sản phẩm từ ComboBox
                string tenSanPham = ((ComboBoxItem)cb_sanpham.SelectedItem).Text;

                // Lấy mã đơn hàng từ TextBox
                int maDonHang = int.Parse(txtMa.Text);

                // Gọi stored procedure để lấy giá và số lượng
                LayGiaVaSoLuongSanPham(tenSanPham, maDonHang);
            }
        }

        private void LayGiaVaSoLuongSanPham(string tenSanPham, int maDonHang)
        {
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LayGiaTienVaSoLuongSP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào Stored Procedure
                        cmd.Parameters.AddWithValue("@TenSanPham", tenSanPham);
                        cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);

                        // Mở kết nối
                        con.Open();

                        // Thực thi stored procedure và lấy kết quả trả về
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Kiểm tra nếu có dữ liệu trả về
                            if (reader.Read())
                            {
                                // Lấy giá trị và cập nhật vào các TextBox tương ứng
                                txtgia.Text = reader["GiaTien"].ToString();
                                txtSoLuong.Text = reader["SoLuong"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy thông tin sản phẩm trong đơn hàng.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
