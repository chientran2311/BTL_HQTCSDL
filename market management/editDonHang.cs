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
            txtMadonhang.Text = ma;

            TxtThanhtien.Text = thanhTien;
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

                   

                    // Tải danh sách công ty vào ComboBox cb_congty
                  

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
                int maDonHang = int.Parse(txtMadonhang.Text); // Đảm bảo txtMa có giá trị hợp lệ

                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    Sqlcon.Open();

                    // Truy vấn để lấy mã và tên sản phẩm theo mã đơn hàng
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

                            // Đọc từng sản phẩm từ kết quả truy vấn
                            while (reader.Read())
                            {
                                var item = new ComboBoxItem
                                {
                                    Value = reader["maSanPham"].ToString(),
                                    Text = reader["tenSanPham"].ToString()
                                };

                                // Thêm sản phẩm vào ComboBox
                                cb_sanpham.Items.Add(item);
                            }

                            // Kiểm tra nếu có sản phẩm trong ComboBox
                            if (cb_sanpham.Items.Count > 0)
                            {
                                // Chọn sản phẩm đầu tiên trong ComboBox
                                cb_sanpham.SelectedIndex = 0;

                                // Cập nhật mã sản phẩm vào txtMasanpham khi chọn sản phẩm
                                ComboBoxItem selectedItem = (ComboBoxItem)cb_sanpham.SelectedItem;
                                txtMasanpham.Text = selectedItem.Value.ToString();  // Cập nhật mã sản phẩm vào txtMasanpham
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
            int a; // Số lượng là kiểu int
            decimal b, total; // Giá bán và tổng tiền đều là kiểu decimal

            // Try to parse the values from the textboxes as int for quantity and decimal for price
            if (int.TryParse(txtSoLuong.Text, out a) && decimal.TryParse(txtgia.Text, out b))
            {
                // Calculate the total price
                total = a * b; // Tính tổng tiền

                // Update the txtThanhtien with the calculated value
                TxtThanhtien.Text = total.ToString("0.##"); // Hiển thị giá trị với 2 chữ số thập phân (nếu có)
            }
            else
            {
                // If input is invalid, clear the txtThanhtien or set it to 0
                TxtThanhtien.Text = "0";
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
                int maDonHang = int.Parse(txtMadonhang.Text);

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
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Chuỗi kết nối
            string connectionString = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Lấy các giá trị từ các TextBox, ComboBox, RichTextBox
            int maDonHang = Convert.ToInt32(txtMadonhang.Text);
            int maSanPham = Convert.ToInt32(txtMasanpham.Text);
            int soLuong = Convert.ToInt32(txtSoLuong.Text);
            string hinhThucThanhToan = cb_hinhthucthanhtoan.SelectedItem.ToString();
            string ghiChu = txtghichu.Text;
            long thanhTien = Convert.ToInt64(TxtThanhtien.Text);

            string updateSPDonHangQuery = @"
        UPDATE SP_DonHang
        SET soLuong = @soLuong
        WHERE maDonHang = @maDonHang AND maSanPham = @maSanPham;
    ";

            string updateDonHangQuery = @"
        UPDATE DonHang
        SET hinhThucThanhToan = @hinhThucThanhToan, ghiChu = @ghiChu, thanhTien = @thanhTien
        WHERE maDonHang = @maDonHang;
    ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Cập nhật SP_DonHang
                    using (SqlCommand cmdSPDonHang = new SqlCommand(updateSPDonHangQuery, conn))
                    {
                        cmdSPDonHang.Parameters.AddWithValue("@maDonHang", maDonHang);
                        cmdSPDonHang.Parameters.AddWithValue("@maSanPham", maSanPham);
                        cmdSPDonHang.Parameters.AddWithValue("@soLuong", soLuong);

                        int rowsAffectedSP = cmdSPDonHang.ExecuteNonQuery();
                        if (rowsAffectedSP == 0)
                        {
                            MessageBox.Show("Không tìm thấy sản phẩm với mã đơn hàng và sản phẩm đã cho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Cập nhật DonHang
                    using (SqlCommand cmdDonHang = new SqlCommand(updateDonHangQuery, conn))
                    {
                        cmdDonHang.Parameters.AddWithValue("@maDonHang", maDonHang);
                        cmdDonHang.Parameters.AddWithValue("@hinhThucThanhToan", hinhThucThanhToan);
                        cmdDonHang.Parameters.AddWithValue("@ghiChu", ghiChu);
                        cmdDonHang.Parameters.AddWithValue("@thanhTien", thanhTien);

                        int rowsAffectedDH = cmdDonHang.ExecuteNonQuery();
                        if (rowsAffectedDH == 0)
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng với mã đã cho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnXoahang_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã đơn hàng từ TextBox
                string maDonHang = txtMadonhang.Text;

                // Kiểm tra nếu mã đơn hàng rỗng hoặc null
                if (string.IsNullOrWhiteSpace(maDonHang))
                {
                    MessageBox.Show("Mã đơn hàng không thể trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận người dùng có chắc chắn muốn xóa đơn hàng không
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                {
                    return; // Nếu người dùng không chọn Yes, thoát
                }

                // Chuỗi kết nối với cơ sở dữ liệu
                string connectionString = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

                // Câu lệnh SQL để xóa đơn hàng
                string deleteQuery = @"
            DELETE FROM DonHang
            WHERE maDonHang = @MaDonHang";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Mở kết nối với cơ sở dữ liệu
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);

                        // Thực thi câu lệnh xóa
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đơn hàng đã được xóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng với mã đã nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
