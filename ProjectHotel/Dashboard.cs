using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Image = System.Drawing.Image;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;


namespace ProjectHotel
{
    public partial class Dashboard : Form
    {

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string usernameglobal = Login.GlobalParams.namaadminglobal;
            label1.Text = usernameglobal;

            MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;database=db_hotel;uid=root;password=;");
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT adminImage FROM admin WHERE adminName=@adminName", conn);
            cmd.Parameters.AddWithValue("@adminName", usernameglobal);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                byte[] imgData = (byte[])reader["adminImage"];
                MemoryStream ms = new MemoryStream(imgData);
                Image image = Image.FromStream(ms);
                fotoadmin.Image = image;
            }


            this.label2.Parent = this.guna2PictureBox1;
            timer1.Start();
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToLongDateString();
            label4.Text = DateTime.Now.ToLongTimeString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientTileButton5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are You Sure?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Login form1 = new Login();
                form1.Show(); // tampilkan Form1
                
                this.Dispose();
            }
        }

        private void guna2GradientTileButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            InputGuest ip= new InputGuest();
            ip.Show();
            
        }

        private void guna2GradientTileButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            RoomList lh = new RoomList();
            lh.Show();
        }
    }
}
