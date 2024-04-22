using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data; // Database handling
using MySql.Data.MySqlClient; // Database handling
// using System.Data.SQLite; 
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography; // Hashing
using System.Xaml.Permissions; 

namespace BioPortal_AMS
{
    public partial class RegisterPage : Form
    {
        // Use relative over absolute path
        //string dbPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..", "students.db")); // Navigate to working directory
        //private string ConnectionString; // Initialise ConnectionString
        MySql.Data.MySqlClient.MySqlConnection myConnection;
        //set the correct values for your server, user, password and database name
        string ConnectionString = "server=127.0.0.1;uid=user;pwd=password;database=database_name";

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterPage_Load(object sender, EventArgs e)
        {

        }

        public string hash(string s)
        {
            /**
                Hashes inputted str using SHA-256
                Parameters: s - String to be hashed
                Returns: hashed - Hashed string
            */

            var sha256 = SHA256.Create(); // New instance of Sha256 
            // Compute hash on s after converting to bytearray using utf-8
            byte[] bytearray = sha256.ComputeHash(Encoding.UTF8.GetBytes(s)); 
            string hashed = ""; // Initialise empty hash str
            for (int i = 0; i < bytearray.Length; i++) // For each value in bytearray
            {
                hashed += (bytearray[i].ToString("x2")); // Convert to str (2-char hex)
            }
            return hashed.ToString();
        }

        public string randSalt()
        {
            /**
                Generates salt value using random alphanumeric combination
                Parameters: Letters - Characters to be picked from at random
                Returns: newSalt - Random salt value
            */
            string Letters = "6kXpx7VTYg5HrhO4JMsBmSiDoau0GldzCtNe1Efn8RbjyQqFwPZcIU3KLvW9A2";

            var newSalt = "";
            Random rand = new Random(); // New random object
            for (int i = 0; i < rand.Next(5, 11); i++) // Min length 5, max length 11
            {
                int letterIndex = rand.Next(Letters.Length);
                newSalt += Letters[letterIndex];
            }
            return newSalt;
        }

        private bool details_valid(string inst_code, string password)
        {
            /**
                Checks if password meets security requirements and if institution exists, return true if conditions met
                Parameters: inst_code - Inputted institution code to be checked
                            password - Inputted password to be checked
                Returns: True - Validation successful / False - Password or institution invalid
            */

            if (password.Any(Char.IsLetter) && password.Any(Char.IsDigit))
            {
                if (password.Length > 8)
                {
                    using (var connection = new MySqlConnection(ConnectionString)) // Make temp connection
                    {
                        connection.Open(); // Open connection
                        using (var command = new MySqlCommand()) // Create command
                        {
                            command.Connection = connection; // Using open connection
                            command.CommandText = "SELECT * FROM academicinstitutions WHERE institutionCode = @code";

                            // Parametise command to prevent SQLi
                            command.Parameters.AddWithValue("@code", inst_code);

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show("Please enter a valid institution.");
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Passwords must be greater than 8 characters in length.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Passwords must contain a mix of alphanumeric characters.");
                return false;
            }
        }

        private void ins_entry(string inst_code, string email, string fullname, string password)
        {
            /**
                Creates new login record within MySQL db
                Parameters: inst_code - Institution code to insert
                            email - Email to insert
                            fullname - Name to insert
                            password - password to salt, hash and insert
                Returns: N/A
            */

            string[] splitname = fullname.Split(" ");
            string firstname = splitname[0];
            string surname = splitname[splitname.Length-1];

            string gensalt = randSalt();
            string hashedpass = hash(password + gensalt);
            string dbpass = hashedpass + '$' + gensalt;

            using (var connection = new MySqlConnection(ConnectionString)) // Make temp connection
            {
                connection.Open(); // Open connection
                using (var command = new MySqlCommand()) // Create command
                {
                    command.Connection = connection; // Using open connection

                    command.CommandText = "INSERT INTO Logins (institutionCode, email, password, firstname, surname) VALUES (@code, @email, @pass, @firstname, @surname)";

                    // Parametise command to prevent SQLi
                    command.Parameters.AddWithValue("@code", inst_code);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@pass", dbpass);
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@surname", surname);

                    command.ExecuteNonQuery(); // Execute command
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox5.PasswordChar == '*') // If password is currently hidden
            {
                textBox5.PasswordChar = '\0'; // Show password
            }
            else // Else if password is showing
            {
                textBox5.PasswordChar = '*'; // Hide password
            }
        }

        Form1 form1 = new Form1();

        private void button2_Click(object sender, EventArgs e)
        {
            // Take inputs of textBox1 and textBox2 as username and password
            if (details_valid(textBox1.Text, textBox5.Text))
            {
                ins_entry(textBox1.Text, textBox3.Text, textBox4.Text, textBox5.Text);

                // If registration successful, hide current page, only show login page
                this.Hide();
                LoginPage Login = new LoginPage(form1);
                Login.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Back to login
            this.Hide();
            LoginPage Login = new LoginPage(form1);
            Login.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString)) // Make temp connection
            {
                connection.Open(); // Open connection
                using (MySqlCommand command = new MySqlCommand()) // Create command
                {
                    command.Connection = connection; // Using the open connection

                    command.CommandText = "SELECT institution FROM academicinstitutions WHERE institutioncode = @code";

                    // Parametise command to prevent SQLi
                    command.Parameters.AddWithValue("@code", textBox1.Text);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            textBox2.Text = reader.GetString(0);
                        }
                        else if (textBox1.Text == "")
                        {
                            textBox2.Text = "";
                        }
                        else
                        {
                            textBox2.Text = "Not Found";
                        }
                    }
                }
            }
        }
    }
}
