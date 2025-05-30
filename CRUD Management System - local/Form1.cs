using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient; // added this line because im using sql server database


namespace CRUD_Management_System___local
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=InventoryDB;Integrated Security=True;Pooling=False;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Items", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }


        private void Form1_Load(object sender, EventArgs e) // when the form loads, this method will be called
        {
            LoadData(); // calling it to run as soon as From 1 runs (when the app starts)   
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // id
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) // name
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e) // quantity
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e) // price
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) // the data grid view
        {

        }

        private void button1_Click(object sender, EventArgs e) // submit to db button
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Items (Name, Quantity, Price) VALUES (@name, @qty, @price)", conn);
                cmd.Parameters.AddWithValue("@name", textBox2.Text); // name is a string
                cmd.Parameters.AddWithValue("@qty", int.Parse(textBox3.Text)); // quantity is an integer
                cmd.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text)); // price is a decimal
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item added successfully.");
                LoadData(); // refresh grid
            }
        }

        private void button2_Click(object sender, EventArgs e) // close app button
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) // when double click on the data grid view row
        {

        }
    }
}
