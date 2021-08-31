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
    public partial class inventory_list : Form
    {
        string Date = DateTime.Now.ToString("dd/MM/yyyy");
        public inventory_list()
        {
            InitializeComponent();

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            view_inventory();
            this.Text = "Daily Inventory : " + DateTime.Now.ToString("dd/MM/yyyy");
            dataGridView1.Columns[0].HeaderText = "Item ID";
            dataGridView1.Columns[2].HeaderText = "Quantity";
            dataGridView1.Columns[4].HeaderText = "New Qty";
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BackgroundColor = Color.White;

            this.dataGridView1.Columns["Date"].Visible = false;
            this.dataGridView1.Columns["Remaining"].Visible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
        }

        public void view_inventory()
        {
            string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\C#\POS - Assignment\POS\WindowsFormsApplication1\Local_DB\C#_POS.mdf;Integrated Security=True";
            string query = "select * from Daily_Inventory where Date='" + Date + "'";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds, "Daily_Inventory");
                dataGridView1.DataSource = ds.Tables["Daily_Inventory"];
                this.dataGridView1.Columns.Add(this.dataGridView1.Columns[2].Clone() as DataGridViewColumn);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            db_methods db = new db_methods();
            int count = db.item_count();
            int x = 0;
            string message = "";
            for (x=-0; x<count-1; x++)
            {
                if(dataGridView1.Rows[x].Cells[4].Value != null)
                {
                    int qty = int.Parse(dataGridView1.Rows[x].Cells[4].Value.ToString());
                    message = db.update_daily_inventory(x + 1, qty, Date);
                }
                else
                {
                    MessageBox.Show("Enter new quantities");
                }
            }
            MessageBox.Show(message + "\n" + Date);
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
