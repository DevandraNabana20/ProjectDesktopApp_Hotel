using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectHotel
{
    public partial class Details_CheckOut : Form
    {
        public Details_CheckOut()
        {
            InitializeComponent();
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            
        }

        private void Details_CheckOut_Load(object sender, EventArgs e)
        {
            this.label2.Parent = this.guna2PictureBox1;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Buat objek dari kelas Document dari library iTextSharp
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            try
            {
                // Tentukan lokasi dan nama file PDF yang akan dibuat
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF (*.pdf)|*.pdf";
                saveFileDialog1.Title = "Save PDF File";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(saveFileDialog1.FileName, FileMode.Create));

                    //size a5
                    doc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, 421f, 595f));

                    // Buka dokumen PDF
                    doc.Open();

                    // Tambahkan judul ke dalam dokumen PDF
                    Paragraph title = new Paragraph("Details Order", FontFactory.GetFont(FontFactory.TIMES_BOLD, 18));
                    title.Alignment = Element.ALIGN_CENTER; // Atur posisi judul ke tengah halaman
                    doc.Add(title);
                    


                    // Buat objek dari kelas Paragraph dari library iTextSharp
                    iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph();

                    // Tambahkan nilai dari TextArea ke dalam Paragraph dan terapkan style yang diinginkan
                    para.Alignment = Element.ALIGN_CENTER;

                    // Tambahkan nilai dari TextBox ke dalam Paragraph
                    para.Add(Textareadetails.Text + "\n");

                    // Tambahkan Paragraph ke dalam dokumen PDF
                    doc.Add(para);

                    // Tutup dokumen PDF
                    doc.Close();

                    MessageBox.Show("PDF file has been created.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
