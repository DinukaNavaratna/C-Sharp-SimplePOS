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

namespace WindowsFormsApplication1
{
    public partial class Signup : Form

    {
        public Signup()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string fname = textBox1.Text;
            string lname = textBox2.Text;
            string email = textBox3.Text;
            string uname = textBox4.Text;
            string psw = textBox5.Text;
            string utype = comboBox1.SelectedItem.ToString();
            
            if (fname == ""||lname == ""||uname == ""||email == ""||psw == ""||textBox6.Text == "")
            {
                MessageBox.Show("All the fields should be filled!");
            }
            else
            {
                if (psw == textBox6.Text)
                {
                    db_methods db = new db_methods();
                    int id = int.Parse(db.get_latest_id(utype));
                    MessageBox.Show("ID : "+id);
                    var tuple = db.signup(id+1, uname, psw, utype, fname, lname, email);
                    MessageBox.Show(tuple.Item1);
                    if (tuple.Item2 == 1)
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Passwords not matching!");
                }
                
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
