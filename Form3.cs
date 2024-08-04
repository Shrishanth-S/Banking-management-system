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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Banking_management_system
{
    public partial class Form3 : Form
    {
        private OracleConnection connection;
        private string connectionString = "User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;";
        private string selectedBranchId;
        public Form3(string branchId)
        {
            InitializeComponent();
            selectedBranchId = branchId;
            InitializeDatabaseConnection();
            
        }

        private void InitializeDatabaseConnection()
        {
            connection = new OracleConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        private void LoadEmployees()
        {

            string query = "SELECT * FROM employee WHERE employee_id IN " +
                           "(SELECT employee_id FROM works WHERE branch_id = :branchId)";
            OracleDataAdapter adapter = new OracleDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.Add("branchId", selectedBranchId);

            DataTable employeesTable = new DataTable();
            adapter.Fill(employeesTable);

            dataGridView1.DataSource = employeesTable;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Selected Branch ID: " + selectedBranchId);
            WindowState = FormWindowState.Maximized;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM EMPLOYEE WHERE employee_id = :id";
            OracleCommand adapter = new OracleCommand(query, connection);
            adapter.Parameters.Add(":id", OracleDbType.Int32).Value = Convert.ToInt32(textBox7.Text);
            
            int rowsupdated = adapter.ExecuteNonQuery();
            if (rowsupdated > 0)
                MessageBox.Show("Employee removed");
            else
                MessageBox.Show("Please enter employee id");
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty || textBox5.Text == string.Empty || textBox6.Text == string.Empty )
                MessageBox.Show("Please enter all details");
            else if (textBox2.Text.Length > 10 && textBox2.Text.Length < 10)
                MessageBox.Show("Please enter valid phone number");
            
                using (OracleConnection connection = new OracleConnection("Data Source=localhost:1521/XEPDB1;User Id=system;Password=password"))
                {
                    connection.Open();




                    // Get the next value from the sequence
                    string getNextValueQuery = "SELECT Employee_ID_Seq.NEXTVAL FROM DUAL";
                    OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                    int newemployeeID = Convert.ToInt32(nextValCommand.ExecuteScalar());

                    // Now you can use the newCustomerID in your code or display it to the user


                    // Insert the new customer into the database with the generated ID
                    string insertQuery1 = "INSERT INTO employee (employee_id, emp_name, phone_no, email, dob, gender, role) " +
                                         "VALUES (:employeeID, :value1, :phone, :email, :dob, :gender, :role)";
                    OracleCommand insertCommand = new OracleCommand(insertQuery1, connection);
                    insertCommand.Parameters.Add(":employeeID", OracleDbType.Int32).Value = newemployeeID;
                    insertCommand.Parameters.Add(":value1", OracleDbType.Varchar2).Value = textBox1.Text; // Example value
                    insertCommand.Parameters.Add(":phone", OracleDbType.Int64).Value = Convert.ToInt64(textBox2.Text);
                    insertCommand.Parameters.Add(":email", OracleDbType.Varchar2).Value = textBox6.Text;
                    insertCommand.Parameters.Add(":dob", OracleDbType.Date).Value = dateTimePicker1.Value;
                    insertCommand.Parameters.Add(":gender", OracleDbType.Varchar2).Value = textBox3.Text;
                    insertCommand.Parameters.Add(":role", OracleDbType.Varchar2).Value = textBox5.Text;

                    string insertQuery2 = "INSERT INTO works (employee_id, branch_id) " +
                                         "VALUES (:employeeID, :branchid)";
                    OracleCommand insertCommand2 = new OracleCommand(insertQuery2, connection);
                    insertCommand2.Parameters.Add(":employeeID", OracleDbType.Int32).Value = newemployeeID;
                    insertCommand2.Parameters.Add(":branchid", OracleDbType.Varchar2).Value = selectedBranchId;


                    int rowsupdated = insertCommand.ExecuteNonQuery();
                    int rowsupdated2 = insertCommand2.ExecuteNonQuery();
                    if (rowsupdated > 0 && rowsupdated2 > 0)
                    {

                        MessageBox.Show("Employee added");
                    }

                    else MessageBox.Show("Employee is not added");


                    connection.Close();
                }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query = "SELECT customer.customer_id , cust_name FROM (customer left join has on customer.customer_id = has.customer_id) join has_accnts using (account_no) WHERE branch_id = :bid";
                           
            OracleDataAdapter adapter = new OracleDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.Add("bid", selectedBranchId);

            DataTable employeesTable = new DataTable();
            adapter.Fill(employeesTable);

            dataGridView2.DataSource = employeesTable;
        }

        //complex query
        private void button5_Click(object sender, EventArgs e)
        {

            string query = "SELECT *\r\nFROM (\r\n    (\r\n        transaction\r\n        NATURAL JOIN sender_accnt\r\n    )\r\n    JOIN receiver_accnt USING (transaction_id)\r\n)\r\nJOIN has_accnts ON sender_accnt.sender_account_no = has_accnts.account_no OR receiver_accnt.receiver_account_no = has_accnts.account_no where branch_id = :bid";
            OracleDataAdapter adapter = new OracleDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.Add("bid", selectedBranchId);

            DataTable employeesTable = new DataTable();
            adapter.Fill(employeesTable);

            dataGridView3.DataSource = employeesTable;
        }
    }
}
