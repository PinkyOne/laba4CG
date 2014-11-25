using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labCG4
{
    using System.Drawing.Imaging;
    using System.Globalization;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Load("2.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }


        private void Button1Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Contraster.ConstrastImage(
                pictureBox1.Image,
                Int32.Parse(textBox1.Text),
                Int32.Parse(textBox2.Text));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
