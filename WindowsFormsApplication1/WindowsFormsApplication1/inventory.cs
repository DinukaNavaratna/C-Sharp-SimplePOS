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
    public partial class inventory : Form
    {
        private int id = 0;
        private string date = DateTime.Now.ToString("dd/MM/yyyy");
        private int item_count = 1;

        public inventory()
        {
            InitializeComponent();
            this.Text = "Daily Inventory : " + date;
            textBox1.Visible = false;
            button1.Text = "New";
            ControlBox = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                textBox1.Visible = true;
                button2.Visible = false;
                button4.Visible = true;
                button1.Text = "Next";
                id++;
            }
            else
            {
                textBox1.Visible = true;
                button2.Visible = false;
                button4.Visible = true;
                button1.Text = "Next";
                enter_inventory();
                inventory_list fm = new inventory_list();
                fm.view_inventory();
            }
        }
        private void enter_inventory()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter a valid amount!");
            }
            else
            {
                db_methods db = new db_methods();
                item_count = db.item_count();
                if (item_count > id)
                {
                    var tuple = db.item_info(id);
                    label1.Text = tuple.Item2;

                    int qty = int.Parse(textBox1.Text);
                    string msg = db.insert_daily_inventory(id, date, qty);
                    textBox1.Text = null;
                }
                else
                {
                    label1.Text = "Inventory Updated!\nDate: " + date;
                    textBox1.Visible = false;
                    textBox1.Visible = false;
                    button2.Visible = false;
                    button1.Visible = false;
                    button4.Visible = false;
                    ControlBox = true;
                }
                id++;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ControlBox = true;
            inventory_list fm1 = new inventory_list();
            fm1.Show();
            this.Close();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            db_methods db = new db_methods();
            item_count = db.item_count();
            if (item_count >= id)
            {
                var tuple = db.item_info(id);
                label1.Text = tuple.Item2;
            }
            else
            {
                label1.Text = "Inventory Updated!\nDate: " + date;
                textBox1.Text = null;
                textBox1.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                button4.Visible = false;
                ControlBox = true;
            }
            id++;
        }
    }
}
