using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProjectHotel.Login;

namespace ProjectHotel
{
    public partial class InputGuest : Form
    {
        
        public InputGuest()
        {
            InitializeComponent();
            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData("Select * from guest;");

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns[0].HeaderText = "Guest Code";
            guna2DataGridView1.Columns[1].HeaderText = "Guest ID Number";
            guna2DataGridView1.Columns[2].HeaderText = "Guest Name";
            guna2DataGridView1.Columns[3].HeaderText = "Guest DOB";
            guna2DataGridView1.Columns[4].HeaderText = "Guest Gender";
            guna2DataGridView1.Columns[5].HeaderText = "Guest Address";
            guna2DataGridView1.Columns[6].HeaderText = "Guest Telephone Number";
            
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void InputGuest_Load(object sender, EventArgs e)
        {
            this.label1.Parent = this.guna2PictureBox1;
            this.label2.Parent = this.guna2PictureBox1;
            this.label3.Parent = this.guna2PictureBox1;
            this.label4.Parent = this.guna2PictureBox1;
            this.label5.Parent = this.guna2PictureBox1;
            this.label6.Parent = this.guna2PictureBox1;
            this.label7.Parent = this.guna2PictureBox1;
            this.label8.Parent = this.guna2PictureBox1;
            this.label10.Parent = this.guna2PictureBox1;
            this.label11.Parent = this.guna2PictureBox1;
            this.label12.Parent = this.guna2PictureBox1;
            this.label13.Parent = this.guna2PictureBox1;
            this.label14.Parent = this.guna2PictureBox1;
            this.label15.Parent = this.guna2PictureBox1;

        }
        private void ResetFields()
        {
            txtIdentity.Text = "";
            txtNama.Text = "";
            txtTelephone.Text = "";
            txtAddress.Text = "";
            guna2DateTimePicker1.Value = DateTime.Now;
            guna2ComboBox1.SelectedItem = null;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIdentity.Text) ||
                    string.IsNullOrWhiteSpace(txtNama.Text) ||
                    string.IsNullOrWhiteSpace(txtTelephone.Text) ||
                    string.IsNullOrWhiteSpace(txtAddress.Text) ||
                    guna2DateTimePicker1.Value == null ||
                    guna2ComboBox1.SelectedItem == null)
            {
                MessageBox.Show(this, "Please fill the required field!!", "Caution",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (string.IsNullOrWhiteSpace(txtIdentity.Text)) txtIdentity.Focus();
                else if (string.IsNullOrWhiteSpace(txtNama.Text)) txtNama.Focus();
                else if (string.IsNullOrWhiteSpace(txtTelephone.Text)) txtTelephone.Focus();
                else if (string.IsNullOrWhiteSpace(txtAddress.Text)) txtAddress.Focus();
                else if (guna2DateTimePicker1.Value == null) guna2DateTimePicker1.Focus();
                else if (guna2ComboBox1.SelectedItem == null) guna2ComboBox1.Focus();
                return;
            }
                // Get the selected date of birth from the DateTimePicker control
                DateTime dob = guna2DateTimePicker1.Value;

                // Calculate the age based on the selected date of birth
                int age = DateTime.Today.Year - dob.Year;

                // Check if the selected date of birth is in the future
                if (dob > DateTime.Today)
                {
                    MessageBox.Show(this, "Invalid date of birth! Please select a date in the past.", "Caution",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }

                // Check if the age is less than 18 years old
                if (age < 18)
                {
                    MessageBox.Show(this, "Invalid date of birth! You must be at least 18 years old to register.", "Caution",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }

                else
                {
                    string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                    MySqlConnection con = new MySqlConnection(connectionString);
                    con.Open();

                    string admincode = null;
                    string usernameglobal = Login.GlobalParams.namaadminglobal;
                    MySqlCommand cmd = new MySqlCommand("SELECT adminCode FROM admin WHERE adminName=@adminName", con);
                    cmd.Parameters.AddWithValue("@adminName", usernameglobal);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        admincode = reader.GetString(0);
                    }
                    reader.Close();

                    MySqlCommand command = new MySqlCommand("INSERT INTO `guest`(`guestidentityNum`, `guestName`, `guestDOB`, `guestGender`, `guestAddress`, `guestTelephone`, `adminCode`) VALUES (@guestIdentityNum, @guestName, @guestDOB, @guestGender, @guestAddress, @guestTelephone, @adminCode)", con);
                    command.Parameters.AddWithValue("@guestIdentityNum", txtIdentity.Text);
                    command.Parameters.AddWithValue("@guestName", txtNama.Text);
                    command.Parameters.AddWithValue("@guestDOB", guna2DateTimePicker1.Value.Date);
                    command.Parameters.AddWithValue("@guestGender", guna2ComboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@guestAddress", txtAddress.Text);
                    command.Parameters.AddWithValue("@guestTelephone", txtTelephone.Text);
                    command.Parameters.AddWithValue("@adminCode", admincode);

                    command.ExecuteNonQuery();
                    ResetFields();
                    // update guest list
                    Connection connection = new Connection();
                    connection.OpenConnection();
                    guna2DataGridView1.DataSource = connection.ShowData("SELECT * FROM guest");

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtIdentity.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString();
            txtNama.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[2].Value.ToString();
            guna2DateTimePicker1.Value = Convert.ToDateTime(guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[3].Value);
            txtTelephone.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[6].Value.ToString();
            guna2ComboBox1.SelectedItem = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[4].Value.ToString();
            txtAddress.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[5].Value.ToString();
            txtguestCode.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString();

        }



        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void txtIdentity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // membatalkan input karakter yang bukan angka
            }

            if (txtIdentity.Text.Length >= 16 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // membatalkan input karakter jika panjang teks sudah mencapai 16 karakter
            }
        }

        private void txtTelephone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // membatalkan input karakter yang bukan angka
            }

