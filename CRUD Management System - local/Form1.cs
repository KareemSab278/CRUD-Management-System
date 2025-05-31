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
        int selectedId = 0;
        string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=InventoryDB;Integrated Security=True;Pooling=False;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }

        // ===========================================================================================================

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Name, Quantity, Price FROM Items", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        // ===========================================================================================================

        private void Form1_Load(object sender, EventArgs e) // when the form loads, this method will be called
        {
            LoadData(); // calling it to run as soon as From 1 runs (when the app starts)   
        }

        // ===========================================================================================================

        private void button1_Click(object sender, EventArgs e) // submit to db button
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Items (Name, Quantity, Price) VALUES (@name, @qty, @price)", conn);
                    // i left out the ID field because it is an identity column and will auto-increment
                    cmd.Parameters.AddWithValue("@name", textBox2.Text); // name is a string
                    cmd.Parameters.AddWithValue("@qty", int.Parse(textBox3.Text)); // quantity is an integer
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text)); // price is a decimal
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item added successfully.");
                    LoadData(); // refresh grid
                }
                catch // try catch blocl added in case of an error
                {
                    MessageBox.Show("Error adding item. Please check your input values.");
                }
            }
        }

        // ===========================================================================================================

        private void dataGridView1_UserDeletingRow_1(object sender, DataGridViewRowCancelEventArgs e)
        {
            var deleteMe = e.Row.Cells[0].Value; // this gets the first cell value of the deleting row, which is the ID
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Items WHERE ID = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", deleteMe);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting item with ID: " + deleteMe + "\nError message: " + ex.Message);
                }
            }
        }

        // ===========================================================================================================

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) // when double click on the data grid view row
        {
            if (selectedId == 0)
            {
                selectedId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value); // this gets the ID of the selected row
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); // get the Name of the row
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); // get the Quantity of the row
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(); // get the Price of the row
            }
            else
            {
                MessageBox.Show("Please select a valid item to update.");
            }
        }

        // ===========================================================================================================

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if(!textBox2.Text.Trim().Equals("") && !textBox3.Text.Trim().Equals("") && !textBox4.Text.Trim().Equals("") && selectedId != 0) // check if the textboxes are not empty and if an item is selected
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE Items SET Name = @name, Quantity = @qty, Price = @price WHERE Id = @id", conn);

                        cmd.Parameters.AddWithValue("@id", selectedId);
                        cmd.Parameters.AddWithValue("@name", textBox2.Text);
                        cmd.Parameters.AddWithValue("@qty", int.Parse(textBox3.Text));
                        cmd.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item updated successfully.");
                        LoadData();
                        selectedId = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating item: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item to update and fill all fields.");
                }
                
            }
        }

    }
}
