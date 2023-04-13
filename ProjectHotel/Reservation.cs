using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectHotel
{
    public partial class Reservation : Form
    {
        public Reservation()
        {
            InitializeComponent();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;

        }

        

        private void Reservation_Load(object sender, EventArgs e)
        {

            guna2DateTimePicker1.MinDate = DateTime.Today;

            this.label1.Parent = this.guna2PictureBox1;
            this.label2.Parent = this.guna2PictureBox1;
            this.label3.Parent = this.guna2PictureBox1;
            this.label4.Parent = this.guna2PictureBox1;
            this.label5.Parent = this.guna2PictureBox1;
            this.label6.Parent = this.guna2PictureBox1;
            this.label7.Parent = this.guna2PictureBox1;
            this.label8.Parent = this.guna2PictureBox1;
            this.label9.Parent = this.guna2PictureBox1;
            this.label14.Parent = this.guna2PictureBox1;

            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM orders order by ordersCode DESC LIMIT 1",con);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
             txtorderCode.Text = reader.GetString(0);
            }
            reader.Close();


            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData("SELECT ordersCode,orderdetail.roomCode,roomtype,room.roomhotelFloor,roomtype.maxGuest,checkinDate,checkoutDate,subPrice FROM orderdetail JOIN room ON orderdetail.roomCode=room.roomCode JOIN roomtype ON room.roomtypeCode=roomtype.roomtypeCode WHERE ordersCode='" + txtorderCode.Text + "'");

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns[0].HeaderText = "Order Code";
            guna2DataGridView1.Columns[1].HeaderText = "Room Code";
            guna2DataGridView1.Columns[2].HeaderText = "Room Type";
            guna2DataGridView1.Columns[3].HeaderText = "Hotel Floor";
            guna2DataGridView1.Columns[4].HeaderText = "Max Guest";
            guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
            guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
            guna2DataGridView1.Columns[7].HeaderText = "Sub Price";

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand command1 = new MySqlCommand("SELECT * FROM orderdetail where ordersCode =@ordersCode", con);
            command1.Parameters.AddWithValue("@ordersCode", txtorderCode.Text);
            MySqlDataReader reader = command1.ExecuteReader();
            if (reader.HasRows)
            {
                MessageBox.Show(this, "Please delete all the records first!!", "Caution",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            reader.Close();

            DialogResult dialogResult = MessageBox.Show("Do you want to cancel this reservation?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    MySqlCommand command = new MySqlCommand("Delete from orders where ordersCode =@ordersCode", con);
                    command.Parameters.AddWithValue("@ordersCode", txtorderCode.Text);
                    command.ExecuteNonQuery();

                    this.Hide();
                    InputGuest inputGuest = new InputGuest();
                    inputGuest.Show();
                }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            SelectRoom selectRoom = new SelectRoom();
            selectRoom.txtroomcodeOLD.Text = txtroomCode.Text;
            selectRoom.Show();
        }

        private void ResetFields()
        {
            txtselectroom.Text = "";
            guna2DateTimePicker1.Value = DateTime.Now;
            guna2DateTimePicker2.Value = DateTime.Now.AddDays(1);
            txtroomCode.Text = "roomCode";
            txtroomPrice.Text = "roomPrice";
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnCreate.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
           ResetFields();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime checkinDate = guna2DateTimePicker1.Value;
                DateTime checkoutDate = guna2DateTimePicker2.Value;
                TimeSpan totalDays = checkoutDate.Date - checkinDate.Date;
                int numberOfDays = totalDays.Days;

                if (string.IsNullOrWhiteSpace(txtorderCode.Text) ||
                    string.IsNullOrWhiteSpace(txtselectroom.Text) ||
                    guna2DateTimePicker1.Value == null ||
                    guna2DateTimePicker2.Value == null)

                {
                    MessageBox.Show(this, "Please fill the required field!!", "Caution",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (string.IsNullOrWhiteSpace(txtorderCode.Text)) txtorderCode.Focus();
                    else if (string.IsNullOrWhiteSpace(txtselectroom.Text)) txtselectroom.Focus();
                    else if (guna2DateTimePicker1.Value == null) guna2DateTimePicker1.Focus();
                    else if (guna2DateTimePicker2.Value == null) guna2DateTimePicker2.Focus();
                    return;
                }

                else
                {
                    string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                    MySqlConnection con = new MySqlConnection(connectionString);
                    con.Open();


                    MySqlCommand command = new MySqlCommand("INSERT INTO `orderdetail`(`ordersCode`, `roomCode`, `checkinDate`, `checkoutDate`, `subPrice`) VALUES (@ordersCode,@roomCode,@checkinDate,@checkoutDate,@subPrice)", con);
                    command.Parameters.AddWithValue("@ordersCode", txtorderCode.Text);
                    command.Parameters.AddWithValue("@roomCode", txtselectroom.Text.Substring(0,7));
                    command.Parameters.AddWithValue("@checkinDate", guna2DateTimePicker1.Value.Date);
                    command.Parameters.AddWithValue("@checkoutDate", guna2DateTimePicker2.Value.Date);
                    command.Parameters.AddWithValue("@subPrice", Convert.ToInt32(txtroomPrice.Text)*numberOfDays);

                    command.ExecuteNonQuery();
                    MessageBox.Show(this, "Order record has been created successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    ResetFields();

                    Connection connection = new Connection();
                    connection.OpenConnection();
                    guna2DataGridView1.DataSource = connection.ShowData("SELECT ordersCode,orderdetail.roomCode,roomtype,room.roomhotelFloor,roomtype.maxGuest,checkinDate,checkoutDate,subPrice FROM orderdetail JOIN room ON orderdetail.roomCode=room.roomCode JOIN roomtype ON room.roomtypeCode=roomtype.roomtypeCode WHERE ordersCode='" + txtorderCode.Text + "'");
                    // Mengubah nama kolom pada DataGridView
                    guna2DataGridView1.Columns[0].HeaderText = "Order Code";
                    guna2DataGridView1.Columns[1].HeaderText = "Room Code";
                    guna2DataGridView1.Columns[2].HeaderText = "Room Type";
                    guna2DataGridView1.Columns[3].HeaderText = "Hotel Floor";
                    guna2DataGridView1.Columns[4].HeaderText = "Max Guest";
                    guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
                    guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
                    guna2DataGridView1.Columns[7].HeaderText = "Sub Price";

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (guna2DateTimePicker1.Value < DateTime.Today)
            {
                MessageBox.Show(this, "Invalid date, please select today's date or after today!!", "Caution",MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
            {
                guna2DateTimePicker2.Enabled = true;
                // Ambil tanggal yang dipilih di DateTimePicker1
                DateTime selectedDate = guna2DateTimePicker1.Value;

                // Set nilai minimal DateTimePicker2 menjadi 1 hari setelah tanggal yang dipilih di DateTimePicker1
                guna2DateTimePicker2.MinDate = selectedDate.AddDays(1);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtroomCode.Text == "roomCode"){
                MessageBox.Show("Please select at least one row in the table before continuing.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            DialogResult dialogResult = MessageBox.Show("Do you want to delete this record, room code : "+txtroomCode.Text+" ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                MySqlCommand command = new MySqlCommand("Delete from orderdetail where roomCode=@roomCode", con);
                command.Parameters.AddWithValue("@roomCode", txtroomCode.Text);

                command.ExecuteNonQuery();
                MessageBox.Show(this, "Delete record Successfull!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                txtroomCode.Text = "roomCode";
                ResetFields();

                Connection connection = new Connection();
                connection.OpenConnection();
                guna2DataGridView1.DataSource = connection.ShowData("SELECT ordersCode,orderdetail.roomCode,roomtype,room.roomhotelFloor,roomtype.maxGuest,checkinDate,checkoutDate,subPrice FROM orderdetail JOIN room ON orderdetail.roomCode=room.roomCode JOIN roomtype ON room.roomtypeCode=roomtype.roomtypeCode WHERE ordersCode='" + txtorderCode.Text + "'");

                // Mengubah nama kolom pada DataGridView
                guna2DataGridView1.Columns[0].HeaderText = "Order Code";
                guna2DataGridView1.Columns[1].HeaderText = "Room Code";
                guna2DataGridView1.Columns[2].HeaderText = "Room Type";
                guna2DataGridView1.Columns[3].HeaderText = "Hotel Floor";
                guna2DataGridView1.Columns[4].HeaderText = "Max Guest";
                guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
                guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
                guna2DataGridView1.Columns[7].HeaderText = "Sub Price";

                con.Close();
            }
        }

        private void guna2DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnCreate.Enabled = false;
            txtroomCode.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString();
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime checkinDate = guna2DateTimePicker1.Value;
                DateTime checkoutDate = guna2DateTimePicker2.Value;
                TimeSpan totalDays = checkoutDate.Date - checkinDate.Date;
                int numberOfDays = totalDays.Days;

                if (string.IsNullOrWhiteSpace(txtorderCode.Text) ||
                    guna2DateTimePicker1.Value == null ||
                    guna2DateTimePicker2.Value == null||
                    txtroomCode.Text=="roomCode")

                {
                    MessageBox.Show("Please select at least one row in the table before continuing.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (string.IsNullOrWhiteSpace(txtorderCode.Text)) txtorderCode.Focus();
                    else if (guna2DateTimePicker1.Value == null) guna2DateTimePicker1.Focus();
                    else if (guna2DateTimePicker2.Value == null) guna2DateTimePicker2.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtselectroom.Text))
                {
                    MessageBox.Show("Please select the room first!", "Caution",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtselectroom.Focus();
                }
                else
                {
                    string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                    MySqlConnection con = new MySqlConnection(connectionString);
                    con.Open();


                    MySqlCommand command = new MySqlCommand("UPDATE `orderdetail` SET `roomCode` = @roomCode, `checkinDate` = @checkinDate, `checkoutDate` = @checkoutDate, `subPrice` = @subPrice WHERE `roomCode` = @roomcodeOLD", con);
                    command.Parameters.AddWithValue("@roomcodeOLD", txtroomCode.Text);
                    command.Parameters.AddWithValue("@roomCode", txtselectroom.Text.Substring(0, 7));
                    command.Parameters.AddWithValue("@checkinDate", guna2DateTimePicker1.Value.Date);
                    command.Parameters.AddWithValue("@checkoutDate", guna2DateTimePicker2.Value.Date);
                    command.Parameters.AddWithValue("@subPrice", Convert.ToInt32(txtroomPrice.Text) * numberOfDays);


                    command.ExecuteNonQuery();
                    MessageBox.Show(this, "Order record has been updated successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    
                    ResetFields();

                    Connection connection = new Connection();
                    connection.OpenConnection();
                    guna2DataGridView1.DataSource = connection.ShowData("SELECT ordersCode,orderdetail.roomCode,roomtype,room.roomhotelFloor,roomtype.maxGuest,checkinDate,checkoutDate,subPrice FROM orderdetail JOIN room ON orderdetail.roomCode=room.roomCode JOIN roomtype ON room.roomtypeCode=roomtype.roomtypeCode WHERE ordersCode='" + txtorderCode.Text + "'");

                    // Mengubah nama kolom pada DataGridView
                    guna2DataGridView1.Columns[0].HeaderText = "Order Code";
                    guna2DataGridView1.Columns[1].HeaderText = "Room Code";
                    guna2DataGridView1.Columns[2].HeaderText = "Room Type";
                    guna2DataGridView1.Columns[3].HeaderText = "Hotel Floor";
                    guna2DataGridView1.Columns[4].HeaderText = "Max Guest";
                    guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
                    guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
                    guna2DataGridView1.Columns[7].HeaderText = "Sub Price";

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string query = "SELECT ordersCode,orderdetail.roomCode,roomtype,room.roomhotelFloor,roomtype.maxGuest,checkinDate,checkoutDate,subPrice FROM orderdetail JOIN room ON orderdetail.roomCode=room.roomCode JOIN roomtype ON room.roomtypeCode=roomtype.roomtypeCode WHERE room.roomCode LIKE @search AND orderdetail.ordersCode LIKE @orderscode";

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns[0].HeaderText = "Order Code";
            guna2DataGridView1.Columns[1].HeaderText = "Room Code";
            guna2DataGridView1.Columns[2].HeaderText = "Room Type";
            guna2DataGridView1.Columns[3].HeaderText = "Hotel Floor";
            guna2DataGridView1.Columns[4].HeaderText = "Max Guest";
            guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
            guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
            guna2DataGridView1.Columns[7].HeaderText = "Sub Price";

            MySqlCommand command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
            command.Parameters.AddWithValue("@orderscode", "%" + txtorderCode.Text + "%");

            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            guna2DataGridView1.DataSource = table;

            con.Close();
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM orderdetail where ordersCode='" + txtorderCode.Text + "'", con);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                Payment payment = new Payment();
                payment.lblOrdercode.Text = txtorderCode.Text;
                this.Hide();
                payment.Show();
            }
            else
            {
                MessageBox.Show("Please do some orders in the table before continuing.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
