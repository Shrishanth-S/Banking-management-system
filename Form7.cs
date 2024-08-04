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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Banking_management_system
{
    public partial class Form7 : Form
    {
        private string user, pw;
        public Form7(string user,string pw)
        {
            InitializeComponent();
            this.user = user;
            this.pw = pw;
            
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            
            WindowState = FormWindowState.Maximized;
            OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            string query = "SELECT interest_rate_id FROM interest_rate";
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                try
                {
                    connection.Open(); // Open the connection before executing the query
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["interest_rate_id"].ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty)
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Please select scheme");
                    return;
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

                        string getNextValueQuery = "SELECT loan_no_seq.NEXTVAL FROM DUAL";
                        OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                        int newloanno = Convert.ToInt32(nextValCommand.ExecuteScalar());

                        string selectedIR_Id = comboBox1.Text;


                        string sqlInsertloanir = "INSERT INTO loan_ir (loan_no, interest_rate_id) " +
                                                      "VALUES (:loan_no, :ir_id)";
                        OracleCommand irCommand = new OracleCommand(sqlInsertloanir, connection);
                        irCommand.Parameters.Add(":loan_no", OracleDbType.Int32).Value = newloanno;
                        irCommand.Parameters.Add(":ir_id", OracleDbType.Varchar2).Value = selectedIR_Id;

                        int rowsInserted = irCommand.ExecuteNonQuery();

                        string sqlTermQuery = "SELECT term FROM interest_rate WHERE interest_rate_id = :iir_id";
                        OracleCommand TermCommand = new OracleCommand(sqlTermQuery, connection);
                        TermCommand.Parameters.Add(":iir_id", OracleDbType.Varchar2).Value = selectedIR_Id;

                        object TermObj = TermCommand.ExecuteScalar();
                        string termm = (string)TermObj;


                        string sqlInsertloan = "INSERT INTO loan (loan_no, loan_amount , term , start_date) " +
                                                      "VALUES (:loan_no, :amt, :term, SYSDATE)";
                        OracleCommand loanCommand = new OracleCommand(sqlInsertloan, connection);
                        loanCommand.Parameters.Add(":loan_no", OracleDbType.Int32).Value = newloanno;
                        loanCommand.Parameters.Add(":amt", OracleDbType.Int32).Value = Convert.ToInt32(textBox2.Text);
                        loanCommand.Parameters.Add(":term", OracleDbType.Varchar2).Value = termm;


                        int rowsInserted2 = loanCommand.ExecuteNonQuery();







                        string sqlInsertavails = "INSERT INTO avails (loan_no, customer_id) " +
                                                      "VALUES (:loan_no, :custid)";
                        OracleCommand hasCommand = new OracleCommand(sqlInsertavails, connection);
                        hasCommand.Parameters.Add(":loan_no", OracleDbType.Int32).Value = newloanno;
                        hasCommand.Parameters.Add(":custid", OracleDbType.Int32).Value = customerId;
                        int rowsInserted3 = hasCommand.ExecuteNonQuery();


                        if (rowsInserted > 0 && rowsInserted2 > 0 || rowsInserted3 > 0)
                        {
                            MessageBox.Show("Loan applied successfully");
                        }
                        else MessageBox.Show("Loan application failed");




                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }


                }
            }
            else MessageBox.Show("Please enter amount");

        } 
        

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
