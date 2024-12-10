using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace market_management
{
    public partial class dashboard : Form
    {
        public dashboard()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {

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
    }

}
