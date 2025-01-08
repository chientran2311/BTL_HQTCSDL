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
    public partial class product : Form
    {
        public product()
        {
            InitializeComponent();
        }

        private void product_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;

            // Gắn sự kiện cho nút "Thống kê sản phẩm bán và doanh thu"
            guna2Button7.Click += guna2Button7_Click;
        }



        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }


        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //guna2DataGridView1.AutoGenerateColumns = false;

            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            SqlConnection sqlCon = null;
            //string query = "select maSanPham, tenSanPham, giaNhapSanPham, giaBanSanPham, slSanPhamTonKho, slSanPhamDaBan, trangThaiSanPham from SanPham";
            string query = "select * from SanPham";
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    guna2DataGridView1.AutoGenerateColumns = false;
                    guna2DataGridView1.Columns.Clear();

                    guna2DataGridView1.Columns.Add("maSanPham", "Mã Sản Phẩm");
                    guna2DataGridView1.Columns["maSanPham"].DataPropertyName = "maSanPham";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.Columns.Add("tenSanPham", "Tên Sản Phẩm");
                    guna2DataGridView1.Columns["tenSanPham"].DataPropertyName = "tenSanPham";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.Columns.Add("giaNhapSanPham", "Giá Nhập");
                    guna2DataGridView1.Columns["giaNhapSanPham"].DataPropertyName = "giaNhapSanPham";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.Columns.Add("giaBanSanPham", "Giá Bán");
                    guna2DataGridView1.Columns["giaBanSanPham"].DataPropertyName = "giaBanSanPham";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.Columns.Add("slSanPhamTonKho", "Số Lượng Tồn Kho");
                    guna2DataGridView1.Columns["slSanPhamTonKho"].DataPropertyName = "slSanPhamTonKho";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.Columns.Add("slSanPhamDaBan", "Số Lượng Đã Bán");
                    guna2DataGridView1.Columns["slSanPhamDaBan"].DataPropertyName = "slSanPhamDaBan";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.Columns.Add("trangThaiSanPham", "Trạng Thái");
                    guna2DataGridView1.Columns["trangThaiSanPham"].DataPropertyName = "trangThaiSanPham";  // Liên kết với cột trong DataTable

                    guna2DataGridView1.DataSource = dataTable;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void FetchBestSellingProducts(DateTime startDate, DateTime endDate)
        {
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";
            //
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo SqlCommand để gọi Procedure
                    SqlCommand command = new SqlCommand("sp_LaySanPhamBanChay", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số cho Procedure
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    // Thực hiện truy vấn và lấy kết quả
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Hiển thị kết quả trên DataGridView
                    guna2DataGridView1.AutoGenerateColumns = false;
                    guna2DataGridView1.Columns.Clear();

                    guna2DataGridView1.Columns.Add("tenSanPham", "Tên Sản Phẩm");
                    guna2DataGridView1.Columns["tenSanPham"].DataPropertyName = "tenSanPham";

                    guna2DataGridView1.Columns.Add("soLuongBan", "Số Lượng Bán");
                    guna2DataGridView1.Columns["soLuongBan"].DataPropertyName = "soLuongBan";

                    guna2DataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy sản phẩm bán chạy: " + ex.Message);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Hiển thị một hộp thoại để chọn khoảng thời gian
            using (Form dateForm = new Form())
            {
                dateForm.Text = "Chọn khoảng thời gian";

                // Thêm DateTimePicker cho Start Date
                DateTimePicker dtpStartDate = new DateTimePicker();
                dtpStartDate.Format = DateTimePickerFormat.Short;
                dtpStartDate.Value = DateTime.Today.AddMonths(-1); // Mặc định: 1 tháng trước
                dtpStartDate.Location = new Point(20, 20);
                dateForm.Controls.Add(new Label { Text = "Từ ngày:", Location = new Point(20, 0) });
                dateForm.Controls.Add(dtpStartDate);

                // Thêm DateTimePicker cho End Date
                DateTimePicker dtpEndDate = new DateTimePicker();
                dtpEndDate.Format = DateTimePickerFormat.Short;
                dtpEndDate.Value = DateTime.Today; // Mặc định: ngày hiện tại
                dtpEndDate.Location = new Point(20, 80);
                dateForm.Controls.Add(new Label { Text = "Đến ngày:", Location = new Point(20, 60) });
                dateForm.Controls.Add(dtpEndDate);

                // Thêm nút OK
                Button btnOk = new Button { Text = "OK", Location = new Point(20, 140) };
                btnOk.Click += (s, ea) => dateForm.DialogResult = DialogResult.OK;
                dateForm.Controls.Add(btnOk);

                dateForm.StartPosition = FormStartPosition.CenterParent;
                dateForm.AutoSize = true;

                if (dateForm.ShowDialog() == DialogResult.OK)
                {
                    DateTime startDate = dtpStartDate.Value;
                    DateTime endDate = dtpEndDate.Value;

                    // Gọi hàm để lấy sản phẩm bán chạy
                    FetchBestSellingProducts(startDate, endDate);
                }
            }
        }
        private void LoadProductList()
        {
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";
            string query = "SELECT * FROM SanPham";

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    guna2DataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message);
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            // Chuỗi kết nối tới cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";
            // Truy vấn để lấy dữ liệu từ View vw_SanPhamDoanhThu
            string query = "SELECT * FROM vw_SanPhamDoanhThu";
            //
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Tạo adapter để lấy dữ liệu
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                    // Tạo DataTable để chứa dữ liệu
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Thiết lập DataGridView để hiển thị dữ liệu
                    guna2DataGridView1.AutoGenerateColumns = false;
                    guna2DataGridView1.Columns.Clear();

                    // Thêm cột "Tên Sản Phẩm"
                    guna2DataGridView1.Columns.Add("tenSanPham", "Tên Sản Phẩm");
                    guna2DataGridView1.Columns["tenSanPham"].DataPropertyName = "tenSanPham";

                    // Thêm cột "Số Lượng Bán"
                    guna2DataGridView1.Columns.Add("soLuongBan", "Số Lượng Bán");
                    guna2DataGridView1.Columns["soLuongBan"].DataPropertyName = "soLuongBan";

                    // Thêm cột "Doanh Thu"
                    guna2DataGridView1.Columns.Add("doanhThu", "Doanh Thu");
                    guna2DataGridView1.Columns["doanhThu"].DataPropertyName = "doanhThu";

                    // Gán dữ liệu từ DataTable vào DataGridView
                    guna2DataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu xảy ra
                MessageBox.Show("Lỗi khi tải thống kê: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Chuỗi kết nối tới cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Truy vấn dữ liệu từ View vw_DonHangKhachHangNhanVien
            string query = "SELECT * FROM vw_DonHangKhachHangNhanVien";
            //
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Tạo SqlDataAdapter để thực thi truy vấn
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                    // Tạo DataTable để chứa dữ liệu
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Cấu hình DataGridView để hiển thị dữ liệu
                    guna2DataGridView1.AutoGenerateColumns = false;
                    guna2DataGridView1.Columns.Clear();

                    // Thêm cột "Mã Đơn Hàng"
                    guna2DataGridView1.Columns.Add("maDonHang", "Mã Đơn Hàng");
                    guna2DataGridView1.Columns["maDonHang"].DataPropertyName = "maDonHang";

                    // Thêm cột "Ngày Tạo Đơn"
                    guna2DataGridView1.Columns.Add("ngayTaoDon", "Ngày Tạo Đơn");
                    guna2DataGridView1.Columns["ngayTaoDon"].DataPropertyName = "ngayTaoDon";

                    // Thêm cột "Tên Khách Hàng"
                    guna2DataGridView1.Columns.Add("tenKhachHang", "Tên Khách Hàng");
                    guna2DataGridView1.Columns["tenKhachHang"].DataPropertyName = "tenKhachHang";

                    // Thêm cột "Tên Nhân Viên"
                    guna2DataGridView1.Columns.Add("tenNhanVien", "Tên Nhân Viên");
                    guna2DataGridView1.Columns["tenNhanVien"].DataPropertyName = "tenNhanVien";

                    // Thêm cột "Thành Tiền"
                    guna2DataGridView1.Columns.Add("thanhTien", "Thành Tiền");
                    guna2DataGridView1.Columns["thanhTien"].DataPropertyName = "thanhTien";

                    // Gắn dữ liệu từ DataTable vào DataGridView
                    guna2DataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show("Lỗi khi tải dữ liệu đơn hàng: " + ex.Message);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Chuỗi kết nối đến cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Lấy thông tin từ giao diện 
            int maKhoHang = int.Parse(guna2TextBox1.Text); // TextBox chứa mã kho hàng
            int maSanPham = int.Parse(guna2TextBox2.Text); // TextBox chứa mã sản phẩm
            int soLuong = int.Parse(guna2NumericUpDown1.Text); // NumericUpDown chứa số lượng

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo SqlCommand để gọi Procedure
                    SqlCommand command = new SqlCommand("sp_ThemSanPhamVaoKho", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số cho Procedure
                    command.Parameters.AddWithValue("@maKhoHang", maKhoHang);
                    command.Parameters.AddWithValue("@maSanPham", maSanPham);
                    command.Parameters.AddWithValue("@soLuong", soLuong);

                    // Thực thi Procedure
                    int rowsAffected = command.ExecuteNonQuery();

                    // Thông báo thành công
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm sản phẩm vào kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại danh sách sản phẩm (nếu cần)
                        LoadProductList();
                    }
                    else
                    {
                        MessageBox.Show("Không thêm được sản phẩm. Vui lòng kiểm tra dữ liệu nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show("Lỗi khi thêm sản phẩm vào kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Lấy mã đơn hàng từ TextBox hoặc bất kỳ nguồn nào trong giao diện
            int maDonHang = int.Parse(guna2TextBox2.Text); // Đảm bảo có TextBox để nhập mã đơn hàng

            // Chuỗi kết nối tới cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Tạo SqlCommand để gọi hàm fn_TongTienDonHang
                    SqlCommand command = new SqlCommand("SELECT dbo.fn_TongTienDonHang(@maDonHang)", connection);
                    command.Parameters.AddWithValue("@maDonHang", maDonHang);  // Thêm tham số mã đơn hàng

                    // Mở kết nối
                    connection.Open();

                    // Thực thi câu lệnh và lấy kết quả
                    var result = command.ExecuteScalar();  // Trả về tổng tiền từ hàm

                    // Hiển thị kết quả trong một Label hoặc TextBox
                    guna2TextBox6.Text = result.ToString();  // Hiển thị tổng tiền trong TextBox
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Lỗi khi tính tổng tiền: " + ex.Message);
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            try
            {
                int maSanPham = int.Parse(guna2TextBox2.Text); // Mã sản phẩm nhập từ TextBox
                int soLuong = (int)guna2NumericUpDown1.Value; // Số lượng nhập từ NumericUpDown

                // Chuỗi kết nối cơ sở dữ liệu
                string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo SqlCommand để gọi hàm fn_KiemTraTonKho
                    SqlCommand command = new SqlCommand("SELECT dbo.fn_KiemTraTonKho(@maSanPham, @soLuong)", connection);
                    command.Parameters.AddWithValue("@maSanPham", maSanPham);  // Thêm tham số mã sản phẩm
                    command.Parameters.AddWithValue("@soLuong", soLuong);      // Thêm tham số số lượng

                    // Thực thi và lấy kết quả (kết quả là 1 nếu đủ tồn kho, 0 nếu không)
                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        // Kiểm tra và ép kiểu an toàn
                        bool isInStock = Convert.ToBoolean(result);  // Ép kiểu sang Boolean

                        // Kiểm tra kết quả và thông báo
                        if (isInStock)
                        {
                            MessageBox.Show("Sản phẩm có đủ tồn kho để bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Sản phẩm không đủ tồn kho để bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu hoặc lỗi xảy ra trong hàm kiểm tra tồn kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show("Lỗi khi kiểm tra tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã sản phẩm cần xóa từ TextBox
                int maSanPham = int.Parse(guna2TextBox2.Text);

                // Chuỗi kết nối đến cơ sở dữ liệu
                string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo câu lệnh xóa sản phẩm
                    SqlCommand command = new SqlCommand("DELETE FROM SanPham WHERE maSanPham = @maSanPham", connection);
                    command.Parameters.AddWithValue("@maSanPham", maSanPham);

                    // Thực thi câu lệnh xóa
                    command.ExecuteNonQuery();

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Làm mới DataGridView sau khi xóa
                    LoadProductList();  // Phương thức này tải lại danh sách sản phẩm từ cơ sở dữ liệu

                    // Xóa giá trị trong TextBox trạng thái sản phẩm
                    guna2TextBox5.Text = "0";  // Cập nhật lại trạng thái sản phẩm thành "0"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
