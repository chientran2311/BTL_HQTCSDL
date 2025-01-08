using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace market_management
{
    public partial class formDash : Form
    {
        public formDash()
        {
            InitializeComponent();
        }

        private void formDash_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            txtTienNhap.Text = null;
            txtDoanhThu.Text = null;
            txtTienBan.Text = null;
            monthlbl.Text = null;
            monthnv.Text = null;
            txtTop1.Text = null;
            txtTop2.Text = null;
            txtTop3.Text = null;
            txtTienmat.Text = null;
            txtChuyenKhoan.Text = null;
        }

        public void TienhangNhap()
        {
            // Kiểm tra nếu ComboBox không có giá trị
            if (string.IsNullOrEmpty(cb_month.Text))
            {
                MessageBox.Show("Vui lòng chọn tháng để xem tổng tiền.");
                return;
            }

            // Lấy giá trị tháng từ ComboBox (giá trị hiển thị)
            int month;
            if (!int.TryParse(cb_month.Text, out month))
            {
                MessageBox.Show("Tháng không hợp lệ.");
                return;
            }

            // Câu lệnh SQL để tính tổng tiền hàng đã nhập theo tháng
            string query = "SELECT SUM(tongTien) FROM ChiTietKhoHang WHERE DATEPART(MM, ngayNhap) = @Month";

            // Chuỗi kết nối cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo đối tượng Command để thực thi câu lệnh SQL
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Thêm tham số @Month vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@Month", month);

                        // Thực thi câu lệnh và lấy kết quả
                        object result = cmd.ExecuteScalar();

                        // Nếu kết quả không rỗng, gán giá trị cho TextBox, nếu không thì gán 0
                        txtTienNhap.Text = result != DBNull.Value ? result.ToString() : "0";
                    }
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra, hiển thị thông báo lỗi
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }
        public void TinhDoanhThuTheoThang()
        {
            // Chuỗi kết nối tới cơ sở dữ liệu
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Lấy giá trị tháng từ giao diện (ví dụ ComboBox cb_month)
            if (cb_month.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tháng để tính doanh thu.");
                return;
            }

            // Chuyển giá trị từ ComboBox sang kiểu int
            if (!int.TryParse(cb_month.SelectedItem.ToString(), out int month))
            {
                MessageBox.Show("Tháng không hợp lệ. Vui lòng chọn lại.");
                return;
            }

            try
            {
                // Kết nối đến SQL Server
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo câu lệnh SQL để gọi hàm
                    string query = "SELECT dbo.TinhDoanhThuTheoThang(@Month)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Thêm tham số @Month
                        cmd.Parameters.AddWithValue("@Month", month);

                        // Thực thi câu lệnh và nhận kết quả
                        object result = cmd.ExecuteScalar();

                        // Kiểm tra kết quả trả về và hiển thị
                        if (result != null && result != DBNull.Value)
                        {
                            // Chuyển đổi kết quả sang kiểu decimal
                            decimal doanhThu = Convert.ToDecimal(result);

                            // Hiển thị doanh thu trong TextBox (txtDoanhThu)
                            txtDoanhThu.Text = doanhThu.ToString("N2"); // Hiển thị dạng số thập phân
                        }
                        else
                        {
                            txtDoanhThu.Text = "0"; // Nếu không có doanh thu, hiển thị 0
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Thông báo lỗi liên quan đến SQL
                MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Thông báo lỗi chung
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }


        public void Tienhangban()
        {
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";
            // Kiểm tra nếu người dùng chưa chọn tháng
            if (cb_month.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tháng để tính doanh thu.");
                return;
            }

            // Lấy giá trị tháng từ ComboBox
            int month = Convert.ToInt32(cb_month.SelectedItem);

            // Câu lệnh SQL để tính tổng tiền hàng bán ra theo tháng
            string query = @"
            SELECT ISNULL(SUM(SP_DonHang.soLuong * SanPham.giaBanSanPham), 0) 
            FROM SP_DonHang
            INNER JOIN SanPham ON SP_DonHang.maSanPham = SanPham.maSanPham
            INNER JOIN DonHang ON SP_DonHang.maDonHang = DonHang.maDonHang
            WHERE DATEPART(MM, DonHang.Ngaytaodon) = @Month";

            // Kết nối với cơ sở dữ liệu và thực thi câu lệnh SQL
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                try
                {
                    connection.Open(); // Mở kết nối

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@Month", month);

                        // Thực thi câu lệnh và lấy giá trị trả về
                        object result = command.ExecuteScalar();

                        // Kiểm tra và hiển thị kết quả vào TextBox
                        decimal totalSales = Convert.ToDecimal(result);
                        txtTienBan.Text = totalSales.ToString("N0"); // Định dạng số với dấu phân cách hàng nghìn
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi (nếu có)
                    MessageBox.Show("Lỗi khi truy vấn dữ liệu: " + ex.Message);
                }
            }
        }
        public void TopNhanVien()
        {
            
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

           
            int thang = Convert.ToInt32(cb_month.SelectedItem); 

           
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    connection.Open();

                    
                    using (SqlCommand cmd = new SqlCommand("sp_Top5NhanVien", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                       
                        cmd.Parameters.AddWithValue("@thang", thang);

                        
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                        {
                           
                            DataTable dataTable = new DataTable();

                           
                            dataAdapter.Fill(dataTable);

                            
                            tableNhanvien.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Lỗi khi kết nối đến cơ sở dữ liệu: " + ex.Message);
                }
            }
        }


        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void monthlbl_Click(object sender, EventArgs e)
        {

        }

        public void Top3SanPham()
        {
            // Chuỗi kết nối
            string connectionString = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Lấy tháng từ ComboBox
            int thangHienTai = Convert.ToInt32(cb_month.SelectedItem);
            

            // Câu lệnh SQL
            string query = @"
        DECLARE @InputThang INT = @thangHienTai;
        DECLARE @InputNam INT = 2024;
        DECLARE @maSanPham INT;
        DECLARE @soLuongDaBan INT;
        DECLARE @tenSanPham NVARCHAR(255);

        CREATE TABLE #TopProducts (
            TenSanPham NVARCHAR(255),
            MaSanPham INT,
            SoLuongDaBan INT
        );

        DECLARE cur_Sp_Hot CURSOR SCROLL FOR
            SELECT TOP 3
                SPD.maSanPham, 
                SUM(SPD.soLuong) AS soLuongDaBan
            FROM SP_DonHang SPD
            INNER JOIN DonHang DH ON SPD.maDonHang = DH.maDonHang
            WHERE DATEPART(MM, DH.Ngaytaodon) = @InputThang
              AND DATEPART(YYYY, DH.Ngaytaodon) = @InputNam
            GROUP BY SPD.maSanPham
            ORDER BY soLuongDaBan DESC;

        OPEN cur_Sp_Hot;
        FETCH NEXT FROM cur_Sp_Hot INTO @maSanPham, @soLuongDaBan;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SELECT @tenSanPham = tenSanPham 
            FROM SanPham 
            WHERE maSanPham = @maSanPham;

            INSERT INTO #TopProducts (TenSanPham, MaSanPham, SoLuongDaBan)
            VALUES (@tenSanPham, @maSanPham, @soLuongDaBan);

            FETCH NEXT FROM cur_Sp_Hot INTO @maSanPham, @soLuongDaBan;
        END

        CLOSE cur_Sp_Hot;
        DEALLOCATE cur_Sp_Hot;

        SELECT * FROM #TopProducts;
        DROP TABLE #TopProducts;
    ";

            // Kết nối tới SQL Server
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Truyền tham số
                        cmd.Parameters.AddWithValue("@thangHienTai", thangHienTai);
                        

                        // Thực thi và đọc dữ liệu
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int count = 1;

                            while (reader.Read() && count <= 3)
                            {
                                // Lấy dữ liệu từ kết quả
                                string tenSanPham = reader["TenSanPham"].ToString();
                                int maSanPham = Convert.ToInt32(reader["MaSanPham"]);
                                int soLuongDaBan = Convert.ToInt32(reader["SoLuongDaBan"]);

                                // Gán vào các Label
                                if (count == 1)
                                {
                                    txtTop1.Text = $"Sản phẩm: {tenSanPham} - Mã: {maSanPham} - Đã bán: {soLuongDaBan}";
                                }
                                else if (count == 2)
                                {
                                    txtTop2.Text = $"Sản phẩm: {tenSanPham} - Mã: {maSanPham} - Đã bán: {soLuongDaBan}";
                                }
                                else if (count == 3)
                                {
                                    txtTop3.Text = $"Sản phẩm: {tenSanPham} - Mã: {maSanPham} - Đã bán: {soLuongDaBan}";
                                }

                                count++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
        }









        public void thongkeHinhthuc()
        {
            // Chuỗi kết nối với SQL Server
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            // Kết nối với cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                try
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo câu lệnh SQL để thực thi con trỏ
                    string query = @"
                DECLARE @hinhThucThanhToan NVARCHAR(50);
                DECLARE @soLuongDonHang INT;
                DECLARE cur_HinhThucThanhToan CURSOR FOR
                    SELECT DISTINCT hinhThucThanhToan
                    FROM DonHang
                    WHERE hinhThucThanhToan IS NOT NULL;

                OPEN cur_HinhThucThanhToan;

                FETCH NEXT FROM cur_HinhThucThanhToan INTO @hinhThucThanhToan;

                -- Lấy số lượng đơn hàng cho mỗi hình thức thanh toán
                CREATE TABLE #ThongKe (HinhThucThanhToan NVARCHAR(50), SoLuongDonHang INT);

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    -- Lấy số lượng đơn hàng cho hình thức thanh toán
                    SELECT @soLuongDonHang = COUNT(maDonHang)
                    FROM DonHang
                    WHERE hinhThucThanhToan = @hinhThucThanhToan;

                    -- Insert vào bảng tạm
                    INSERT INTO #ThongKe (HinhThucThanhToan, SoLuongDonHang)
                    VALUES (@hinhThucThanhToan, @soLuongDonHang);

                    FETCH NEXT FROM cur_HinhThucThanhToan INTO @hinhThucThanhToan;
                END

                -- Trả về kết quả thống kê hình thức thanh toán
                SELECT * FROM #ThongKe;

                -- Xóa bảng tạm
                DROP TABLE #ThongKe;

                CLOSE cur_HinhThucThanhToan;
                DEALLOCATE cur_HinhThucThanhToan;
            ";

                    // Tạo command để thực thi câu truy vấn SQL
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Tạo SqlDataReader để đọc kết quả
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Đọc từng dòng kết quả từ con trỏ
                            while (reader.Read())
                            {
                                // Lấy hình thức thanh toán và số lượng đơn hàng
                                string hinhThucThanhToan = reader.GetString(0);
                                int soLuongDonHang = reader.GetInt32(1);

                                // Gán kết quả vào các TextBox
                                if (hinhThucThanhToan == "Tiền mặt")
                                {
                                    txtTienmat.Text = $"Hình thức thanh toán: Tiền mặt, Số đơn hàng: {soLuongDonHang}";
                                }
                                else if (hinhThucThanhToan == "Chuyển khoản")
                                {
                                    txtChuyenKhoan.Text = $"Hình thức thanh toán: Chuyển khoản, Số đơn hàng: {soLuongDonHang}";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    MessageBox.Show("Lỗi khi kết nối đến cơ sở dữ liệu: " + ex.Message);
                }
            }
        }



        private void cb_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            TienhangNhap();
            Tienhangban();
            monthlbl.Text = cb_month.SelectedItem.ToString();
            monthnv.Text = cb_month.SelectedItem.ToString();
            txtDoanhThu.Text = "0";
            tableNhanvien.DataSource = null;
            txtTop1.Text = null;
            txtTop2.Text = null;
            txtTop3.Text = null;
            txtTienmat.Text = null;
            txtChuyenKhoan.Text = null;
        }

        private void btnKetToan_Click(object sender, EventArgs e)
        {
            TinhDoanhThuTheoThang();
            TopNhanVien();
            Top3SanPham();
            thongkeHinhthuc();
            
        }

        private void tableNhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
