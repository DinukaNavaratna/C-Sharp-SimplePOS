using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class welcome : Form
    {
        public welcome()
        {
            InitializeComponent();
        }
        private Timer tm;
        private void welcome_Load(object sender, EventArgs e)
        {
            tm = new Timer();
            tm.Interval = 2 * 1000; // 2 seconds
            tm.Tick += new EventHandler(tm_Tick);
            tm.Start();

        }
        private void tm_Tick(object sender, EventArgs e)
        {
            login fm1 = new login();
            fm1.Show();
            this.Hide();
            tm.Stop();
        }
    }
}
