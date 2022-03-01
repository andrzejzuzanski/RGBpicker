using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace RGBpicker
{
    public partial class RGBpickerForm : Form
    {
        public RGBpickerForm()
        {
            InitializeComponent();
        }

        public void pickRGB_Click(object sender, EventArgs e)
        { 

            this.Hide();

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                         Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            using (MemoryStream s = new MemoryStream())
            {
                printscreen.Save(s, ImageFormat.Bmp);
                pictureBox1.Size = new System.Drawing.Size(this.Width, this.Height);

                pictureBox1.Image = Image.FromStream(s);
                s.Dispose();
            }
            this.Size = new Size(1200, 800);
            this.Show();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            PropertyInfo imageRectangleProperty = typeof(PictureBox).GetProperty("ImageRectangle", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);

            if (pictureBox1.Image != null)
            {
                MouseEventArgs me = (MouseEventArgs)e;

                Bitmap original = (Bitmap)pictureBox1.Image;

                Color? color = null;

                color = original.GetPixel(me.X, me.Y);
                string colorString = color.ToString();
                string colorString1 = colorString.Replace("Color [A=255, ", "");
                string colorString2 = colorString1.Replace("]", "");
                string[] colorString3 = colorString2.Split(",");

                rValue.Text = colorString3[0].Replace("R=", "");
                gValue.Text = colorString3[1].Replace(" G=", "");
                bValue.Text = colorString3[2].Replace(" B=", "");

                int r = Convert.ToInt32(colorString3[0].Replace("R=", ""));
                int g = Convert.ToInt32(colorString3[1].Replace(" G=", ""));
                int b = Convert.ToInt32(colorString3[2].Replace(" B=", ""));

                pictureBox2.BackColor = Color.FromArgb(r, g, b);

                hexValue.Text = ColorTranslator.ToHtml(Color.FromArgb(r, g, b));
            }
        }

        private bool Dragging;
        private int xPos;
        private int yPos;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (Dragging && c != null)
            {
                c.Top = e.Y + c.Top - yPos;
                c.Left = e.X + c.Left - xPos;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                xPos = e.X;
                yPos = e.Y;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) { Dragging = false; }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(hexValue.Text);
        }
    }
}