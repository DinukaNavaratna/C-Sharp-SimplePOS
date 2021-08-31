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
    public partial class main_interface : Form
    {
        private int billno = 1;
        private double tot = 0;
        private string Description = "";
        private string User_Name = "";

        public main_interface(string user, int utype)
        {
            InitializeComponent();
            timer1.Start();
            label24.Text = user;
            User_Name = user;
            if(utype == 1)
            {
                button24.Visible = false;
                button35.Visible = false;
                button36.Visible = false;
            }
            else
            {
                button24.Visible = true;
                button35.Visible = true;
                button36.Visible = true;
            }

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime datetime = DateTime.Now;
            label7.Text = datetime.ToLongTimeString();
            label8.Text = datetime.ToLongDateString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int id = 001;
            AddToBill(id);
            /*Qty qty1 = new Qty(id);
            qty1.Show();*/
        }

        public void AddToBill(int id)
        {
            Control num = bill.GetControlFromPosition(0, billno);
            bill.Controls.Remove(num);

            db_methods db = new db_methods();
            var tuple = db.item_info_for_bill(id);

            bill.Controls.Add(new Label() { Text = billno.ToString().PadLeft(3, '0')}, 0, billno);
            bill.Controls.Add(new Label() { Text = tuple.Item1}, 1, billno);
            bill.Controls.Add(new Label() { Text =  "1"}, 2, billno);
            bill.Controls.Add(new Label() { Text = (tuple.Item2).ToString()}, 3, billno);
            bill.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            bill.Controls.Add(new Label() { Text = ""}, 0, billno + 1);
            billno = billno + 1;

            Description = Description + tuple.Item1 + ":" + (tuple.Item2).ToString() + "/ ";
            
            tot = tot + tuple.Item2;
            label19.Text = tot.ToString();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int id = 002;
            AddToBill(id);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            int id = 003;
            AddToBill(id);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            int id = 004;
            AddToBill(id);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            int id = 005;
            AddToBill(id);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            int id = 006;
            AddToBill(id);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            int id = 007;
            AddToBill(id);
        }

        private void Button21_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            if(textBox1.Text == "")
            {
                MessageBox.Show("Please enter the paid amount to proceed!");
            }
            else
            {
                label21.Text = (int.Parse(textBox1.Text) - tot).ToString();

                double Total = tot;
                string Time = DateTime.Now.ToString("h:mm:ss tt");
                string Date = DateTime.Now.ToString("dd/MM/yyyy");

                db_methods db = new db_methods();
                string msg = db.add_bill(Description, Total, User_Name, Time, Date);
                //MessageBox.Show(Description + "\n\n" + Total + "\n" + User_Name + "\n" + Time + "\n" + Date);
                MessageBox.Show(msg);
                button22.PerformClick();
            }

        }

        private void Button22_Click(object sender, EventArgs e)
        {
            int x = 0;

            textBox1.Enabled = true;
            textBox1.Text = null;
            label21.Text = null;
            label19.Text = null;

            for (x=billno; x>0; x--)
            {
                Control num = bill.GetControlFromPosition(0, x);
                Control num1 = bill.GetControlFromPosition(1, x);
                Control num2 = bill.GetControlFromPosition(2, x);
                Control num3 = bill.GetControlFromPosition(3, x);
                bill.Controls.Remove(num);
                bill.Controls.Remove(num1);
                bill.Controls.Remove(num2);
                bill.Controls.Remove(num3);
                billno = 1;
            }
        }

        private void Label2_Click_1(object sender, EventArgs e)
        {

        }

        private void Button23_Click(object sender, EventArgs e)
        {
            this.Close();
            login fm1 = new login();
            fm1.Show();
        }

        private void Button24_Click(object sender, EventArgs e)
        {
            Signup fm1 = new Signup();
            fm1.Show();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            int id = 008;
            AddToBill(id);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            int id = 009;
            AddToBill(id);
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            int id = 010;
            AddToBill(id);
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            int id = 011;
            AddToBill(id);
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            int id = 012;
            AddToBill(id);
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            int id = 013;
            AddToBill(id);
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            int id = 014;
            AddToBill(id);
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            int id = 0015;
            AddToBill(id);
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            int id = 016;
            AddToBill(id);
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            int id = 017;
            AddToBill(id);
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            int id = 018;
            AddToBill(id);
        }

        private void Button19_Click(object sender, EventArgs e)
        {
            int id = 019;
            AddToBill(id);
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            int id = 020;
            AddToBill(id);
        }

        private void Button35_Click(object sender, EventArgs e)
        {
            new_item fm1 = new new_item();
            fm1.Show();
        }

        private void Button36_Click(object sender, EventArgs e)
        {
            inventory fm1 = new inventory();
            fm1.Show();
        }
    }
}
