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
    public partial class SelectRoom : Form
    {
        public SelectRoom()
        {
            InitializeComponent();
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtroomCode.Text))
            {
                MessageBox.Show("Please select at least one row in the table before continuing.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Reservation reservation = new Reservation();
                
                string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlCommand command = new MySqlCommand("SELECT roomtype.roomtype FROM roomtype JOIN room on room.roomtypeCode=roomtype.roomtypeCode WHERE room.roomCode = @roomCode",conn);
                command.Parameters.AddWithValue("@roomcode",txtroomCode.Text);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string roomtype = reader.GetString(0);
                    reservation.txtselectroom.Text = txtroomCode.Text + " - " + roomtype;
                }

                reservation.txtroomPrice.Text = txtroomprice.Text;
                reservation.txtroomCode.Text = txtroomcodeOLD.Text;
               
                this.Hide();
                reservation.Show();
            }
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Reservation reservation = new Reservation();
            reservation.Show();
        }

        private void ListHotelSelect_Load(object sender, EventArgs e)
        {
            txtroomCode.Text = null;
            this.label2.Parent = this.guna2PictureBox1;
            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData("SELECT roomCode AS Code, roomhotelFloor AS Floor, roomStatus AS Status, roomtype AS Type, roomtype.roomDescription AS Description, roomtype.maxGuest AS Max, roomtype.roomPrice_Night AS Price FROM room JOIN roomtype ON room.roomtypeCode = roomtype.roomtypeCode where roomStatus = 'Available'; ");

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns["Code"].HeaderText = "Room Code";
            guna2DataGridView1.Columns["Floor"].HeaderText = "Hotel Floor";
            guna2DataGridView1.Columns["Status"].HeaderText = "Room Status";
            guna2DataGridView1.Columns["Type"].HeaderText = "Room Type";
            guna2DataGridView1.Columns["Description"].HeaderText = "Room Description";
            guna2DataGridView1.Columns["Max"].HeaderText = "Max Guest";
            guna2DataGridView1.Columns["Price"].HeaderText = "Room Price/Night";
        }

        private void guna2DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtroomCode.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString();
            txtroomprice.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[6].Value.ToString();
        }
    }
}
