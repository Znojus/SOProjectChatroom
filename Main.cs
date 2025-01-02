using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;

namespace SOProject
{
    public partial class Main : Form
    {
        Socket server;
        string lista;
        string myname;
        public Main(Socket s)
        {
            InitializeComponent();

            this.server = s;
            //CheckForIllegalCrossThreadCalls = false;

        }

        private void signupbutton_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp(server);
            this.Hide();
            signUp.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            MessageBox.Show("Disconnected");
            this.Close();
        }

        private void loginbutton_Click(object sender, EventArgs e)
        {
            string soutput = "2/" + usernameText.Text + "/" + passwordText.Text;
            byte[] output = System.Text.Encoding.ASCII.GetBytes("2/" + usernameText.Text + "/" + passwordText.Text);
            server.Send(output);

            byte[] msg2 = new byte[1024];
            server.Receive(msg2);
            string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
            int codigo = Convert.ToInt32(trozos[1]);

            switch (codigo)
            {
                case 0:
                    MessageBox.Show("The userame does not exist.");
                    break;
                case 1:
                    MessageBox.Show("The data was not found in the database");
                    break;

                case 2:
                    MessageBox.Show("Login error. Please, try again.");
                    break;

                case 3:
                    MessageBox.Show("Login successful");

                    lista = trozos[4];
                    myname = trozos[2];
                    Queries q = new Queries(server, lista, myname);
                    q.ShowDialog();
                    this.Hide();
                    break;
                case 4:
                    MessageBox.Show("The password is not correct.");
                    break;
                case 5:
                    MessageBox.Show("You are already logged in");
                    break;
                default:
                    MessageBox.Show(Convert.ToString(codigo));
                    break;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (server != null && server.Connected)
                {
                    // Gracefully shut down the server connection
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                }
                MessageBox.Show("Disconnected");
            }
            catch (SocketException ex)
            {
                //MessageBox.Show($"Socket error during shutdown: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show("Main window was clossed");
            }
        }

        private void usernameText_Enter(object sender, EventArgs e)
        {
            if (usernameText.Text == "Username")
            {
                usernameText.Text = String.Empty;
                usernameText.ForeColor = Color.Black;
            }
        }

        private void passwordText_Enter(object sender, EventArgs e)
        {
            if (passwordText.Text == "Password")
            {
                passwordText.Text = String.Empty;
                passwordText.ForeColor = Color.Black;
            }

            if (passwordText.UseSystemPasswordChar == false)
            {
                passwordText.UseSystemPasswordChar = true;
            }
        }

        private void usernameText_Leave(object sender, EventArgs e)
        {
            if (usernameText.Text == String.Empty)
            {
                usernameText.Text = "Username";
                usernameText.ForeColor = Color.Silver;
            }
        }

        private void passwordText_Leave(object sender, EventArgs e)
        {
            if (passwordText.Text == String.Empty)
            {
                passwordText.Text = "Password";
                passwordText.ForeColor = Color.Silver;
                passwordText.UseSystemPasswordChar = false;

            }

            else
            {
                passwordText.UseSystemPasswordChar = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                passwordText.UseSystemPasswordChar = false;
            }

            else
            {
                passwordText.UseSystemPasswordChar = true;
            }
        }

        private void passwordText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


