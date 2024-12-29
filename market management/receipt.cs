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
using System.Data.SqlClient;
namespace market_management
{
    public partial class receipt : Form
    {
        string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True;Trust Server Certificate=True";
        SqlConnection Sqlcon = null;
        public receipt()
        {
            InitializeComponent();
        }

        private void receipt_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            LoadComboBoxes();
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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



        private void TimKiem_Click(object sender, EventArgs e)
        {
            // Chuỗi kết nối
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    // Tạo SqlCommand để gọi stored procedure
                    using (SqlCommand command = new SqlCommand("sp_LocDonHang", Sqlcon))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào stored procedure
                        command.Parameters.AddWithValue("@maDonHang",
                            string.IsNullOrWhiteSpace(txt_madonhang.Text) ? (object)DBNull.Value : int.Parse(txt_madonhang.Text));

                        command.Parameters.AddWithValue("@hinhThucThanhToan",
                            cb_hinhthucthanhtoan.SelectedIndex == -1 ? (object)DBNull.Value : cb_hinhthucthanhtoan.Text);

                        command.Parameters.AddWithValue("@TenNhanVien",
                            cb_nhanvien.SelectedIndex == -1 ? (object)DBNull.Value : cb_nhanvien.SelectedItem.ToString());

                        // Thêm tham số TenCongTy
                        command.Parameters.AddWithValue("@TenCongTy",
                            cb_congty.SelectedIndex == -1 ? (object)DBNull.Value : cb_congty.SelectedItem.ToString());

                        // Mở kết nối
                        Sqlcon.Open();

                        // Thực thi stored procedure và lấy dữ liệu trả về
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Tạo một DataTable để chứa kết quả
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader); // Đọc tất cả các dòng trả về từ SqlDataReader và điền vào DataTable

                            // Gán DataTable làm nguồn dữ liệu cho DataGridView
                            guna2DataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số cho Mã đơn hàng!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Làm rỗng các trường nhập liệu
            txt_madonhang.Clear();  // Xóa nội dung của TextBox txt_madonhang
            cb_hinhthucthanhtoan.SelectedIndex = -1;  // Đặt lại ComboBox cb_hinhthucthanhtoan về giá trị mặc định (không chọn)
            cb_nhanvien.SelectedIndex = -1;  // Đặt lại ComboBox cb_nhanvien về giá trị mặc định (không chọn)
            cb_congty.SelectedIndex = -1;  // Đặt lại ComboBox cb_congty về giá trị mặc định (không chọn)

            // Chuỗi kết nối
            string strCon = @"Data Source=DESKTOP-AQT03QH\SQLEXPRESS;Initial Catalog=QLBH;Integrated Security=True";

            try
            {
                using (SqlConnection Sqlcon = new SqlConnection(strCon))
                {
                    // Tạo SqlCommand để gọi stored procedure sp_LocDonHang với các tham số là NULL
                    using (SqlCommand command = new SqlCommand("sp_LocDonHang", Sqlcon))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào stored procedure (tất cả là NULL)
                        command.Parameters.AddWithValue("@maDonHang", DBNull.Value);
                        command.Parameters.AddWithValue("@hinhThucThanhToan", DBNull.Value);
                        command.Parameters.AddWithValue("@TenNhanVien", DBNull.Value);
                        command.Parameters.AddWithValue("@TenCongTy", DBNull.Value);

                        Sqlcon.Open();

                        // Thực thi câu lệnh và lấy dữ liệu trả về
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Tạo một DataTable để chứa kết quả
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);  // Đọc tất cả các dòng trả về từ SqlDataReader và điền vào DataTable

                            // Gán DataTable làm nguồn dữ liệu cho DataGridView
                            guna2DataGridView1.DataSource = dataTable;  // Giả sử bạn sử dụng DataGridView để hiển thị kết quả
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



        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Tạo một thể hiện của form editDonHang
            editDonHang frmEdit = new editDonHang();

            // Hiển thị form editDonHang dưới dạng modal (chặn việc sử dụng form chính khi form con mở)
            frmEdit.Show();
        }

        private void guna2DataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure there's a selected row
                if (guna2DataGridView1.CurrentRow != null && guna2DataGridView1.CurrentRow.Index >= 0)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = guna2DataGridView1.CurrentRow;

                    // Retrieve the necessary data from the selected row
                    string ma = selectedRow.Cells["MãĐơnHàng"].Value?.ToString();
                    string ten = selectedRow.Cells["TênKháchHàng"].Value?.ToString();
                    string thanhTien = selectedRow.Cells["ThànhTiền"].Value?.ToString();
                    string hinhThucThanhToan = selectedRow.Cells["HìnhThứcThanhToán"].Value?.ToString();
                    string ghiChu = selectedRow.Cells["GhiChú"].Value?.ToString();
                    int maDonHang = Convert.ToInt32(selectedRow.Cells["MãĐơnHàng"].Value);  // Assuming the MãĐơnHàng column holds the maDonHang value

                    // Create an instance of the subform (editDonHang) and set the values
                    editDonHang editForm = new editDonHang();

                    // Pass the values to the editDonHang form
                    editForm.SetTextBoxValues(ma, ten, thanhTien, hinhThucThanhToan, ghiChu, maDonHang);

                    // Show the editDonHang form
                    editForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
