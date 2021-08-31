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

namespace WindowsFormsApplication1
{
    public partial class login : Form
    {
        int wrong_psw_count = 1;
        public login()
        {
            InitializeComponent(); 
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            forgot_password fm2 = new forgot_password();
            fm2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            label3.Text = (3-wrong_psw_count).ToString() + " attempts left!";

            if (wrong_psw_count == 3)
            {
                if (MessageBox.Show("Login with a different account?", "Forgot your password?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    button1.Enabled = true;
                    checkBox1.Visible = false;
                    button2.Enabled = false;
                    wrong_psw_count = 1;
                    label3.Text = "3 attempts left!";
                    textBox1.Text = null;
                    textBox2.Text = null;
                    return;                }
                else
                {
                    button1.Enabled = false;
                    label3.Text = "Attempts exceeded!";
                }
            }
            else if (wrong_psw_count < 2)
            {
                checkBox1.Visible = false;
            }else
            {
                button2.Enabled = true;
                checkBox1.Visible = true;
            }

            db_methods db = new db_methods();
            
            var tuple = db.log_in(username, password, wrong_psw_count);
            if (tuple.Item2 == 0)
            {
                MessageBox.Show("Message : " + tuple.Item1);
                int utype = db.user_type(username);
                main_interface fm2 = new main_interface(username, utype);
                fm2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Message : " + tuple.Item1);
                //MessageBox.Show("Wrong Pwd : " + tuple.Item2);
                wrong_psw_count = wrong_psw_count + 1;
            }
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }

        }
    }
}
