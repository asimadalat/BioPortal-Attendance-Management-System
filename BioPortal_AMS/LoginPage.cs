using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SQLite; // Database handling
using MySql.Data; // Database handling
using MySql.Data.MySqlClient; // Database handling
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography; // Hashing

namespace BioPortal_AMS
{
    public partial class LoginPage : Form
    {
        MySql.Data.MySqlClient.MySqlConnection myConnection;
        // Define server, user, password and database name
        string ConnectionString = "server=127.0.0.1;uid=user;pwd=password;database=database_name";

        private Form1 mainPage;

        public LoginPage(Form1 main)
        {
            InitializeComponent();
            mainPage = main;
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

        }

        private string hash(string s)
        {
            /**
                Hashes inputted str using SHA-256
                Parameters: s - String to be hashed
                Returns: hashed - Hashed string
            */

            var sha256 = SHA256.Create();
            byte[] bytearray = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            string hashed = "";
            for (int i = 0; i < bytearray.Length; i++)
            {
                hashed += (bytearray[i].ToString("x2"));
            }
            return hashed.ToString();
        }

        public bool login_valid(string email, string pw)
        {
            /**
                Checks inputted email, pw against stored email, pw, returns true if match
                Parameters: email - Inputted email to be checked
                            pw - Inputted password to be checked
                Returns: True - Successful match / False - Credentials invalid
            */

            using (var connection = new MySqlConnection(ConnectionString)) // Make temp connection
            {
                connection.Open(); // Open connection
                using (var command = new MySqlCommand()) // Create command
                {
                    command.Connection = connection; // Using open connection
                    command.CommandText = "SELECT password FROM logins WHERE email = @email";

                    // Parametise command to prevent SQLi
                    command.Parameters.AddWithValue("@email", email);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var splitPass = reader.GetString(0).Split("$");
                            pw += splitPass[1];
                            if (hash(pw) == splitPass[0]) // If user login details are correct
                            {
                                return true;
                            }
                            return false;
                        }
                        return false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*') // If password is currently hidden
            {
                textBox2.PasswordChar = '\0'; // Show password
            }
            else // Else if password is showing
            {
                textBox2.PasswordChar = '*'; // Hide password
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Take inputs of textBox1 and textBox2 as username and password
            if (login_valid(textBox1.Text, textBox2.Text))
            {
                // When login button is clicked, hide current page, only show main page
                mainPage.log_user(textBox1.Text);
                mainPage.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credentials invalid.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Close application
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Open register page
            this.Hide();
            RegisterPage register = new RegisterPage();
            register.Show();
        }

        private void LoginPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // Prevent any background threads or forms running when app is closed
        }
    }
}
