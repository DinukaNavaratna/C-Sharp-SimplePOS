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
using System.Net.Mail;
using System.Net;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class forgot_password : Form
    {
        private int remember=0;
        public forgot_password()
        {
            if (MessageBox.Show("Do you remember your account username?", "Forgot Username?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                InitializeComponent();
                remember = 1;
            }
            else
            {
                InitializeComponent();
                remember = 0;
                label1.Text = "Email";
            }
        }

        

        private void Button3_Click(object sender, EventArgs e)
        {
            if (remember == 1)
            {
                string username = textBox1.Text;

                db_methods db = new db_methods();
                var tuple = db.forgot_psw(username);
                
                if (tuple.Item4 == 1)                                                   //message_usertype
                {
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = false;
                    button4.Visible = false;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox5.Visible = true;
                    textBox1.Enabled = false;
                }
                else
                {
                    int success = string.Compare(tuple.Item2, "User not Found!");
                    if (success == 0)
                    {
                        MessageBox.Show(tuple.Item2+"\n\n");                            //user not found
                        if (!(tuple.Item3.Equals("")))
                        {
                            MessageBox.Show("Msg2 : " + tuple.Item3);                   //message_exception
                        }
                    }
                    else
                    {
                        MessageBox.Show(tuple.Item1);                                   //message_email
                        if (!(tuple.Item3.Equals("")))
                        {
                            MessageBox.Show("Msg2 : " + tuple.Item3);                   //message_exception
                        }
                        login fm2 = new login();
                        fm2.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                string email = textBox1.Text;

                db_methods db = new db_methods();
                var tuple = db.forgot_username(email);

                if ((tuple.Item1.Equals("User Found!")))
                {
                    MessageBox.Show(tuple.Item2);                                       //message_username
                }
                else
                {
                    MessageBox.Show(tuple.Item1);                                       //user not found
                }

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            login fm2 = new login();
            fm2.Show();
            this.Hide();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string admin_username = textBox2.Text;
            string admin_password = textBox3.Text;
            string new_password = textBox4.Text;
            string confirm_password = textBox5.Text;

            db_methods db = new db_methods();
            var tuple = db.user_password_reset(admin_username, admin_password, new_password, confirm_password, username);

            if ((tuple.Item1.Equals("'" + username + "' password changed successfully!")))
            {
                MessageBox.Show(tuple.Item2);                                       //message_username
            }
            else
            {
                MessageBox.Show(tuple.Item1);                                       //user not found
            }

            MessageBox.Show("Msg 1 : " + tuple.Item1);
            MessageBox.Show("Msg 2 : " + tuple.Item2);
            MessageBox.Show("Msg 3 : " + tuple.Item3);

            login fm1 = new login();
            //fm1.Show();
            //this.Hide();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            login fm2 = new login();
            fm2.Show();
            this.Hide();
        }
    }
}

