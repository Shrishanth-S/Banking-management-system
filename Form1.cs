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
    public partial class Form1 : Form
    {
        private string username, password;
        OracleConnection conn;
        public void ConnectDb()
        {
            conn = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;");
            conn.Open();
            MessageBox.Show("Connected");
        }


        
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;    
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
           
            using (OracleConnection connection = new OracleConnection("User Id=system;Password=password;Data Source=localhost:1521/XEPDB1;"))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected to Oracle database.");

                    
                   

                    // Validate login credentials
                    bool isValidLogin = ValidateLogin(connection, textBox1.Text, textBox2.Text);

                    // Check if login was successful
                    if (isValidLogin)
                    {
                        username = textBox1.Text;
                        password = textBox2.Text;   
                        MessageBox.Show("Login successful!");
                        Form5 frm5 = new Form5(username,password);
                        frm5.Show();
                        // Add your code here for what to do after successful login
                    }

                    else if (textBox1.Text == "Admin" && textBox2.Text == "Admin123")
                    {
                        MessageBox.Show("Welcome Admin");
                        Form2 form2 = new Form2();
                        form2.Show();
                    }

                    else
                    {
                        MessageBox.Show("Invalid username or password!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        static bool ValidateLogin(OracleConnection connection, string username, string password)
        {
            string query = "SELECT * FROM customer WHERE username = :username AND password = :password";
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                command.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }




        private void button2_Click(object sender, EventArgs e)
        {
            Form4 secondForm = new Form4();
            secondForm.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
