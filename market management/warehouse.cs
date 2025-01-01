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
using Guna.UI2.WinForms;

namespace market_management
{
    public partial class warehouse : Form
    {
        public warehouse()
        {
            InitializeComponent();

        }





        private void warehouse_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLBHDataSet2.ChiTietKhoHang' table. You can move, or remove it, as needed.
            this.chiTietKhoHangTableAdapter2.Fill(this.qLBHDataSet2.ChiTietKhoHang);
            // TODO: This line of code loads data into the 'qLBHDataSet2.KhoHang' table. You can move, or remove it, as needed.
            this.khoHangTableAdapter.Fill(this.qLBHDataSet2.KhoHang);
            // TODO: This line of code loads data into the 'qLBHDataSet2.SanPham' table. You can move, or remove it, as needed.
            this.sanPhamTableAdapter.Fill(this.qLBHDataSet2.SanPham);
            // TODO: This line of code loads data into the 'qLBHDataSet2.viewChiTietKhoHang' table. You can move, or remove it, as needed.
            this.viewChiTietKhoHangTableAdapter2.Fill(this.qLBHDataSet2.viewChiTietKhoHang);
            // TODO: This line of code loads data into the 'qLBHDataSet1.viewChiTietKhoHang' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'qLBHDataSet1.ChiTietKhoHang' table. You can move, or remove it, as needed.


            this.ControlBox = false;
        }
        // Warehouse.cs
        public void RefreshDataGridView()
        {
            string query = "SELECT * FROM ChiTietKhoHang"; // Hoặc view của bạn

            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=HUY;Initial Catalog=QLBH;Integrated Security=True;"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader); // Tải dữ liệu vào DataTable
                            dataGridView1.DataSource = dt; // Gán DataTable vào DataGridView
                        }
                    }
                }
                MessageBox.Show("Data refreshed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while refreshing data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form_Them form2 = new Form_Them();

            // Show Form2
            form2.Show();

            // Optionally, hide the current form (this form)
            // this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem ComboBox đã chọn chưa
            if (guna2ComboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a KhoHang!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy dữ liệu maKhoHang từ ComboBox
                DataRowView selectedRow1 = (DataRowView)guna2ComboBox2.SelectedItem;
                DataRowView selectedRow2 = (DataRowView)guna2ComboBox3.SelectedItem;

                var tenKhoHang = selectedRow1["tenKhoHang"]; // Thay đổi tên cột tương ứng với cột bạn cần
                var tenSanPham = selectedRow2["tenSanPham"]; // Thay đổi tên cột tương ứng với cột bạn cần
                // Mở kết nối và gọi stored procedure để truy vấn dữ liệu
                using (SqlConnection conn = new SqlConnection("Data Source=HUY;Initial Catalog=QLBH;Integrated Security=True;"))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("TraCuuKhoHang", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số cho stored procedure
                        cmd.Parameters.AddWithValue("@tenKhoHang", tenKhoHang);
                        cmd.Parameters.AddWithValue("@tenSanPham", tenSanPham);

                        // Thực thi lệnh và lấy kết quả từ stored procedure
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader); // Tải dữ liệu từ SqlDataReader vào DataTable

                            // Gán DataTable vào DataGridView để hiển thị dữ liệu
                            dataGridView1.DataSource = dt;
                        }
                    }
                }

                MessageBox.Show("Data fetched successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Check if any row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extract values from the selected row
            var selectedRow = dataGridView1.SelectedRows[0];
            var maChiTietKhoHang = Convert.ToInt32(selectedRow.Cells["maChiTietKhoHangDataGridViewTextBoxColumn"].Value);
            var maSanPham = Convert.ToInt32(selectedRow.Cells["maSanPhamDataGridViewTextBoxColumn"].Value);
            var soLuong = Convert.ToInt32(selectedRow.Cells["soLuongDataGridViewTextBoxColumn"].Value);
            var maKhoHang = Convert.ToInt32(selectedRow.Cells["maKhoHangDataGridViewTextBoxColumn"].Value);
            var ngayNhap = Convert.ToDateTime(selectedRow.Cells["ngayNhapDataGridViewTextBoxColumn"].Value);

            // Confirm deletion
            DialogResult result = MessageBox.Show($"Are you sure you want to delete the product with maChiTietKhoHang = {maChiTietKhoHang}, maSanPham = {maSanPham}, soLuong = {soLuong}, maKhoHang = {maKhoHang}, ngayNhap = {ngayNhap:yyyy-MM-dd}?",
     "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                try
                {
                    // Connect to the database
                    using (SqlConnection conn = new SqlConnection("Data Source=HUY;Initial Catalog=QLBH;Integrated Security=True;"))
                    {
                        conn.Open();

                        // Create SqlCommand for stored procedure
                        using (SqlCommand cmd = new SqlCommand("DeleteKhoHang", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Add parameters
                            cmd.Parameters.AddWithValue("@ngayNhap", ngayNhap);
                            cmd.Parameters.AddWithValue("@maSanPham", maSanPham);
                            cmd.Parameters.AddWithValue("@soLuong", soLuong);
                            cmd.Parameters.AddWithValue("@maKhoHang", maKhoHang);
                            cmd.Parameters.AddWithValue("@maChiTietKhoHang", maChiTietKhoHang);

                            // Execute the stored procedure
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Show success message
                    MessageBox.Show("Product successfully deleted from the warehouse!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh DataGridView
                    RefreshDataGridView(); // Replace this with your actual refresh logic
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
