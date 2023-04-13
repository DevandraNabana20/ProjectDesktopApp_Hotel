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
    public partial class Payment : Form
    {
        private string totalprice;

        public Payment()
        {
            InitializeComponent();

            
            btnNext.Visible = false;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Reservation reservation = new Reservation();
            reservation.txtorderCode.Text = lblOrdercode.Text;
            reservation.Show();
        }

        private void UpdateCharge()
        {
            int harga1 = 50000;
            int harga2 = 100000;
            int harga3 = 150000;

            switch (cbExtra.SelectedIndex)
            {
                case 1:
                    txtCharge.Text = harga1.ToString();
                    txttotalPrice.Text =
                        (int.Parse(totalprice) + int.Parse(txtCharge.Text)).ToString();
                    break;
                case 2:
                    txtCharge.Text = harga2.ToString();
                    txttotalPrice.Text =
                         (int.Parse(totalprice) + int.Parse(txtCharge.Text)).ToString();
                    break;
                case 3:
                    txtCharge.Text = harga3.ToString();
                    txttotalPrice.Text =
                        (int.Parse(totalprice) + int.Parse(txtCharge.Text)).ToString();
                    break;
                default:
                    txtCharge.Text = "0";
                    txttotalPrice.Text =
                       (int.Parse(totalprice) + int.Parse(txtCharge.Text)).ToString();
                    break;

            }

        }


        private void Payment_Load(object sender, EventArgs e)
        {

            this.label1.Parent = this.guna2PictureBox1;
            this.label2.Parent = this.guna2PictureBox1;
            this.label3.Parent = this.guna2PictureBox1;
            this.label4.Parent = this.guna2PictureBox1;
            this.label5.Parent = this.guna2PictureBox1;
            this.label6.Parent = this.guna2PictureBox1;
            this.label7.Parent = this.guna2PictureBox1;
            this.label8.Parent = this.guna2PictureBox1;
            this.label9.Parent = this.guna2PictureBox1;
            this.label10.Parent = this.guna2PictureBox1;
            this.label11.Parent = this.guna2PictureBox1;
            this.label12.Parent = this.guna2PictureBox1;
            this.label13.Parent = this.guna2PictureBox1;
            this.label14.Parent = this.guna2PictureBox1;
            this.label15.Parent = this.guna2PictureBox1;
            this.label16.Parent = this.guna2PictureBox1;



            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData(
                "SELECT ordersCode,orderdetail.roomCode,roomtype,room.roomhotelFloor,roomtype.maxGuest,checkinDate,checkoutDate,subPrice FROM orderdetail JOIN room ON orderdetail.roomCode=room.roomCode JOIN roomtype ON room.roomtypeCode=roomtype.roomtypeCode WHERE ordersCode='" +
                lblOrdercode.Text + "'");

            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns[0].HeaderText = "Order Code";
            guna2DataGridView1.Columns[1].HeaderText = "Room Code";
            guna2DataGridView1.Columns[2].HeaderText = "Room Type";
            guna2DataGridView1.Columns[3].HeaderText = "Hotel Floor";
            guna2DataGridView1.Columns[4].HeaderText = "Max Guest";
            guna2DataGridView1.Columns[5].HeaderText = "CheckIn Date";
            guna2DataGridView1.Columns[6].HeaderText = "CheckOut Date";
            guna2DataGridView1.Columns[7].HeaderText = "Sub Price";

            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT guest.guestName FROM orders JOIN guest on guest.guestCode=orders.guestCode WHERE ordersCode=@ordercode",
                    con);
            command.Parameters.AddWithValue("@ordercode", lblOrdercode.Text);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                txtguestName.Text = reader.GetString(0);
            }

            reader.Close();


            //get Total Price
            MySqlCommand command1 =
                new MySqlCommand("SELECT SUM(subPrice) FROM orderdetail WHERE ordersCode=@ordercode", con);
            command1.Parameters.AddWithValue("@ordercode", lblOrdercode.Text);
            MySqlDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                totalprice = reader1.GetString(0);
                txttotalPrice.Text = totalprice;
            }

            reader1.Close();

            UpdateCharge();

        }

        private void cbExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharge();
        }

        private void txtPay_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // membatalkan input karakter yang bukan angka
            }


        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPay.Text))
            {
                MessageBox.Show("Please fill the right amount!", "Caution",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (int.Parse(txtPay.Text) < int.Parse(txttotalPrice.Text))
            {
                MessageBox.Show("Payment is insufficient.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int moneyChanges = int.Parse(txtPay.Text) - int.Parse(txttotalPrice.Text);
                txtmoneyChanges.Text = moneyChanges.ToString();
                btnNext.Visible = true;
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Is the data you entered correct?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                MySqlCommand command =
                    new MySqlCommand(
                        "UPDATE orders SET extraGuest = @extraguest, charge = @charge, totalPrice = @totalprice, pay = @pay , moneyChanges = @moneychange WHERE ordersCode=@ordercode",
                        con);
                command.Parameters.AddWithValue("@extraguest", cbExtra.SelectedItem.ToString());
                command.Parameters.AddWithValue("@charge", txtCharge.Text);
                command.Parameters.AddWithValue("@totalprice", txttotalPrice.Text);
                command.Parameters.AddWithValue("@pay", txtPay.Text);
                command.Parameters.AddWithValue("@moneychange", txtmoneyChanges.Text);
                command.Parameters.AddWithValue("@ordercode", lblOrdercode.Text);


                MySqlCommand command1 =
                    new MySqlCommand(
                        "INSERT INTO historyorders (ordersCode, guestCode , roomCode, checkinDate, checkoutDate, subPrice, extraGuest, charge, totalPrice, pay, moneyChanges, adminCode) SELECT orders.ordersCode, orders.guestCode, orderdetail.roomCode, orderdetail.checkinDate, orderdetail.checkoutDate, orderdetail.subPrice, extraGuest, charge, totalPrice, pay, moneyChanges,orders.adminCode FROM orders JOIN orderdetail ON orderdetail.ordersCode=orders.ordersCode WHERE orders.ordersCode = @ordercode",
                        con);
                command1.Parameters.AddWithValue("@ordercode", lblOrdercode.Text);



                command.ExecuteNonQuery();
                command1.ExecuteNonQuery();

                MessageBox.Show(this, "Check In guest successfull.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                this.Hide();
                Dashboard db = new Dashboard();
                db.Show();
            }
            else
            {
                return;
            }
        }
    }
}
