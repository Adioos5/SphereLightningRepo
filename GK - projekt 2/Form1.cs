using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Numerics;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using GK___projekt_2.Containers;

namespace GK___projekt_2
{
    public partial class Form1 : Form
    {

        private Context context;

        public Form1()
        {
            InitializeComponent();

            context = new Context(Canvas, ByPointRadioButton, ByColorRadioButton, LoadedCheckBox, ShowCheckBox);

            Logic.InitializeEngine(context);
            Painter.PrepareCanvas(context);
            Painter.FillSphere(context);
        }

        private void ChooseColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                context.lightColor = (
                                       Logic.Map((double)color.R, 0.0, 255.0, 0.0, 1.0),
                                       Logic.Map((double)color.G, 0.0, 255.0, 0.0, 1.0),
                                       Logic.Map((double)color.B, 0.0, 255.0, 0.0, 1.0)
                                       );

                Painter.FillSphere(context);
                Canvas.Refresh();
            }
        }

        private void ChooseImageButton_Click(object sender, EventArgs e)
        {
            
            if (ChooseImageButton.Text == "Remove image")
            {
                ChooseImageButton.Text = "Choose image";
                ChooseImageButton.Refresh();
                context.drawFromImage = false;
                context.objectColor = (1, 1, 1);

                Painter.FillSphere(context);
                Canvas.Refresh();


                return;
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PictureBox PictureBox1 = new PictureBox();
                    PictureBox1.Image = Logic.ResizeImage(new Bitmap(dlg.FileName), new Size(Canvas.Size.Width, Canvas.Size.Height));

                    context.imageBitmap = ((Bitmap)PictureBox1.Image);
                    context.drawFromImage = true;

                    Painter.FillSphere(context);
                    Canvas.Refresh();
                }
            }

            if (ChooseImageButton.Text == "Choose image")
            {
                ChooseImageButton.Text = "Remove image";
                ChooseImageButton.Refresh();
            }
        }

        private void LoadedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadedCheckBox.Checked)
            {
                PictureBox PictureBox1 = new PictureBox();
                PictureBox1.Image = Logic.ResizeImage(new Bitmap("..\\..\\..\\Images\\ball.jpg"), new Size(Canvas.Size.Width, Canvas.Size.Height));
                
                context.imageBitmap = ((Bitmap)PictureBox1.Image);
                context.objectColor = (Logic.Map((double)Color.White.R, 0.0, 255.0, 0.0, 1.0),
                                       Logic.Map((double)Color.White.G, 0.0, 255.0, 0.0, 1.0),
                                       Logic.Map((double)Color.White.B, 0.0, 255.0, 0.0, 1.0)
                                       );

                ChooseImageButton.Text = "Choose image";

                ByPointRadioButton.Enabled = false;
                ByColorRadioButton.Enabled = false;
                ChooseImageButton.Enabled = false;
                context.drawFromImage = false;
            } else
            {
                ByPointRadioButton.Enabled = true;
                ByColorRadioButton.Enabled = true;
                ChooseImageButton.Enabled = true;
                context.drawFromImage = false;
            }

            Painter.FillSphere(context);
            Canvas.Refresh();
        }

        private void ByColorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Painter.FillSphere(context);
            Canvas.Refresh();
        }

        private void ByPointRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Painter.FillSphere(context);
            Canvas.Refresh();
        }

        private void ShowCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Painter.FillSphere(context);
            Canvas.Refresh();
        }


        private void StartAnimationButton_Click(object sender, EventArgs e)
        {
            StartAnimationButton.Text = "Animation in progress...";
            StartAnimationButton.Refresh();
            
            double angle = 0.8;

            do {
                context.lightRoute.CalculateNewCoordinates(angle);
                Painter.FillSphere(context);
                Canvas.Refresh();
                Thread.Sleep(1);

                angle += 0.8;
            } while (angle < 6.28);

            context.lightRoute.CalculateNewCoordinates(0);
            Painter.FillSphere(context);
            Canvas.Refresh();

            StartAnimationButton.Text = "Start animation";
            StartAnimationButton.Refresh();
        }

        private void KDSlider_Scroll(object sender, EventArgs e)
        {
            context.kd = (double) KDSlider.Value / 10.0;

            Painter.FillSphere(context);
            Canvas.Refresh();
        }

        private void KSSlider_Scroll(object sender, EventArgs e)
        {
            context.ks = (double) KSSlider.Value / 10.0;

            Painter.FillSphere(context);
            Canvas.Refresh();
        }

        private void MSlider_Scroll(object sender, EventArgs e)
        {
            context.mirroring = MSlider.Value;

            Painter.FillSphere(context);
            Canvas.Refresh();
        }

        private void KASlider_Scroll(object sender, EventArgs e)
        {
            context.ka = (double)KASlider.Value / 10.0;

            Painter.FillSphere(context);
            Canvas.Refresh();
        }
    }
}