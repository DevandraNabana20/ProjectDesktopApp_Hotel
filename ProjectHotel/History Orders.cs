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
    public partial class History_Orders : Form
    {
        public History_Orders()
        {
            InitializeComponent();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard db = new Dashboard();
            db.Show();
        }

        private void History_Orders_Load(object sender, EventArgs e)
        {
            this.label2.Parent = this.guna2PictureBox1;
            

            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData("SELECT `historyCode`,`ordersCode`,guest.guestName,historyorders.roomCode,roomtype.roomtype,`checkinDate`,`checkoutDate`,`subPrice`,`extraGuest`,`charge`,`totalPrice`,`pay`,`moneyChanges`,historyorders.adminCode,`created_at` FROM historyorders JOIN guest on guest.guestCode=historyorders.guestCode JOIN room on room.roomCode=historyorders.roomCode JOIN roomtype on roomtype.roomtypeCode=room.roomtypeCode");

            

            // Mengatur lebar kolom terakhir agar seluruh kolom sebelumnya terlihat tanpa menyisakan area kosong pada DataGridView
            guna2DataGridView1.Columns[guna2DataGridView1.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns[0].HeaderText = "History Code";
            guna2DataGridView1.Columns[1].HeaderText = "Order Code";
            guna2DataGridView1.Columns[2].HeaderText = "Guest Name";
            guna2DataGridView1.Columns[3].HeaderText = "Room Code";
            guna2DataGridView1.Columns[4].HeaderText = "Room Type";
            guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
            guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
            guna2DataGridView1.Columns[7].HeaderText = "Sub Price";
            guna2DataGridView1.Columns[8].HeaderText = "Extra Guest";
            guna2DataGridView1.Columns[9].HeaderText = "Charge";
            guna2DataGridView1.Columns[10].HeaderText = "Total Price";
            guna2DataGridView1.Columns[11].HeaderText = "Pay";
            guna2DataGridView1.Columns[12].HeaderText = "Money Changes";
            guna2DataGridView1.Columns[13].HeaderText = "Admin Code";
            guna2DataGridView1.Columns[14].HeaderText = "Created At";
        }
    }
}
