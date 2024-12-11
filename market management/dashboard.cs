using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace market_management
{
    public partial class dashboard : Form
    {
        formDash dashboard2;
        sale Sale;
        product Product;
        receipt Receipt;
        warehouse Warehouse;
        employee Employee;
        public dashboard()
        {
            InitializeComponent();
            mdiProp(); 
        }
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232,232,232);
        }
        private void dashboard_Load(object sender, EventArgs e)
        {

        }
        bool sidebarExpanded = true;
        private async void hide_Click(object sender, EventArgs e)
        {
            if (sidebarExpanded)
            {
                await CollapseSidebar();
            }
            else
            {
                await ExpandSidebar();
            }
        }

        private async Task CollapseSidebar()
        {
            while (slidebar.Width > 60)
            {
                slidebar.Width -= 10;
                await Task.Delay(10); // Tạo hiệu ứng mượt
            }
            sidebarExpanded = false;
        }

        private async Task ExpandSidebar()
        {
            while (slidebar.Width < 200)
            {
                slidebar.Width += 10;
                await Task.Delay(10); // Tạo hiệu ứng mượt
            }
            sidebarExpanded = true;
        }

        private void Dashboard_btn_Click(object sender, EventArgs e)
        {
            if(dashboard2 == null)
            {
                dashboard2 = new formDash();
                dashboard2.FormClosed += Dashboard_FormClosed;
                dashboard2.MdiParent = this;
                dashboard2.Dock = DockStyle.Fill;
                dashboard2.Show();
            }
            else
            {
                dashboard2.Activate();
            }
        }
        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
           dashboard2 = null;
        }

        private void sale_btn_Click(object sender, EventArgs e)
        {
            if (Sale == null)
            {
                Sale = new sale();
                Sale.FormClosed += Sale_FormClosed;
                Sale.MdiParent = this;
                Sale.Dock = DockStyle.Fill;
                Sale.Show();
            }
            else
            {
                Sale.Activate();
            }
        }

        private void Sale_FormClosed(object sender, FormClosedEventArgs e)
        {
            Sale = null;
        }

        private void receipt_btn_Click(object sender, EventArgs e)
        {
            if(Receipt == null)
            {
                Receipt = new receipt();
                Receipt.FormClosed += Receipt_FormClosed;
                Receipt.MdiParent = this;
                Receipt.Dock = DockStyle.Fill;
                Receipt.Show();
            }
            else{
                    Receipt.Activate();
            }
        }

        private void Receipt_FormClosed(object sender, FormClosedEventArgs e)
        {
            Receipt = null;
        }

        private void product_btn_Click(object sender, EventArgs e)
        {
            if (Product == null)
            {
                Product = new product();
                Product.FormClosed += Product_FormClosed;
                Product.MdiParent = this;
                Product.Dock = DockStyle.Fill;
                Product.Show();
            }
            else
            {
                Product.Activate(); 
            }

        }

        private void Product_FormClosed(object sender, FormClosedEventArgs e)
        {
           Product = null;
        }

        private void warehouse_btn_Click(object sender, EventArgs e)
        {
            if(Warehouse == null)
            {
                Warehouse = new warehouse();
                Warehouse.FormClosed += Warehouse_FormClosed;
                Warehouse.MdiParent = this;
                Warehouse.Dock = DockStyle.Fill;
                Warehouse.Show();
            }
            else
            {
                Warehouse.Activate();
            }
        }

        private void Warehouse_FormClosed(object sender, FormClosedEventArgs e)
        {
           Warehouse=null;
        }

        private void employee_btn_Click(object sender, EventArgs e)
        {
            if(Employee == null)
            {
                Employee = new employee();
                Employee.FormClosed += Employee_FormClosed;
                Employee.MdiParent = this;
                Employee.Dock = DockStyle.Fill;
                Employee.Show();
            }
            else
            {
                Employee.Activate();
            }
        }

        private void Employee_FormClosed(object sender, FormClosedEventArgs e)
        {
            Employee=null;
        }

        private void signout_Click(object sender, EventArgs e)
        {
            
            login2 login2 = new login2();
            login2.Show();
            
            this.Close();
        }
    }

}
