using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace SOProject
{
    public partial class SignUp : Form
    {

        Socket server;
        public SignUp(Socket s)
        {
            InitializeComponent();

            this.server = s;
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void email_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Username_TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                password_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void ConfirmPassword_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                ConfirmPassword_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void acceptbutton_Click(object sender, EventArgs e)
        {

            //Registers the user with its password

            try
            {
               if (password_TextBox.Text != ConfirmPassword_TextBox.Text)
               {
                    MessageBox.Show("The passwords do not match. Please, check it again."); //Error of not matching passwords
               }

               else
               {
                    byte[] register = System.Text.Encoding.ASCII.GetBytes("1/" + email.Text + "/" + Username_TextBox.Text + "/" + password_TextBox.Text);
                    server.Send(register);

                    byte[] msg2 = new byte[1024];
                    server.Receive(msg2);
                    string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                    int codigo = Convert.ToInt32(trozos[0]);
                    string mensaje = trozos[1].Split('\0')[0];

                    switch (Convert.ToInt32(mensaje))
                    {
                        case 0:
                            MessageBox.Show("The email is already registered. Use another one.");
                            break;

                        case 1:
                            MessageBox.Show("This username already exists. Please, choose another one.");
                            break;

                        case 2:
                            MessageBox.Show("The email must have between 15 and 80 characters.");
                            break;

                        case 3:
                            MessageBox.Show("Your username must have between 3 and 80 characters.");
                            break;

                        case 4:
                            MessageBox.Show("Your passwrd must have between 8 and 20 characters");
                            break;


                        case 5:
                            MessageBox.Show("Register was unsuccessful. Please, try again.");
                            break;

                        case 6:
                            MessageBox.Show("New user registered successfully!");
                            this.Hide();
                            break;
                    }
                }

            }

            catch (SocketException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show("The connection with the sever failed");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                password_TextBox.UseSystemPasswordChar = false;
            }

            else
            {
                password_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                ConfirmPassword_TextBox.UseSystemPasswordChar = false;
            }

            else
            {
                ConfirmPassword_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void email_Leave(object sender, EventArgs e)
        {
            if (email.Text == String.Empty)
            {
                email.Text = "Email";
                email.ForeColor = Color.Silver;
            }
        }

        private void Username_TextBox_Leave(object sender, EventArgs e)
        {
            if (Username_TextBox.Text == String.Empty)
            {
                Username_TextBox.Text = "Username";
                Username_TextBox.ForeColor = Color.Silver;
            }
        }

        private void password_TextBox_Leave(object sender, EventArgs e)
        {
            if (password_TextBox.Text == String.Empty)
            {
                password_TextBox.Text = "Password";
                password_TextBox.ForeColor = Color.Silver;
                password_TextBox.UseSystemPasswordChar = false;

            }

            else
            {
                password_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void ConfirmPassword_TextBox_Leave(object sender, EventArgs e)
        {
            if (ConfirmPassword_TextBox.Text == String.Empty)
            {
                ConfirmPassword_TextBox.Text = "Confirm Password";
                ConfirmPassword_TextBox.ForeColor = Color.Silver;
                password_TextBox.UseSystemPasswordChar = false;

            }

            else
            {
                password_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void Username_TextBox_Enter(object sender, EventArgs e)
        {
            if (Username_TextBox.Text == "Username")
            {
                Username_TextBox.Text = String.Empty;
                Username_TextBox.ForeColor = Color.Black;
            }
        }

        private void password_TextBox_Enter(object sender, EventArgs e)
        {
            if (password_TextBox.Text == "Password")
            {
                password_TextBox.Text = String.Empty;
                password_TextBox.ForeColor = Color.Black;
            }

            if (password_TextBox.UseSystemPasswordChar == false)
            {
                password_TextBox.UseSystemPasswordChar = true;
            }
        }

        private void email_Enter(object sender, EventArgs e)
        {
            if (email.Text == "Email")
            {
                email.Text = String.Empty;
                email.ForeColor = Color.Black;
            }
        }

        private void ConfirmPassword_TextBox_Enter(object sender, EventArgs e)
        {

            if (ConfirmPassword_TextBox.Text == "Confirm Password")
            {
                ConfirmPassword_TextBox.Text = String.Empty;
                ConfirmPassword_TextBox.ForeColor = Color.Black;
            }

            if (ConfirmPassword_TextBox.UseSystemPasswordChar == false)
            {
                ConfirmPassword_TextBox.UseSystemPasswordChar = true;
            }
        }
    }
}

