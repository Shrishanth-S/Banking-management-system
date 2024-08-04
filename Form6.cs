using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banking_management_system
{
    public partial class Form6 : Form
    {
        int selected_acc_no;
        public Form6(int acc_no)
        {
            InitializeComponent();
            selected_acc_no = acc_no;
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            string query = "SELECT account_no FROM account";
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                try
                {
                    connection.Open(); // Open the connection before executing the query
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(reader["account_no"].ToString());
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

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
                connection.Open();
                string sqlUpdateBalance = "UPDATE account SET balance = balance + :depositAmount WHERE account_no = :accountNumber";

                OracleCommand command = new OracleCommand(sqlUpdateBalance, connection);

                command.Parameters.Add(":depositAmount", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);
                command.Parameters.Add(":accountNumber", OracleDbType.Int32).Value = selected_acc_no;

                int rowsUpdated = command.ExecuteNonQuery();




                string getNextValueQuery = "SELECT transaction_id_seq.NEXTVAL FROM DUAL";
                OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                int newtid = Convert.ToInt32(nextValCommand.ExecuteScalar());


                string sqlInserttransaction = "INSERT INTO TRANSACTION (transaction_id, transaction_type, amount, transaction_date) VALUES (:id, :type, :amt, SYSDATE)";
                OracleCommand command2 = new OracleCommand(sqlInserttransaction, connection);

                command2.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                command2.Parameters.Add(":type", OracleDbType.Varchar2).Value = "DEPOSIT";
                command2.Parameters.Add(":amt", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);

                int rowsUpdated2 = command2.ExecuteNonQuery();




                string sqlInsertreceiver = "INSERT INTO receiver_accnt (transaction_id, receiver_account_no)"
                                            + "VALUES (:id, :acc_no) ";
                OracleCommand command3 = new OracleCommand(sqlInsertreceiver, connection);

                command3.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                command3.Parameters.Add(":acc_no", OracleDbType.Int32).Value = selected_acc_no;



                int rowsUpdated3 = command3.ExecuteNonQuery();




                string sqlInsertsender = "INSERT INTO sender_accnt (transaction_id, sender_account_no) VALUES (:id, :acc_no)";


                OracleCommand command4 = new OracleCommand(sqlInsertsender, connection);

                command4.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                command4.Parameters.Add(":acc_no", OracleDbType.Int32).Value = null;



                int rowsUpdated4 = command4.ExecuteNonQuery();



                if (rowsUpdated > 0 && rowsUpdated2 > 0 && rowsUpdated3 > 0 && rowsUpdated4 > 0)
                {
                    MessageBox.Show("Amount deposited successfully");
                }
                else MessageBox.Show("Deposition failed");

            }
            else MessageBox.Show("Please enter amount");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            connection.Open();
            string query = "SELECT balance FROM account WHERE account_no = :accountId";
            OracleCommand command34 = new OracleCommand(query, connection);
            command34.Parameters.Add(":accountId", selected_acc_no);
            decimal currentBalance = (decimal)command34.ExecuteScalar();
            if (textBox1.Text != string.Empty)
            {
                if (currentBalance >= Convert.ToInt64(textBox1.Text))
                {

                    string sqlUpdateBalance = "UPDATE account SET balance = balance - :depositAmount WHERE account_no = :accountNumber";

                    OracleCommand command = new OracleCommand(sqlUpdateBalance, connection);

                    command.Parameters.Add(":depositAmount", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);
                    command.Parameters.Add(":accountNumber", OracleDbType.Int32).Value = selected_acc_no;

                    int rowsUpdated = command.ExecuteNonQuery();




                    string getNextValueQuery = "SELECT transaction_id_seq.NEXTVAL FROM DUAL";
                    OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                    int newtid = Convert.ToInt32(nextValCommand.ExecuteScalar());


                    string sqlInserttransaction = "INSERT INTO TRANSACTION (transaction_id, transaction_type, amount, transaction_date) VALUES (:id, :type, :amt, SYSDATE)";
                    OracleCommand command2 = new OracleCommand(sqlInserttransaction, connection);

                    command2.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                    command2.Parameters.Add(":type", OracleDbType.Varchar2).Value = "WITHDRAW";
                    command2.Parameters.Add(":amt", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);

                    int rowsUpdated2 = command2.ExecuteNonQuery();




                    string sqlInsertreceiver = "INSERT INTO receiver_accnt (transaction_id, receiver_account_no)"
                                                + "VALUES (:id, :acc_no) ";
                    OracleCommand command3 = new OracleCommand(sqlInsertreceiver, connection);

                    command3.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                    command3.Parameters.Add(":acc_no", OracleDbType.Int32).Value = null;



                    int rowsUpdated3 = command3.ExecuteNonQuery();




                    string sqlInsertsender = "INSERT INTO sender_accnt (transaction_id, sender_account_no) VALUES (:id, :acc_no)";


                    OracleCommand command4 = new OracleCommand(sqlInsertsender, connection);

                    command4.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                    command4.Parameters.Add(":acc_no", OracleDbType.Int32).Value = selected_acc_no;



                    int rowsUpdated4 = command4.ExecuteNonQuery();



                    if (rowsUpdated > 0 && rowsUpdated2 > 0 && rowsUpdated3 > 0 && rowsUpdated4 > 0)
                    {
                        MessageBox.Show("Amount withdrawn successfully");
                    }
                    else MessageBox.Show("Withdrawal failed");
                }
                else MessageBox.Show("Insufficient balance");
            }
            else MessageBox.Show("Please enter amount");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            connection.Open();

            string query = "SELECT balance FROM account WHERE account_no = :acc_no ";

            OracleDataAdapter adapter = new OracleDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.Add("acc_no", selected_acc_no);

            DataTable balanceTable = new DataTable();
            adapter.Fill(balanceTable);

            dataGridView2.DataSource = balanceTable;
        }

        //complex query
        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;";
            string query = "SELECT * FROM (transaction t NATURAL JOIN sender_accnt s) JOIN receiver_accnt r USING (transaction_id) WHERE s.sender_account_no = :acc_no1 OR r.receiver_account_no = :acc_no2";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter(":acc_no1", OracleDbType.Int32)).Value = selected_acc_no;
                    command.Parameters.Add(new OracleParameter(":acc_no2", OracleDbType.Int32)).Value = selected_acc_no;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);
                    DataTable transactionTable = new DataTable();
                    adapter.Fill(transactionTable);

                    dataGridView1.DataSource = transactionTable;
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            connection.Open();
            string query = "SELECT balance FROM account WHERE account_no = :accountId";
            OracleCommand command = new OracleCommand(query, connection);
            command.Parameters.Add(":accountId", selected_acc_no);
            decimal currentBalance = (decimal)command.ExecuteScalar();
            
            
            if (textBox1.Text != string.Empty)
            {
                if (comboBox2.SelectedItem != null)
                {
                    if (currentBalance >= Convert.ToInt64(textBox1.Text))
                    {


                        string sqlUpdateBalance = "UPDATE account SET balance = balance - :depositAmount WHERE account_no = :accountNumber";

                        OracleCommand command2 = new OracleCommand(sqlUpdateBalance, connection);

                        command2.Parameters.Add(":depositAmount", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);
                        command2.Parameters.Add(":accountNumber", OracleDbType.Int32).Value = selected_acc_no;

                        int rowsUpdated = command2.ExecuteNonQuery();

                        string sqlUpdateBalance2 = "UPDATE account SET balance = balance + :depositAmount WHERE account_no = :accountNumber";

                        OracleCommand command3 = new OracleCommand(sqlUpdateBalance2, connection);

                        command3.Parameters.Add(":depositAmount", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);
                        command3.Parameters.Add(":accountNumber", OracleDbType.Int32).Value = Convert.ToInt32(comboBox2.SelectedItem);

                        int rowsUpdated2 = command3.ExecuteNonQuery();


                        string getNextValueQuery = "SELECT transaction_id_seq.NEXTVAL FROM DUAL";
                        OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                        int newtid = Convert.ToInt32(nextValCommand.ExecuteScalar());


                        string sqlInserttransaction = "INSERT INTO TRANSACTION (transaction_id, transaction_type, amount, transaction_date) VALUES (:id, :type, :amt, SYSDATE)";
                        OracleCommand command4 = new OracleCommand(sqlInserttransaction, connection);

                        command4.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                        command4.Parameters.Add(":type", OracleDbType.Varchar2).Value = "TRANSFER";
                        command4.Parameters.Add(":amt", OracleDbType.Int64).Value = Convert.ToInt64(textBox1.Text);

                        int rowsUpdated3 = command4.ExecuteNonQuery();




                        string sqlInsertreceiver = "INSERT INTO receiver_accnt (transaction_id, receiver_account_no)"
                                                    + "VALUES (:id, :acc_no) ";
                        OracleCommand command5 = new OracleCommand(sqlInsertreceiver, connection);

                        command5.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                        command5.Parameters.Add(":acc_no", OracleDbType.Int32).Value = Convert.ToInt32(comboBox2.SelectedItem);



                        int rowsUpdated4 = command5.ExecuteNonQuery();




                        string sqlInsertsender = "INSERT INTO sender_accnt (transaction_id, sender_account_no) VALUES (:id, :acc_no)";


                        OracleCommand command6 = new OracleCommand(sqlInsertsender, connection);

                        command6.Parameters.Add(":id", OracleDbType.Int32).Value = newtid;
                        command6.Parameters.Add(":acc_no", OracleDbType.Int32).Value = selected_acc_no;



                        int rowsUpdated5 = command6.ExecuteNonQuery();

                        

                        if (rowsUpdated > 0 && rowsUpdated2 > 0 && rowsUpdated3 > 0 && rowsUpdated4 > 0 && rowsUpdated5 > 0)
                        {
                            MessageBox.Show("Amount transferred successfully");
                        }
                        else MessageBox.Show("Transfer failed");
                    }
                    else MessageBox.Show("Insufficient balance");

                }
                else MessageBox.Show("Please enter account number you wish to transfer the amount to");
            }
            else MessageBox.Show("Please enter amount");
        }
    }

    
}

