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
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Banking_management_system
{
    public partial class Form4 : Form
    {
        OracleConnection conn;
        public void ConnectDb()
        {
            conn = new OracleConnection("Data Source=localhost:1521/XEPDB1;User Id=system;Password=password");
            conn.Open();
            MessageBox.Show("Connected");
        }

        public Form4()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty || textBox5.Text == string.Empty || textBox6.Text == string.Empty || textBox7.Text == string.Empty || textBox8.Text == string.Empty )
                MessageBox.Show("Please enter all details");
            else if(textBox2.Text.Length>10 || textBox2.Text.Length<10)
            {
                MessageBox.Show("Enter valid phone number");
            }
            else
            {
                using (OracleConnection connection = new OracleConnection("Data Source=localhost:1521/XEPDB1;User Id=system;Password=password"))
                {
                    connection.Open();

                    // Create a sequence in Oracle if it doesn't already exist
                    string createSequenceQuery = "CREATE SEQUENCE CustomerID_Seq START WITH 1 INCREMENT BY 1";


                    // Get the next value from the sequence
                    string getNextValueQuery = "SELECT CustomerID_Seq.NEXTVAL FROM DUAL";
                    OracleCommand nextValCommand = new OracleCommand(getNextValueQuery, connection);
                    int newCustomerID = Convert.ToInt32(nextValCommand.ExecuteScalar());

                    // Now you can use the newCustomerID in your code or display it to the user


                    // Insert the new customer into the database with the generated ID
                    string insertQuery = "INSERT INTO Customer (customer_id, cust_name, dob, email, phone, gender, username, password) " +
                                         "VALUES (:customerID, :value1, :dob, :email, :phone, :gender, :username, :pw)";
                    OracleCommand insertCommand = new OracleCommand(insertQuery, connection);
                    insertCommand.Parameters.Add(":customerID", OracleDbType.Int32).Value = newCustomerID;
                    insertCommand.Parameters.Add(":value1", OracleDbType.Varchar2).Value = textBox1.Text; // Example value
                    insertCommand.Parameters.Add(":dob", OracleDbType.Date).Value = dateTimePicker1.Value;
                    insertCommand.Parameters.Add(":email", OracleDbType.Varchar2).Value = textBox3.Text;
                    insertCommand.Parameters.Add(":phone", OracleDbType.Int64).Value = Convert.ToInt64(textBox2.Text);
                    insertCommand.Parameters.Add(":gender", OracleDbType.Varchar2).Value = textBox7.Text;
                    insertCommand.Parameters.Add(":username", OracleDbType.Varchar2).Value = textBox6.Text;
                    insertCommand.Parameters.Add(":pw", OracleDbType.Varchar2).Value = textBox5.Text;

                    if (textBox5.Text == textBox8.Text)
                    {
                        insertCommand.ExecuteNonQuery();
                        MessageBox.Show("Registration succesfull");
                    }

                    else MessageBox.Show("Passwords do not match");


                    connection.Close();
                }
            }
            }

            private void label8_Click(object sender, EventArgs e)
            {

            }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
    }


        

    

    