            if (txtTelephone.Text.Length >= 13 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // membatalkan input karakter jika panjang teks sudah mencapai 13 karakter
            }
        }

       

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
           
                string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                string query = "SELECT * FROM guest WHERE guestName LIKE @search";

                MySqlCommand command = new MySqlCommand(query, con);
                command.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                guna2DataGridView1.DataSource = table;

                con.Close();
            

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentity.Text) ||
               string.IsNullOrWhiteSpace(txtNama.Text) ||
               string.IsNullOrWhiteSpace(txtTelephone.Text) ||
               string.IsNullOrWhiteSpace(txtAddress.Text) ||
               guna2DateTimePicker1.Value == null ||
               guna2ComboBox1.SelectedItem == null)
            {
                MessageBox.Show(this, "Please fill the required field!!", "Caution",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                // Get the selected date of birth from the DateTimePicker control
                DateTime dob = guna2DateTimePicker1.Value;

                // Calculate the age based on the selected date of birth
                int age = DateTime.Today.Year - dob.Year;

                // Check if the selected date of birth is in the future
                if (dob > DateTime.Today)
                {
                    MessageBox.Show(this, "Invalid date of birth! Please select a date in the past.", "Caution",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }

                // Check if the age is less than 18 years old
                if (age < 18)
                {
                    MessageBox.Show(this, "Invalid date of birth! You must be at least 18 years old to register.", "Caution",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }

                // Get the current admin code from the logged-in user
                string adminCode = null;
                string usernameglobal = Login.GlobalParams.namaadminglobal;
                MySqlCommand cmd = new MySqlCommand("SELECT adminCode FROM admin WHERE adminName=@adminName", con);
                cmd.Parameters.AddWithValue("@adminName", usernameglobal);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    adminCode = reader.GetString(0);
                }
                reader.Close();

                // Update the guest record in the database
                MySqlCommand command = new MySqlCommand("UPDATE guest SET guestIdentityNum=@guestIdentityNum, guestName=@guestName, guestDOB=@guestDOB, guestGender=@guestGender, guestAddress=@guestAddress, guestTelephone=@guestTelephone, adminCode=@adminCode WHERE guestCode=@guestCode", con);
                command.Parameters.AddWithValue("@guestIdentityNum", txtIdentity.Text);
                command.Parameters.AddWithValue("@guestName", txtNama.Text);
                command.Parameters.AddWithValue("@guestDOB", guna2DateTimePicker1.Value.Date);
                command.Parameters.AddWithValue("@guestGender", guna2ComboBox1.SelectedItem.ToString());
                command.Parameters.AddWithValue("@guestAddress", txtAddress.Text);
                command.Parameters.AddWithValue("@guestTelephone", txtTelephone.Text);
                command.Parameters.AddWithValue("@adminCode", adminCode);
                command.Parameters.AddWithValue("@guestCode", txtguestCode.Text);

                command.ExecuteNonQuery();
                ResetFields();

                // Update the guest list
                Connection connection = new Connection();
                connection.OpenConnection();
                guna2DataGridView1.DataSource = connection.ShowData("SELECT * FROM guest");

                MessageBox.Show(this, "Guest record has been updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
