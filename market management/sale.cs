using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace market_management
{
    public partial class sale : Form
    {
        string connectionString = "Data Source=DESKTOP-UL53I9J;Initial Catalog=QLBH;Integrated Security=True;";

        int maKhachHang = 0;  // Lưu mã khách hàng
        int maDonHang = 0;    // Lưu mã đơn hàng

        public sale()
        {
            InitializeComponent();
            ToggleControls(false);
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT sp.maSanPham AS [Mã SP], 
                   sp.tenSanPham AS [Tên SP], 
                   spd.soLuong AS [Số lượng], 
                   sp.giaBanSanPham AS [Giá bán sản phẩm], 
                   (spd.soLuong * sp.giaBanSanPham) AS [Thành tiền]
            FROM SP_DonHang spd
            JOIN SanPham sp ON spd.maSanPham = sp.maSanPham
            WHERE spd.maDonHang = @maDonHang;";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@maDonHang", maDonHang);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvSP_DonHang.DataSource = dt;

                // Thêm cột Hành động nếu chưa có
                if (dgvSP_DonHang.Columns["Sửa"] == null)
                {
                    // Tạo nút Sửa
                    DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                    btnEdit.HeaderText = "Hành động";
                    btnEdit.Name = "Sửa";
                    btnEdit.Text = "Sửa";
                    btnEdit.UseColumnTextForButtonValue = true;
                    dgvSP_DonHang.Columns.Add(btnEdit);

                    // Tạo nút Xóa
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    btnDelete.HeaderText = "";
                    btnDelete.Name = "Xóa";
                    btnDelete.Text = "Xóa";
                    btnDelete.UseColumnTextForButtonValue = true;
                    dgvSP_DonHang.Columns.Add(btnDelete);
                }
            }
        }



        private void sale_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo người dùng bấm vào hàng hợp lệ
            {
                int maSanPham = Convert.ToInt32(dgvSP_DonHang.Rows[e.RowIndex].Cells["Mã SP"].Value);

                if (dgvSP_DonHang.Columns[e.ColumnIndex].Name == "Sửa") // Nút Sửa
                {
                    // Lấy số lượng mới từ ô số lượng trong DataGridView
                    int soLuongMoi = Convert.ToInt32(dgvSP_DonHang.Rows[e.RowIndex].Cells["Số lượng"].Value);

                    // Cập nhật số lượng sản phẩm trong cơ sở dữ liệu
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                    UPDATE SP_DonHang 
                    SET soLuong = @soLuong
                    WHERE maDonHang = @maDonHang AND maSanPham = @maSanPham";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@soLuong", soLuongMoi);
                        cmd.Parameters.AddWithValue("@maDonHang", maDonHang);
                        cmd.Parameters.AddWithValue("@maSanPham", maSanPham);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Cập nhật số lượng thành công!");
                    LoadData(); // Cập nhật lại dữ liệu trong DataGridView

                    // Tính lại tổng tiền sau khi sửa
                    TinhTongTien();
                }
                else if (dgvSP_DonHang.Columns[e.ColumnIndex].Name == "Xóa") // Nút Xóa
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = @"
                        DELETE FROM SP_DonHang 
                        WHERE maDonHang = @maDonHang AND maSanPham = @maSanPham";

                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@maDonHang", maDonHang);
                            cmd.Parameters.AddWithValue("@maSanPham", maSanPham);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Xóa sản phẩm thành công!");
                        LoadData(); // Cập nhật lại dữ liệu trong DataGridView

                        // Tính lại tổng tiền sau khi xóa
                        TinhTongTien();
                    }
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            dgvSP_DonHang.Rows.Clear();
            txtTenKhachHang.Clear();
            txtSDTKhachHangADD.Clear();
            txtSDTKhachHangSEARCH.Clear();
            txtEmailKhachHang.Clear();
            txtDiaChiKhachHang.Clear();
            txtGhiChuKhachHang.Clear();
            txtTongTien.Clear();
            txtMaKhuyenMai.Clear();
            txtMaNV.Clear();
            txtSDTKhachHang.Clear();
            txtGhiChuDonHang.Clear();
            txtMaSP.Clear();
        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }

        private void btnTaoDonHang_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Truy vấn lấy mã khách hàng
                string query = "SELECT maKhachhang FROM KhachHang WHERE soDienThoai = @soDienThoai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@soDienThoai", txtSDTKhachHangSEARCH.Text);

                object result = cmd.ExecuteScalar();

                // Kiểm tra nếu khách hàng không tồn tại
                if (result == null)
                {
                    MessageBox.Show("Khách hàng mới! Không thể tạo đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    maKhachHang = 0; // Đặt lại giá trị để ngăn chặn việc tiếp tục thêm đơn hàng
                    maDonHang = 0;
                    return; // Thoát khỏi hàm
                }

                // Nếu khách hàng tồn tại, lấy mã khách hàng
                maKhachHang = Convert.ToInt32(result);

                // Tạo đơn hàng mới
                string insertOrder = @"
                INSERT INTO DonHang (maKhachHang, thanhTien, ghiChu, maNhanVien, Ngaytaodon)
                VALUES (@maKhachHang, 0, N'Đơn hàng mới', 1, GETDATE());
                SELECT SCOPE_IDENTITY();";

                SqlCommand cmdOrder = new SqlCommand(insertOrder, conn);
                cmdOrder.Parameters.AddWithValue("@maKhachHang", maKhachHang);

                // Lấy mã đơn hàng vừa tạo
                maDonHang = Convert.ToInt32(cmdOrder.ExecuteScalar());

                MessageBox.Show("Đã tạo đơn hàng mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSDTKhachHang.Text = txtSDTKhachHangSEARCH.Text;
                ToggleControls(true);
            }
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra xem mã sản phẩm có tồn tại trong bảng SanPham không
                string checkProductQuery = @"
            SELECT COUNT(*) 
            FROM SanPham 
            WHERE maSanPham = @maSanPham";

                SqlCommand checkProductCmd = new SqlCommand(checkProductQuery, conn);
                checkProductCmd.Parameters.AddWithValue("@maSanPham", int.Parse(txtMaSP.Text));

                int productCount = (int)checkProductCmd.ExecuteScalar();

                if (productCount == 0)
                {
                    // Nếu mã sản phẩm không tồn tại
                    MessageBox.Show("Mã sản phẩm sai hoặc mã sản phẩm chưa được thêm vào cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Nếu mã sản phẩm hợp lệ, thực hiện thêm sản phẩm vào bảng SP_DonHang
                    string query = @"
                INSERT INTO SP_DonHang (maDonHang, maSanPham, soLuong)
                VALUES (@maDonHang, @maSanPham, 1);";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maDonHang", maDonHang);
                    cmd.Parameters.AddWithValue("@maSanPham", int.Parse(txtMaSP.Text));

                    cmd.ExecuteNonQuery();

                    LoadData(); // Tải lại dữ liệu lên DataGridView
                    TinhTongTien(); // Tính lại tổng tiền
                }
            }
        }


        private void ToggleControls(bool isEnabled)
        {
            // Tắt/bật các nút và ô nhập liệu
            txtMaSP.Enabled = isEnabled;
            btnThemSP.Enabled = isEnabled;
            dgvSP_DonHang.Enabled = isEnabled;


            // Tắt/bật từng dòng trong DataGridView
            foreach (DataGridViewRow row in dgvSP_DonHang.Rows)
            {
                row.Cells["SoLuong"].ReadOnly = !isEnabled; // Chỉ cho phép sửa số lượng khi đã tạo đơn
            }
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            try
            {
                // Khai báo và mở kết nối tới cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Lấy dữ liệu từ các trường nhập
                    string tenKhachHang = txtTenKhachHang.Text.Trim();
                    string sdtKhachHang = txtSDTKhachHangADD.Text.Trim();
                    string email = txtEmailKhachHang.Text.Trim();
                    string diaChi = txtDiaChiKhachHang.Text.Trim();
                    string gioiTinh = cbGioiTinhKhachHang.SelectedItem.ToString(); // Lấy giá trị từ ComboBox
                    string ghiChu = txtGhiChuKhachHang.Text.Trim();

                    // Kiểm tra ràng buộc dữ liệu
                    if (string.IsNullOrEmpty(tenKhachHang) || string.IsNullOrEmpty(sdtKhachHang) ||
                        string.IsNullOrEmpty(email) || string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(gioiTinh))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Truy vấn thêm khách hàng
                    string query = @"INSERT INTO KhachHang 
                            (tenKhachhang, soDienThoai, ghiChu, email, gioiTinh, diaChi) 
                            VALUES 
                            (@tenKhachHang, @sdt, @ghiChu, @email, @gioiTinh, @diaChi)";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@tenKhachHang", tenKhachHang);
                    cmd.Parameters.AddWithValue("@sdt", sdtKhachHang);
                    cmd.Parameters.AddWithValue("@ghiChu", ghiChu);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@gioiTinh", gioiTinh);
                    cmd.Parameters.AddWithValue("@diaChi", diaChi);

                    // Thực thi câu lệnh SQL
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearCustomerFields(); // Xóa dữ liệu sau khi thêm thành công
                    }
                    else
                    {
                        MessageBox.Show("Thêm khách hàng thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearCustomerFields()
        {
            txtTenKhachHang.Text = "";
            txtSDTKhachHangADD.Text = "";
            txtEmailKhachHang.Text = "";
            txtDiaChiKhachHang.Text = "";
            cbGioiTinhKhachHang.SelectedIndex = -1; // Reset ComboBox
            txtGhiChuKhachHang.Text = "";
        }

        private void btnLuuDonHang_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lấy id của mã khuyến mãi nếu có
                    int? maKhuyenMai = null; // Biến để lưu id của mã khuyến mãi
                    if (!string.IsNullOrEmpty(txtMaKhuyenMai.Text))
                    {
                        string queryMaKhuyenMai = @"
                    SELECT id
                    FROM KhuyenMai
                    WHERE maKhuyenMai = @maKhuyenMai;";

                        SqlCommand cmdMaKhuyenMai = new SqlCommand(queryMaKhuyenMai, conn);
                        cmdMaKhuyenMai.Parameters.AddWithValue("@maKhuyenMai", txtMaKhuyenMai.Text);

                        object result = cmdMaKhuyenMai.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            maKhuyenMai = Convert.ToInt32(result); // Gán id của mã khuyến mãi
                        }
                        else
                        {
                            MessageBox.Show("Mã khuyến mãi không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Tính tổng tiền của đơn hàng
                    string queryTongTien = @"
                SELECT SUM(spd.soLuong * sp.giaBanSanPham)
                FROM SP_DonHang spd
                JOIN SanPham sp ON spd.maSanPham = sp.maSanPham
                WHERE spd.maDonHang = @maDonHang;";

                    SqlCommand cmdTongTien = new SqlCommand(queryTongTien, conn);
                    cmdTongTien.Parameters.AddWithValue("@maDonHang", maDonHang);
                    object resultTongTien = cmdTongTien.ExecuteScalar();

                    decimal tongTien = resultTongTien != DBNull.Value ? Convert.ToDecimal(resultTongTien) : 0;

                    // Cập nhật thông tin đơn hàng
                    string queryUpdate = @"
                UPDATE DonHang
                SET thanhTien = @tongTien,
                    ghiChu = @ghiChu,
                    maKhuyenMai = @maKhuyenMai,
                    maNhanVien = @maNhanVien
                WHERE maDonHang = @maDonHang;";

                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                    cmdUpdate.Parameters.AddWithValue("@tongTien", tongTien);
                    cmdUpdate.Parameters.AddWithValue("@ghiChu", txtGhiChuDonHang.Text);
                    cmdUpdate.Parameters.AddWithValue("@maKhuyenMai", maKhuyenMai.HasValue ? (object)maKhuyenMai.Value : DBNull.Value);
                    cmdUpdate.Parameters.AddWithValue("@maNhanVien", string.IsNullOrEmpty(txtMaNV.Text) ? (object)DBNull.Value : txtMaNV.Text);
                    cmdUpdate.Parameters.AddWithValue("@maDonHang", maDonHang);

                    int rowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Làm mới danh sách sản phẩm
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại! Vui lòng kiểm tra lại thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void TinhTongTien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string queryTongTien = @"
                SELECT SUM(spd.soLuong * sp.giaBanSanPham)
                FROM SP_DonHang spd
                JOIN SanPham sp ON spd.maSanPham = sp.maSanPham
                WHERE spd.maDonHang = @maDonHang;";

                    SqlCommand cmd = new SqlCommand(queryTongTien, conn);
                    cmd.Parameters.AddWithValue("@maDonHang", maDonHang);

                    object result = cmd.ExecuteScalar();

                    // Nếu không có sản phẩm nào thì tổng tiền là 0
                    decimal tongTien = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                    

                    // Hiển thị tổng tiền
                    txtTongTien.Text = tongTien.ToString("N0"); // Định dạng số có dấu phẩy
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính tổng tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMaKhuyenMai_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void btnSDKhuyenMai_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem mã khuyến mãi có hợp lệ không
                if (string.IsNullOrEmpty(txtMaKhuyenMai.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã khuyến mãi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lấy phần trăm giảm giá từ mã khuyến mãi
                    string queryDiscount = @"
                SELECT phanTramGiamGia
                FROM KhuyenMai
                WHERE maKhuyenMai = @maKhuyenMai;";

                    SqlCommand cmdDiscount = new SqlCommand(queryDiscount, conn);
                    cmdDiscount.Parameters.AddWithValue("@maKhuyenMai", txtMaKhuyenMai.Text);

                    object discountResult = cmdDiscount.ExecuteScalar();

                    // Nếu mã khuyến mãi không tồn tại
                    if (discountResult == null)
                    {
                        MessageBox.Show("Mã khuyến mãi không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Lấy phần trăm giảm giá
                    decimal phanTramGiamGia = Convert.ToDecimal(discountResult);

                    // Tính lại tổng tiền sau khi giảm giá
                    string queryTongTien = @"
                SELECT SUM(spd.soLuong * sp.giaBanSanPham)
                FROM SP_DonHang spd
                JOIN SanPham sp ON spd.maSanPham = sp.maSanPham
                WHERE spd.maDonHang = @maDonHang;";

                    SqlCommand cmdTongTien = new SqlCommand(queryTongTien, conn);
                    cmdTongTien.Parameters.AddWithValue("@maDonHang", maDonHang);

                    object result = cmdTongTien.ExecuteScalar();
                    decimal tongTien = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                    // Áp dụng giảm giá
                    decimal tongTienSauGiam = tongTien - (tongTien * phanTramGiamGia / 100);

                    // Cập nhật lại tổng tiền trong cơ sở dữ liệu (cập nhật vào bảng DonHang)
                    string updateQuery = @"
                UPDATE DonHang
                SET thanhTien = @thanhTien
                WHERE maDonHang = @maDonHang;";

                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, conn);
                    cmdUpdate.Parameters.AddWithValue("@thanhTien", tongTienSauGiam);
                    cmdUpdate.Parameters.AddWithValue("@maDonHang", maDonHang);

                    cmdUpdate.ExecuteNonQuery();

                    // Hiển thị tổng tiền sau giảm giá
                    txtTongTien.Text = tongTienSauGiam.ToString("N0"); // Định dạng số có dấu phẩy
                    MessageBox.Show("Đã áp dụng mã khuyến mãi thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi áp dụng mã khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
