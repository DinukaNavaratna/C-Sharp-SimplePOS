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
    public partial class new_item : Form
    {
        public new_item()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 5;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string Name = textBox1.Text;
            string Image = textBox3.Text;
            string Type = comboBox1.SelectedItem.ToString();

            if (Name == "" || textBox2.Text == null || Image == "" || Type == "")
            {
                MessageBox.Show("All the fields should be filled!");
            }
            else
            {
                double Price = double.Parse(textBox2.Text);

                db_methods db = new db_methods();
                    var tuple = db.new_item(Name, Price, Type, Image);
                    MessageBox.Show(tuple.Item1);
                if (tuple.Item2 == 1)
                {
                    this.Close();
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
