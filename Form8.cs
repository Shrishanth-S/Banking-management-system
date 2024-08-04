using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banking_management_system
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty || textBox4.Text == string.Empty)
            {
                MessageBox.Show("Please enter all details");

            }
            else
            {
                using (OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;"))
                {
                    try
                    {
                        connection.Open();



                        // SQL update command
                        string sql = "UPDATE interest_rate SET Term = :term, Product_type = :p_t, interest_rate = :ir WHERE interest_rate_id = :id";

                        // Create command
                        using (OracleCommand command = new OracleCommand(sql, connection))
                        {
                            // Add parameters

                            command.Parameters.Add(":term", OracleDbType.Varchar2).Value = textBox2.Text;
                            command.Parameters.Add(":p_t", OracleDbType.Varchar2).Value = textBox3.Text;
                            command.Parameters.Add(":ir", OracleDbType.Decimal).Value = Convert.ToDecimal(textBox4.Text);
                            command.Parameters.Add(":id", OracleDbType.Varchar2).Value = textBox1.Text;



                            // Execute command
                            int rowsupdated = command.ExecuteNonQuery();
                            if (rowsupdated > 0)
                            {
                                MessageBox.Show("Row updated");
                            }
                            else MessageBox.Show("Updation unsuccessfull");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;"))
            using (var cmd = new OracleCommand("SELECT * FROM interest_rate", conn))

            {
                conn.Open();
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }
    }
}
