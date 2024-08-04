using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;



namespace Banking_management_system
{
    public partial class Form2 : Form
    {
        OracleConnection conn;
        private string selectedBranchId;
        public void ConnectDb()
        {
            conn = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            conn.Open();
            MessageBox.Show("Connected");
        }




        public Form2()
        {
            InitializeComponent();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load branch names into dropdown list on form load
            LoadBranches();
        }

        private void LoadBranches()
        {
            try
            {
                string query = "SELECT branch_id ,city FROM Branch";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Bind branch names to ComboBox

                comboBox1.ValueMember = "branch_id";
                comboBox1.DisplayMember = "city";
                comboBox1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading branches: " + ex.Message);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                MessageBox.Show("Please select branch");
            else
            {
                selectedBranchId = comboBox1.SelectedItem.ToString();
                Form3 frm3 = new Form3(selectedBranchId);
                frm3.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        { 
            using (var conn = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;"))
            using (var cmd = new OracleCommand("SELECT * FROM Branch", conn))

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

       


        private void button1_Click(object sender, EventArgs e)
        {
            Form8 frm8 = new Form8();
            frm8.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
                OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
                string query = "SELECT branch_id FROM branch";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        connection.Open(); // Open the connection before executing the query
                        OracleDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["branch_id"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading data: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close(); // Close the connection after data retrieval
                    }
                }
            }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    }


    

