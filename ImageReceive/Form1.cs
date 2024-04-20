using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ImageReceive
{
    public partial class Form1 : Form
    {

        private const int PORT = 9051;
        private UdpClient client;
        private MemoryStream receivedImageStream;

        public Form1()
        {
            client = new UdpClient(PORT);
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReceiveImage();
        }

        private void ReceiveImage()
        {
            try
            {
                IPEndPoint senderEP = new IPEndPoint(IPAddress.Any, PORT);
                receivedImageStream = new MemoryStream();

                while (true)
                {
                    byte[] chunkData = client.Receive(ref senderEP);
                    receivedImageStream.Write(chunkData, 0, chunkData.Length);

                    if (chunkData.Length < 1024)
                        break;
                }

                receivedImageStream.Position = 0;

                pictureBox1.Image = Image.FromStream(receivedImageStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (receivedImageStream != null)
                    receivedImageStream.Close();
            }
        }
    }
}
