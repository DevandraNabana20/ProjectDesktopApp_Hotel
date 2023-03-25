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
using MySql.Data.MySqlClient;

namespace ProjectHotel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_hotel;uid=root;password=;";

            // Check if the username and password fields are empty
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Please enter username and password!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            // Hash the password using MD5
            string hashedPassword = CalculateMD5Hash(txtPassword.Text);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create a MySQL query to select the user with the specified username and hashed password
                string query = "SELECT * FROM admin WHERE adminUsername = @username AND adminPassword = @password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@username", txtUsername.Text);
                    command.Parameters.AddWithValue("@password", hashedPassword);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Login successful, do something here like opening a new form
                            MessageBox.Show(this, "Login Successfull", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                            Form2 form2 = new Form2(); // buat objek baru dari Form2
                            form2.Show(); // tampilkan Form2
                            this.Hide(); // sembunyikan Form1
                        }
                        else
                        {
                            // Login failed, show an error message
                            MessageBox.Show(this, "Invalid username and password!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                        }
                    }
                }
            }
        }
        private static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
