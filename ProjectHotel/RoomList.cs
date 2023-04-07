using Guna.UI2.WinForms.Suite;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectHotel
{
    public partial class RoomList : Form
    {
        public RoomList()
        {
            InitializeComponent();
            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData("SELECT roomCode AS Code, roomhotelFloor AS Floor, roomStatus AS Status, roomtype AS Type, roomtype.roomDescription AS Description, roomtype.maxGuest AS Max, roomtype.roomPrice_Night AS Price FROM room JOIN roomtype ON room.roomtypeCode = roomtype.roomtypeCode; ");

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns["Code"].HeaderText = "Room Code";
            guna2DataGridView1.Columns["Floor"].HeaderText = "Hotel Floor";
            guna2DataGridView1.Columns["Status"].HeaderText = "Room Status";
            guna2DataGridView1.Columns["Type"].HeaderText = "Room Type";
            guna2DataGridView1.Columns["Description"].HeaderText = "Room Description";
            guna2DataGridView1.Columns["Max"].HeaderText = "Max Guest";
            guna2DataGridView1.Columns["Price"].HeaderText = "Room Price/Night";
        }

        private void ListHotel_Load(object sender, EventArgs e)
        {
            this.label2.Parent = this.guna2PictureBox1;
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();

        }
    }
}
