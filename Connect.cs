using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOProject;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace SOProject
{
    public partial class Connect : Form
    {
        public Socket server;
        public Connect()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 50215);
            

            //We create the socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);
                MessageBox.Show("Connected");
                Main m = new Main(server);
                m.ShowDialog();
                this.Hide();

            }
            catch (SocketException)
            {
                MessageBox.Show("Error connecting to server", "Error");
                this.Close();

            }
        }

        private void Connect_Load(object sender, EventArgs e)
        {
         
        }

    }
}

