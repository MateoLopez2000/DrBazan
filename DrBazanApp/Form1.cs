using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrBazanApp
{
    public partial class Form1 : Form
    {
        private const string Format = "yyyyMMdd_hhmmss";
        private string Path = @"C:\Users\Mlopez\Visual Studio\DrBazanApp\DrBazanApp\pictures\";
        private bool HayDispositivos;
        private FilterInfoCollection MisDispositivos;
        private VideoCaptureDevice MiWebCam;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargaDispositivos();
        }

        public void CargaDispositivos()
        {
            MisDispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if(MisDispositivos.Count > 0)
            {
                HayDispositivos = true;
                for(int i = 0; i < MisDispositivos.Count; i++)
                {
                    cbxCameras.Items.Add(MisDispositivos[i].Name.ToString());
                    cbxCameras.Text = MisDispositivos[0].Name.ToString();
                }
            }
            else
            {
                HayDispositivos = false;
            }
        }

        private void CerrarWebCam()
        {
            if (MiWebCam != null && MiWebCam.IsRunning)
            {
                MiWebCam.SignalToStop();
                MiWebCam = null;
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            CerrarWebCam();
            int i = cbxCameras.SelectedIndex;
            string NombreVideo = MisDispositivos[i].MonikerString;
            MiWebCam = new VideoCaptureDevice(NombreVideo);
            MiWebCam.NewFrame += new NewFrameEventHandler(Capturando);
            MiWebCam.Start();
        }

        private void Capturando(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = Imagen;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CerrarWebCam();
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (MiWebCam != null && MiWebCam.IsRunning)
            {
                pictureBox2.Image = pictureBox1.Image;

                pictureBox2.Image.Save(Path + "Reporte Dr. Bazan" + DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") + ".jpg", ImageFormat.Jpeg);
            }
        }
    }
}
