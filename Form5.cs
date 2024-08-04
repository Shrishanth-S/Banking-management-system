using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banking_management_system
{
    public partial class Form5 : Form
    {
        private string user, pw;
        public Form5(string username, string password)
        {
            InitializeComponent();
            user = username;
            pw = password;
        }
        
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
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
                        comboBox2.Items.Add(reader["branch_id"].ToString());
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


            string query2 = "SELECT account_no from has where customer_id = :id";
            using (OracleCommand command = new OracleCommand(query2, connection))
            {
                try
                {
                    connection.Open();
                    string sqlLoginQuery = "SELECT customer_id FROM customer WHERE username = :username AND password = :password";

                    OracleCommand loginCommand = new OracleCommand(sqlLoginQuery, connection);
                    loginCommand.Parameters.Add(":username", OracleDbType.Varchar2).Value = user;
                    loginCommand.Parameters.Add(":password", OracleDbType.Varchar2).Value = pw;

                    object customerIdObj = loginCommand.ExecuteScalar();
                    int customerId = Convert.ToInt32(customerIdObj);

                    command.Parameters.Add(":id",OracleDbType.Int32).Value = customerId;

                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["account_no"].ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            
                if (comboBox2.SelectedItem != null)
                {
                  if (!radioButton1.Checked && !radioButton2.Checked)
                  {
                    MessageBox.Show("Please select an account type (Savings or Current).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if no radio button is checked
                  }
                using (OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;"))
                    {
                        try
                        {
                            connection.Open();

                            

                            // Query to retrieve customer_id based on username and password
                            string sqlLoginQuery = "SELECT customer_id FROM customer WHERE username = :username AND password = :password";

                            OracleCommand loginCommand = new OracleCommand(sqlLoginQuery, connection);
                            loginCommand.Parameters.Add(":username", OracleDbType.Varchar2).Value = user;
                            loginCommand.Parameters.Add(":password", OracleDbType.Varchar2).Value = pw;

                            object customerIdObj = loginCommand.ExecuteScalar();
                            int customerId = Convert.ToInt32(customerIdObj);

                            string getNextValueQuery = "SELECT acc_no_seq.NEXTVAL FROM DUAL";
                            OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                            int newaccno = Convert.ToInt32(nextValCommand.ExecuteScalar());

                            string selectedBranchId = comboBox2.Text;

                            string accountType = radioButton1.Checked ? "Savings" : "Current";
                            string sqlInsertAccount = "INSERT INTO account (account_no, account_type, balance, open_date) " +
                                                      "VALUES (:acc_no, :account_type, :balance, SYSDATE)";

                            OracleCommand accountCommand = new OracleCommand(sqlInsertAccount, connection);
                            accountCommand.Parameters.Add(":acc_no", OracleDbType.Int32).Value = newaccno;
                            accountCommand.Parameters.Add(":account_type", OracleDbType.Varchar2).Value = accountType;
                            accountCommand.Parameters.Add(":balance", OracleDbType.Int64).Value = 0;
                            int rowsInserted = accountCommand.ExecuteNonQuery();

                            string sqlInsertHas = "INSERT INTO has (Account_no, customer_id) " +
                                                  "VALUES (:acc_no, :custid)";
                            OracleCommand hasCommand = new OracleCommand(sqlInsertHas, connection);
                            hasCommand.Parameters.Add(":acc_no", OracleDbType.Int32).Value = newaccno;
                            hasCommand.Parameters.Add(":custid", OracleDbType.Int32).Value = customerId;
                            int rowsInserted2 = hasCommand.ExecuteNonQuery();

                            string sqlInsertHasaccnt = "INSERT INTO HAS_ACCNTS (Account_no, branch_id) " +
                                                       "VALUES (:acc_no, :bid)";
                            OracleCommand hasAccnt = new OracleCommand(sqlInsertHasaccnt, connection);
                            hasAccnt.Parameters.Add(":acc_no", OracleDbType.Int32).Value = newaccno;
                            hasAccnt.Parameters.Add(":bid", OracleDbType.Varchar2).Value = selectedBranchId;
                            int rowsInserted3 = hasAccnt.ExecuteNonQuery();

                            if (rowsInserted > 0 && rowsInserted2 > 0 && rowsInserted3 > 0)
                            {
                                MessageBox.Show("Account created!");
                            }
                            else
                            {
                                MessageBox.Show("Account creation failed!");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a branch.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        

        private void button3_Click(object sender, EventArgs e)
        {

            Form7 frm7 = new Form7(user,pw);
            frm7.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selected_acc_no = Convert.ToInt32(comboBox1.SelectedItem); 
            if (comboBox1.SelectedItem != null)
            {
                Form6 frm6 = new Form6(selected_acc_no);
                frm6.Show();
            }
            else MessageBox.Show("Please select account");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
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
    }
    }


