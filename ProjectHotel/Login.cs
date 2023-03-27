using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace ProjectHotel
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;port=3306;database=db_hotel;uid=root;password=;";

            // Check if the username and password fields are empty
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Please enter username and password!!", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                // Set focus to the first empty field
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    txtUsername.Focus();
                }
                else if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Focus();
                }

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
                            MessageBox.Show(this, "Login Successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                            Dashboard form2 = new Dashboard(); // buat objek baru dari Form2

                            while (reader.Read())
                            {
                                string value = reader.GetString(1); // mengambil nilai kolom pertama dari hasil query
                                Console.WriteLine(value);
                                form2.SetText(value);

                                byte[] imgData = (byte[])reader["adminImage"];
                                MemoryStream ms = new MemoryStream(imgData);
                                Image image = Image.FromStream(ms);
                                form2.SetPictureBoxImage(image);
                            }
                            form2.Show(); // tampilkan Form2
                            this.Hide(); // sembunyikan Form1
                        }
                        else
                        {
                            // Login failed, show an error message
                            MessageBox.Show(this, "Invalid username and password!!", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

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

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.label2.Parent = this.guna2PictureBox4;
            this.label3.Parent = this.guna2PictureBox4;
            this.label4.Parent = this.guna2PictureBox4;
            this.label5.Parent = this.guna2PictureBox4;
            this.label6.Parent = this.guna2PictureBox4;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
