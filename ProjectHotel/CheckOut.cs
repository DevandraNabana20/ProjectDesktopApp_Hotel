using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectHotel
{
    public partial class CheckOut : Form
    {
        public CheckOut()
        {
            InitializeComponent();
            btnDetails.Enabled = false;
            btncheckOut.Enabled = false;
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            Connection connection = new Connection();
            connection.OpenConnection();
            guna2DataGridView1.DataSource = connection.ShowData("SELECT * FROM orders");


            // Mengubah nama kolom pada DataGridView
            guna2DataGridView1.Columns[0].HeaderText = "Order Code";
            guna2DataGridView1.Columns[1].HeaderText = "Guest Code";
            guna2DataGridView1.Columns[2].HeaderText = "Extra Guest";
            guna2DataGridView1.Columns[3].HeaderText = "Charge";
            guna2DataGridView1.Columns[4].HeaderText = "Total Price";
            guna2DataGridView1.Columns[5].HeaderText = "Pay";
            guna2DataGridView1.Columns[6].HeaderText = "Money Changes";
            guna2DataGridView1.Columns[7].HeaderText = "Admin Code";

            this.label2.Parent = this.guna2PictureBox1;

        }

        private void guna2DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnDetails.Enabled = true;
            btncheckOut.Enabled = true;
            lblorderCode.Text = guna2DataGridView1.Rows[guna2DataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString();
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = new MySqlCommand(
                    "SELECT orders.ordersCode,guest.guestName,guest.guestAddress,guest.guestTelephone,room.roomCode,room.roomhotelFloor,roomtype.roomtype,orderdetail.checkinDate,orderdetail.checkoutDate,orderdetail.subPrice,extraGuest,charge,totalPrice,pay,moneyChanges FROM orders JOIN orderdetail on orderdetail.ordersCode=orders.ordersCode JOIN guest on guest.guestCode = orders.guestCode JOIN room on orderdetail.roomCode = room.roomCode JOIN roomtype on room.roomtypeCode=roomtype.roomtypeCode WHERE orders.ordersCode ='" +
                    lblorderCode.Text + "'", conn);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var ordersCode = reader.GetString("ordersCode");
                    var guestName = reader.GetString("guestName");
                    var guestAddress = reader.GetString("guestAddress");
                    var guestTelephone = reader.GetString("guestTelephone");
                    var extraGuest = reader.GetInt32("extraGuest");
                    var charge = reader.GetDecimal("charge").ToString("C");
                    var totalPrice = reader.GetDecimal("totalPrice").ToString("C");
                    var pay = reader.GetDecimal("pay").ToString("C");
                    var moneyChanges = reader.GetDecimal("moneyChanges").ToString("C");

                    reader.Close();
                    reader = command.ExecuteReader();

                    Details_CheckOut dco = new Details_CheckOut();
                    dco.Textareadetails.SelectionAlignment = HorizontalAlignment.Center;


                    // Output order details
                    dco.Textareadetails.AppendText("\n");
                    dco.Textareadetails.AppendText("Order Code: " + ordersCode + "\n");
                    dco.Textareadetails.AppendText("Guest Name: " + guestName + "\n");
                    dco.Textareadetails.AppendText("Guest Address: " + guestAddress + "\n");
                    dco.Textareadetails.AppendText("Guest Telephone: " + guestTelephone + "\n");
                    dco.Textareadetails.AppendText("----------------------------------------------\n");

                    while (reader.Read())
                    {
                        var roomCode = reader.GetString("roomCode");
                        var roomhotelFloor = reader.GetString("roomhotelFloor");
                        var roomtype = reader.GetString("roomtype");
                        var checkinDate = reader.GetDateTime("checkinDate").ToString("yyyy-MM-dd");
                        var checkoutDate = reader.GetDateTime("checkoutDate").ToString("yyyy-MM-dd");
                        var subPrice = reader.GetDecimal("subPrice").ToString("C");
                        
                        // Output room details
                        dco.Textareadetails.AppendText("Room Code: " + roomCode + "\n");
                        dco.Textareadetails.AppendText("Hotel Floor: " + roomhotelFloor + "\n");
                        dco.Textareadetails.AppendText("Room Type: " + roomtype + "\n");
                        dco.Textareadetails.AppendText("Check-in Date: " + checkinDate + "\n");
                        dco.Textareadetails.AppendText("Check-out Date: " + checkoutDate + "\n");
                        dco.Textareadetails.AppendText("Sub Price: " + subPrice + "\n");
                        dco.Textareadetails.AppendText("\n");
                    }
                    // Output additional details

                    dco.Textareadetails.AppendText("----------------------------------------------\n");
                    dco.Textareadetails.AppendText("Extra Guest: " + extraGuest.ToString() + "\n");
                    dco.Textareadetails.AppendText("Charge: " + charge + "\n");
                    dco.Textareadetails.AppendText("Total Price: " + totalPrice + "\n");
                    dco.Textareadetails.AppendText("Pay: " + pay + "\n");
                    dco.Textareadetails.AppendText("Money Changes: " + moneyChanges + "\n");

                    dco.Show();
                }

            }        
        }

        private void btncheckOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to checkout this data, order Code = "+lblorderCode.Text+" ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                MySqlCommand command = new MySqlCommand(
                    "DELETE FROM orderdetail WHERE ordersCode ='" + lblorderCode.Text + "'; " +
                    "DELETE FROM orders WHERE ordersCode ='" + lblorderCode.Text + "'", con);
                command.ExecuteNonQuery();

                MessageBox.Show(this, "Check Out guest successfull.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                btncheckOut.Enabled = false;
                btnDetails.Enabled = false;

                Connection connection = new Connection();
                connection.OpenConnection();
                guna2DataGridView1.DataSource = connection.ShowData("SELECT * FROM orders");


                // Mengubah nama kolom pada DataGridView
                guna2DataGridView1.Columns[0].HeaderText = "Order Code";
                guna2DataGridView1.Columns[1].HeaderText = "Guest Code";
                guna2DataGridView1.Columns[2].HeaderText = "Extra Guest";
                guna2DataGridView1.Columns[3].HeaderText = "Charge";
                guna2DataGridView1.Columns[4].HeaderText = "Total Price";
                guna2DataGridView1.Columns[5].HeaderText = "Pay";
                guna2DataGridView1.Columns[6].HeaderText = "Money Changes";
                guna2DataGridView1.Columns[7].HeaderText = "Admin Code";
                lblorderCode.Text = "";

                return;
            }
        }
    }
}
