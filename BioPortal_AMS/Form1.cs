//using System.Data.SQLite;
using MySql.Data;
using MySql.Data.MySqlClient;
using Mysqlx.Session;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BioPortal_AMS
{
    public partial class Form1 : Form
    {
        MySql.Data.MySqlClient.MySqlConnection myConnection;
        // Define server, user, password and database name
        string ConnectionString = "server=127.0.0.1;uid=user;pwd=password;database=database_name";

        public Form1()
        {
            InitializeComponent();
        }

        LoginPage loginPage;
        private void Form1_Load(object sender, EventArgs e)
        {
            // Define backend procedures to run on first loading the page
            comboBox1.SelectedIndex = 0; // Set dropdown filter default value to 'All'
            refresh_table(); // Get initial student data
        }

        public void log_user(string email)
        {
            label1.Text = email;
        }



        // -------------------------REGISTER SECTION----------------------------

        private void update_attendance(string studentId, bool presentStatus)
        {
            /**
                Use student ID to switch bool value absent -> present,  vice versa
                Parameters: studentId - Student whose status requires updating
                            presentStatus - Whether student needs to be marked absent or present
                Returns: N/A
            */

            DateTime now = DateTime.Now; // Get current time to determine class of interest
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString)) // Make temp connection
            {
                connection.Open(); // Open connection
                using (MySqlCommand command = new MySqlCommand()) // Create command
                {
                    command.Connection = connection; // Using the open connection

                    if (now.Hour > 12)
                    {
                        // If afternoon, mark student for PM class
                        command.CommandText = "UPDATE StudentsPM SET Present = @Present WHERE ID = @ID";
                    }
                    else
                    {
                        // If morning, mark student for AM class
                        command.CommandText = "UPDATE StudentsAM SET Present = @Present WHERE ID = @ID";
                    }

                    // Parametise command to prevent SQLi
                    command.Parameters.AddWithValue("@Present", presentStatus ? 1 : 0);
                    command.Parameters.AddWithValue("@ID", studentId);

                    command.ExecuteNonQuery(); // Execute command
                }
            }
        }

        private void refresh_table()
        {
            /**
                Fetch data from MySQL db, fill dataGridView1
                Parameters: N/A
                Returns: N/A
            */

            string name = textBox1.Text; // Get input from textbox
            bool? present = null; // Intialise present bool
            if (comboBox1.SelectedIndex == 1)
            {
                // If second option in dropdown selected, only show present
                present = true;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                // If third option in dropdown selected, only show absent
                present = false;
            }
            DataTable dtStudents = new DataTable(); // Define datatable
            DateTime now = DateTime.Now; // Get current time
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString)) // Make temp connection
            {
                connection.Open(); // Open connection
                using (MySqlCommand command = new MySqlCommand()) // Create command
                {
                    command.Connection = connection; // Using the open connection

                    if (now.Hour > 12)
                    {
                        // If afternoon, show student from PM class
                        command.CommandText = "SELECT id, name, gender, contact, present FROM StudentsPM WHERE Name LIKE @name";
                    }
                    else
                    {
                        // If morning, show student from AM class
                        command.CommandText = "SELECT id, name, gender, contact, present FROM StudentsAM WHERE Name LIKE @name";
                    }

                    // Parametise command to prevent SQLi
                    command.Parameters.AddWithValue("@name", $"%{name}%");
                    if (present.HasValue)
                    {
                        command.CommandText += " AND Present = @present";
                        command.Parameters.AddWithValue("@present", present.Value ? 1 : 0);
                    }

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dtStudents); // Populate datatable with command results
                    }
                }
            }
            dataGridView1.DataSource = dtStudents; // Show on dataGridView object
        }

        private void button2_Click(object sender, EventArgs e)
        {
            refresh_table(); // Refresh table when refresh button clicked
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_table(); // Refresh table when dropdown value changed
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            refresh_table();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // A row must be selected
            {
                // Get ID of selected, use to update present to True then reload table
                string studentId = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();
                update_attendance(studentId, true);
                refresh_table();
            }
            else
            {
                // Error message when no student selected
                MessageBox.Show("Please select a student.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // A row must be selected
            {
                // Get ID of selected, use to update present to True then reload table
                string studentId = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                update_attendance(studentId, false);
                refresh_table();
            }
            else
            {
                // Error message when no student selected
                MessageBox.Show("Please select a student.");
            }
        }



        // ---------------------------MENU BUTTONS----------------------------

        private void button5_Click(object sender, EventArgs e)
        {
            // When logout button is clicked, hide current page, only show login page
            this.Hide();
            loginPage = new LoginPage(this);
            loginPage.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Close application
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (panel6.Visible == true && panel7.Visible == true)
            {
                panel6.Visible = false;
                panel7.Visible = false;
            }
            else if (panel11.Visible == true)
            {
                panel11.Visible = false;
            }

            panel5.Show();
            dataGridView1.Show();
            label2.Text = "Register";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (panel5.Visible == true && dataGridView1.Visible == true)
            {
                panel5.Visible = false;
                dataGridView1.Visible = false;
            }
            else if (panel11.Visible == true)
            {
                panel11.Visible = false;
            }

            label2.Text = "Analytics";
            panel6.Visible = true;
            panel7.Visible = true;
            refresh_chart();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (panel6.Visible == true && panel7.Visible == true)
            {
                panel6.Visible = false;
                panel7.Visible = false;
            }
            else if (panel5.Visible == true && dataGridView1.Visible == true)
            {
                panel5.Visible = false;
                dataGridView1.Visible = false;
            }
            panel11.Visible = true;
            label2.Text = "Settings";
        }


        // ---------------------------ANALYTICS SECTION----------------------------

        private void refresh_chart()
        {
            /**
                Populate bar chart with weekly absences from MySQL db table
                Parameters: N/A
                Returns: N/A
            */

            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString)) // Make temp connection
            {
                connection.Open(); // Open connection
                using (MySqlCommand command = new MySqlCommand()) // Create command
                {
                    command.Connection = connection; // Using the open connection

                    // Retrieve only required information in custom order
                    command.CommandText = "SELECT weekday, session, absent FROM attendanceForWeek\r\nORDER BY FIELD(weekday, 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday')";

                    var reader = command.ExecuteReader(); // Execute query

                    while (reader.Read()) // For each weekday
                    {
                        // Convert required fields to from obj to str and to int for measured metric
                        string weekday = reader["weekday"].ToString();
                        string session = reader["session"].ToString();
                        int numberPresent = Convert.ToInt32(reader["absent"]);

                        double weekdayNumeric;

                        // Selection to map weekdays to integers
                        if (weekday == "Monday")
                        {
                            weekdayNumeric = 1;
                        }
                        else if (weekday == "Tuesday")
                        {
                            weekdayNumeric = 2;
                        }
                        else if (weekday == "Wednesday")
                        {
                            weekdayNumeric = 3;
                        }
                        else if (weekday == "Thursday")
                        {
                            weekdayNumeric = 4;
                        }
                        else
                        {
                            weekdayNumeric = 5;
                        }
                        // Unique value for every day-session combination
                        weekdayNumeric += (session == "AM" ? 0.1 : 0.3); // Split AM and PM using decimal

                        // Add data point, will add two columns due to differing decimal values
                        chart1.Series["PM Session (RIGHT)"].Points.AddXY(weekdayNumeric, numberPresent);


                    }

                    // Map integers back to weekdays for x axis labelling
                    chart1.ChartAreas[0].AxisX.CustomLabels.Add(1 + 0.1, 1 + 0.3, "Monday");
                    chart1.ChartAreas[0].AxisX.CustomLabels.Add(2 + 0.1, 2 + 0.3, "Tuesday");
                    chart1.ChartAreas[0].AxisX.CustomLabels.Add(3 + 0.1, 3 + 0.3, "Wednesday");
                    chart1.ChartAreas[0].AxisX.CustomLabels.Add(4 + 0.1, 4 + 0.3, "Thursday");
                    chart1.ChartAreas[0].AxisX.CustomLabels.Add(5 + 0.1, 5 + 0.3, "Friday");
                }
            }
        }


        // ---------------------------SETTINGS SECTION----------------------------

        private void button9_Click(object sender, EventArgs e)
        {
            string password = textBox3.Text;
            string confPassword = textBox4.Text;
            LoginPage loginpage = new LoginPage(this);
            if (loginpage.login_valid(label1.Text, textBox2.Text))
            {
                if (password.Any(Char.IsLetter) && password.Any(Char.IsDigit) && password.Length > 8)
                {
                    if (password == confPassword)
                    {
                        RegisterPage registerPage = new RegisterPage();
                        string gensalt = registerPage.randSalt();
                        string hashedpass = registerPage.hash(password + gensalt);
                        string dbpass = hashedpass + '$' + gensalt;

                        using (var connection = new MySqlConnection(ConnectionString)) // Make temp connection
                        {
                            connection.Open(); // Open connection
                            using (var command = new MySqlCommand()) // Create command
                            {
                                command.Connection = connection; // Using open connection

                                command.CommandText = "UPDATE Logins SET password = @password WHERE email = @email";

                                // Parametise command to prevent SQLi
                                command.Parameters.AddWithValue("@password", dbpass);
                                command.Parameters.AddWithValue("@email", label1.Text);

                                command.ExecuteNonQuery(); // Execute command
                            }
                        }
                        registerPage.Close();
                        MessageBox.Show("Password successfully changed. Please restart application.");
                    }
                    else
                    {
                        MessageBox.Show("Passwords must match.");
                    }
                }
                else
                {
                    MessageBox.Show("Passwords must be greater than 8 characters in length and contain a mix of alphanumeric characters.");
                }
            }
            else
            {
                MessageBox.Show("Credentials invalid.");
            }
            loginpage.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button6.ForeColor = Color.FromArgb(32, 31, 31);
                button6.BackColor = Color.FromArgb(232, 231, 231);
                button7.ForeColor = Color.FromArgb(32, 31, 31);
                button7.BackColor = Color.FromArgb(232, 231, 231);
                button8.ForeColor = Color.FromArgb(32, 31, 31);
                button8.BackColor = Color.FromArgb(232, 231, 231);
                panel1.BackColor = Color.FromArgb(232, 231, 231);
                panel2.BackColor = Color.FromArgb(242, 241, 241);
                label1.BackColor = Color.FromArgb(242, 241, 241);
                label1.ForeColor = Color.Black;
                button5.BackColor = Color.FromArgb(242, 241, 241);
                button5.BackgroundImage = Properties.Resources.LogOutLight;
                panel3.BackColor = Color.FromArgb(232, 231, 231);
                pictureBox2.BackColor = Color.FromArgb(232, 231, 231);
                pictureBox2.Image = Properties.Resources.logo_bioportallightm;
                this.BackColor = Color.FromArgb(225, 225, 225);
                label2.BackColor = Color.FromArgb(225, 225, 225);
                label2.ForeColor = Color.FromArgb(25, 25, 25);
                button1.BackColor = Color.FromArgb(225, 225, 225);
                button1.ForeColor = Color.FromArgb(25, 25, 25);
                panel11.BackColor = Color.FromArgb(242, 241, 241);
                label15.BackColor = Color.FromArgb(242, 241, 241);
                label15.ForeColor = Color.FromArgb(42, 41, 41);
                textBox2.BackColor = Color.FromArgb(255, 255, 255);
                textBox3.BackColor = Color.FromArgb(255, 255, 255);
                textBox4.BackColor = Color.FromArgb(255, 255, 255);
                textBox2.ForeColor = Color.FromArgb(55, 55, 55);
                textBox3.ForeColor = Color.FromArgb(55, 55, 55);
                textBox4.ForeColor = Color.FromArgb(55, 55, 55);
                label17.ForeColor = Color.FromArgb(42, 41, 41);
                label18.ForeColor = Color.FromArgb(42, 41, 41);
                label19.ForeColor = Color.FromArgb(42, 41, 41);
                label17.BackColor = Color.FromArgb(242, 241, 241);
                label18.BackColor = Color.FromArgb(242, 241, 241);
                label19.BackColor = Color.FromArgb(242, 241, 241);
                button9.BackColor = Color.Black;
                button9.ForeColor = Color.White;
                label16.BackColor = Color.FromArgb(242, 241, 241);
                label16.ForeColor = Color.FromArgb(42, 41, 41);
                checkBox1.ForeColor = Color.FromArgb(42, 41, 41);
                checkBox1.BackColor = Color.FromArgb(242, 241, 241);
                panel4.BackColor = Color.FromArgb(242, 241, 241);
                panel5.BackColor = Color.FromArgb(242, 241, 241);
                panel6.BackColor = Color.FromArgb(242, 241, 241);
                panel7.BackColor = Color.FromArgb(242, 241, 241);
                panel8.BackColor = Color.FromArgb(42, 41, 41);
                panel9.BackColor = Color.FromArgb(42, 41, 41);
                panel10.BackColor = Color.FromArgb(42, 41, 41);
                dataGridView1.BackgroundColor = Color.FromArgb(242, 241, 241);
                dataGridView1.ForeColor = Color.FromArgb(242, 241, 241);
                button2.ForeColor = Color.FromArgb(42, 41, 41);
                button2.BackColor = Color.FromArgb(242, 241, 241);
                button3.ForeColor = Color.FromArgb(42, 41, 41);
                button3.BackColor = Color.FromArgb(242, 241, 241);
                button4.ForeColor = Color.FromArgb(42, 41, 41);
                button4.BackColor = Color.FromArgb(242, 241, 241);
                textBox1.BackColor = Color.FromArgb(242, 241, 241);
                textBox1.ForeColor = Color.FromArgb(42, 41, 41);
                comboBox1.BackColor = Color.FromArgb(242, 241, 241);
                comboBox1.ForeColor = Color.FromArgb(42, 41, 41);
                label14.BackColor = Color.FromArgb(242, 241, 241);
                label14.ForeColor = Color.FromArgb(42, 41, 41);
                label13.BackColor = Color.FromArgb(242, 241, 241);
                label13.ForeColor = Color.FromArgb(42, 41, 41);
                label12.BackColor = Color.FromArgb(242, 241, 241);
                label12.ForeColor = Color.FromArgb(42, 41, 41);
                label11.BackColor = Color.FromArgb(242, 241, 241);
                label11.ForeColor = Color.FromArgb(42, 41, 41);
                label10.BackColor = Color.FromArgb(242, 241, 241);
                label10.ForeColor = Color.FromArgb(42, 41, 41);
                label9.BackColor = Color.FromArgb(242, 241, 241);
                label9.ForeColor = Color.FromArgb(42, 41, 41);
                label8.BackColor = Color.FromArgb(242, 241, 241);
                label8.ForeColor = Color.FromArgb(42, 41, 41);
                label7.BackColor = Color.FromArgb(242, 241, 241);
                label7.ForeColor = Color.FromArgb(42, 41, 41);
                label6.BackColor = Color.FromArgb(242, 241, 241);
                label6.ForeColor = Color.FromArgb(42, 41, 41);
                label5.BackColor = Color.FromArgb(242, 241, 241);
                label5.ForeColor = Color.FromArgb(42, 41, 41);
                label4.BackColor = Color.FromArgb(242, 241, 241);
                label4.ForeColor = Color.FromArgb(42, 41, 41);
                label3.BackColor = Color.FromArgb(242, 241, 241);
                label3.ForeColor = Color.FromArgb(42, 41, 41);
            }
            else
            {
                button6.ForeColor = Color.FromArgb(232, 231, 231);
                button6.BackColor = Color.FromArgb(32, 31, 31);
                button7.ForeColor = Color.FromArgb(232, 231, 231);
                button7.BackColor = Color.FromArgb(32, 31, 31);
                button8.ForeColor = Color.FromArgb(232, 231, 231);
                button8.BackColor = Color.FromArgb(32, 31, 31);
                panel1.BackColor = Color.FromArgb(32, 31, 31);
                panel2.BackColor = Color.FromArgb(42, 41, 41);
                label1.BackColor = Color.FromArgb(42, 41, 41);
                label1.ForeColor = Color.White;
                button5.BackColor = Color.FromArgb(42, 41, 41);
                button5.BackgroundImage = Properties.Resources.LogOut;
                panel3.BackColor = Color.FromArgb(32, 31, 31);
                pictureBox2.BackColor = Color.FromArgb(32, 31, 31);
                pictureBox2.Image = Properties.Resources.logo_bioportaldarkm;
                this.BackColor = Color.FromArgb(25, 25, 25);
                label2.BackColor = Color.FromArgb(25, 25, 25);
                label2.ForeColor = Color.FromArgb(225, 225, 225); ;
                button1.BackColor = Color.FromArgb(25, 25, 25);
                button1.ForeColor = Color.FromArgb(225, 225, 225);
                panel11.BackColor = Color.FromArgb(42, 41, 41);
                label15.BackColor = Color.FromArgb(42, 41, 41);
                label15.ForeColor = Color.FromArgb(242, 241, 241);
                textBox2.BackColor = Color.FromArgb(55, 55, 55);
                textBox3.BackColor = Color.FromArgb(55, 55, 55);
                textBox4.BackColor = Color.FromArgb(55, 55, 55);
                textBox2.ForeColor = Color.FromArgb(255, 255, 255);
                textBox3.ForeColor = Color.FromArgb(255, 255, 255);
                textBox4.ForeColor = Color.FromArgb(255, 255, 255);
                label17.ForeColor = Color.FromArgb(242, 241, 241);
                label18.ForeColor = Color.FromArgb(242, 241, 241);
                label19.ForeColor = Color.FromArgb(242, 241, 241);
                label17.BackColor = Color.FromArgb(42, 41, 41);
                label18.BackColor = Color.FromArgb(42, 41, 41);
                label19.BackColor = Color.FromArgb(42, 41, 41);
                button9.BackColor = Color.White;
                button9.ForeColor = Color.Black;
                label16.BackColor = Color.FromArgb(42, 41, 41);
                label16.ForeColor = Color.FromArgb(242, 241, 241);
                checkBox1.ForeColor = Color.FromArgb(242, 241, 241);
                checkBox1.BackColor = Color.FromArgb(42, 41, 41);
                panel4.BackColor = Color.FromArgb(42, 41, 41);
                panel5.BackColor = Color.FromArgb(42, 41, 41);
                panel6.BackColor = Color.FromArgb(42, 41, 41);
                panel7.BackColor = Color.FromArgb(42, 41, 41);
                panel8.BackColor = Color.FromArgb(242, 241, 241);
                panel9.BackColor = Color.FromArgb(242, 241, 241);
                panel10.BackColor = Color.FromArgb(242, 241, 241);
                dataGridView1.BackgroundColor = Color.FromArgb(42, 41, 41);
                dataGridView1.ForeColor = Color.FromArgb(242, 241, 241);
                button2.ForeColor = Color.FromArgb(242, 241, 241);
                button2.BackColor = Color.FromArgb(42, 41, 41);
                button3.ForeColor = Color.FromArgb(242, 241, 241);
                button3.BackColor = Color.FromArgb(42, 41, 41);
                button4.ForeColor = Color.FromArgb(242, 241, 241);
                button4.BackColor = Color.FromArgb(42, 41, 41);
                textBox1.BackColor = Color.FromArgb(55, 55, 55);
                textBox1.ForeColor = Color.FromArgb(255, 255, 255);
                comboBox1.BackColor = Color.FromArgb(55, 55, 55);
                comboBox1.ForeColor = Color.FromArgb(255, 255, 255);
                label14.BackColor = Color.FromArgb(42, 41, 41);
                label14.ForeColor = Color.FromArgb(242, 241, 241);
                label13.BackColor = Color.FromArgb(42, 41, 41);
                label13.ForeColor = Color.FromArgb(242, 241, 241);
                label12.BackColor = Color.FromArgb(42, 41, 41);
                label12.ForeColor = Color.FromArgb(242, 241, 241);
                label11.BackColor = Color.FromArgb(42, 41, 41);
                label11.ForeColor = Color.FromArgb(242, 241, 241);
                label10.BackColor = Color.FromArgb(42, 41, 41);
                label10.ForeColor = Color.FromArgb(242, 241, 241);
                label9.BackColor = Color.FromArgb(42, 41, 41);
                label9.ForeColor = Color.FromArgb(242, 241, 241);
                label8.BackColor = Color.FromArgb(42, 41, 41);
                label8.ForeColor = Color.FromArgb(242, 241, 241);
                label7.BackColor = Color.FromArgb(42, 41, 41);
                label7.ForeColor = Color.FromArgb(242, 241, 241);
                label6.BackColor = Color.FromArgb(42, 41, 41);
                label6.ForeColor = Color.FromArgb(242, 241, 241);
                label5.BackColor = Color.FromArgb(42, 41, 41);
                label5.ForeColor = Color.FromArgb(242, 241, 241);
                label4.BackColor = Color.FromArgb(42, 41, 41);
                label4.ForeColor = Color.FromArgb(242, 241, 241);
                label3.BackColor = Color.FromArgb(42, 41, 41);
                label3.ForeColor = Color.FromArgb(242, 241, 241);

            }
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // Prevent any background threads or forms running when app is closed
        }
    }
}
