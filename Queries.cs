using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Threading;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace SOProject
{
    public partial class Queries : Form
    {
        Socket server;
        Thread atender;
        string myname;
        delegate void hide_window_delegate();
        int acceptedInvitation = 0;
        public string invites;
        public string invited;
        public int idgame;
        NewGame ng;
        ChatRoom room;
        int inv = 0;
        private Dictionary<int, ChatRoom> activeChatRooms = new Dictionary<int, ChatRoom>();

        public Queries(Socket s, string conectados, string myname)
        {
            InitializeComponent();
            this.server = s;
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
            Console.WriteLine("Hilo iniciado...");
            ConnectedList(conectados);
            this.myname = myname;
        }

        public void RemoveChatRoom(int roomID)
        {
            if (activeChatRooms.ContainsKey(roomID))
            {
                activeChatRooms.Remove(roomID);
            }
        }
        private void Queries_Load(object sender, EventArgs e)
        {
        }

        private void PlayerGame_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void winner_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void query_Click(object sender, EventArgs e)
        {
            try
            {
                string message = ""; // Prepare the message to send to the server

                if (PlayerGame.Checked)
                {
                    message = "3/";

                }
                else if (winner.Checked)
                {
                    message = "4/" +gameid.Text; // The code for fetching the winner by game ID

                }
                else if (gamesPlayed.Checked)  // New case for Games Played by a Player
                {
                    message = "6/" + playerName.Text; // "6" represents the new query

                }

                else if (mygames.Checked)
                {
                    message = "11/"+myname;

                }
                else if (resultofthegame.Checked)
                { 
                    message = "12/" + myname + "/" + Playername2.Text;
                }
                else if (dateQuery.Checked) // New case for games during selected dates
                {
                    DateTime startDate = monthCalendar1.SelectionStart;
                    DateTime endDate = monthCalendar1.SelectionEnd;

                    // changing format
                    string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                    string formattedEndDate = endDate.ToString("yyyy-MM-dd");

                    message = $"13/{formattedStartDate}/{formattedEndDate}";
                }

                byte[] msg = Encoding.ASCII.GetBytes(message);
                server.Send(msg);
                Console.WriteLine($"Message sent: {message}");
            }

            catch (SocketException ex)
            {
                // If there's a socket exception, show an error message
                MessageBox.Show("Error connecting to the server: " + ex.Message);
            }
        }

            private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gameid_TextChanged(object sender, EventArgs e)
        {

        }
        private void gamesPlayed_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void playerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void newgame_Click(object sender, EventArgs e)
        {
            if (acceptedInvitation == 1)
            {
                ThreadStart ts = delegate { PonerEnMarchaFormulario(invites,invited,idgame, myname); };
                Thread T = new Thread(ts);
                T.Start();
            }

            else
            {
                MessageBox.Show("You must invite someone and they have to accept your invitation to start a new game.");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "5/";
            byte[] msg = Encoding.ASCII.GetBytes(message);
            server.Send(msg);   
        }

        public void query1(string message)
        {
            string formattedMessage = message.Replace(",", "\n");
            MessageBox.Show(formattedMessage);
        }

        public void query2(string message)
        {
            MessageBox.Show(message);
        }

        public void query3(string message)
        {
            MessageBox.Show(message);
        }
        public void query4(string [] message)
        {
            string[] trimmedMessage = message.Skip(1).ToArray();
            string combinedMessage = string.Join("/", trimmedMessage);
            string formattedMessage = combinedMessage.Replace("/", "\n");
            MessageBox.Show(formattedMessage);
        }

        public void query5(string [] message)
        {
            Console.WriteLine("Received query 5 response: " + string.Join("/", message)); // Debugging message

            string[] trimmedMessage = message.Skip(1).ToArray();
            string combinedMessage = string.Join("/", trimmedMessage);
            string formattedMessage = combinedMessage.Replace("/", "\n");
            MessageBox.Show(formattedMessage);
        }

        public void query6(string[] message) //List of games played in a given period of time
        {
            Console.WriteLine("Received query 6 response: " + string.Join("/", message)); // Debugging message

            string gamesMessage = string.Join("/", message.Skip(1).ToArray()).Split('\0')[0];
            string formattedMessage = gamesMessage.Replace("/", "\n");
            MessageBox.Show($"Games during the selected dates:\n{formattedMessage}");
        }

        public void ConnectedList(string mensaje)
        {
            Console.WriteLine("ConnectedList ejecutado con: " + mensaje);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(ConnectedList), mensaje);
            }
            else
            {
                try
                {
                    string[] trozos = mensaje.Split(',');
                    string connected = trozos[0];
                    label_users_connected.Text = connected;

                    dataGridView1.Rows.Clear();
                    for (int i = 1; i < trozos.Length; i++)
                    {
                        dataGridView1.Rows.Add(false, trozos[i]);
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show("Error connecting to the server: " + ex.Message);
                }
            }
        }

        private void Queries_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Ensure the server is valid before proceeding
                if (server != null)
                {
                    string mensaje = "0/"; // Message to notify server for player removal
                    byte[] msg = Encoding.ASCII.GetBytes(mensaje);

                    // Send the message
                    server.Send(msg);

                    // Gracefully shut down and close the socket
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Socket error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Display the disconnect message, ensuring it always happens
                MessageBox.Show("Disconnected");
            }
        }

        private void label_users_connected_Click(object sender, EventArgs e)
        {

        }

        public void AtenderServidor() //receive ALL the messages from the server!!
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("I am executing this while");
                    //Recibimos mensaje del servidor
                    byte[] msg2 = new byte[1024];
                    Array.Clear(msg2, 0, msg2.Length);
                    server.Receive(msg2);
                    string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                    string codigo = (trozos[0]);
                    string mensaje;

                    switch (codigo)
                    {
                        case "1": //query 1
                            string mess = trozos[1].Split('\0')[0];
                            query1(mess);
                            break;

                        case "2": //query 2
                            mensaje = trozos[1].Split('\0')[0];
                            query2(mensaje);
                            break;
                        case "3": //query 3
                            mensaje = trozos[1].Split('\0')[0];
                            query3(mensaje);
                            break;

                        case "4": //Connected List 
                            mensaje = trozos[1].Split('\0')[0];
                            Console.WriteLine("Llamando a ConnectedList...");
                            ConnectedList(mensaje);
                            break;
                        case "7": //invitaion
                            string pinvites = trozos[1];
                            string message = trozos[2];
                            DialogResult dialogResult = MessageBox.Show(message, "You're about to accept this game", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                string accepted = "8/" + pinvites + "/" + "1/"+myname;                                
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(accepted);
                                server.Send(msg);
                                acceptedInvitation = 1;
                                MessageBox.Show("You can now press the start button. Enjoy the game!");
                                invites = pinvites;
                                invited = myname;

                            }
                            else //not accepted
                            {
                                string accepted = "8/" + pinvites + "/" + "0/" + myname;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(accepted);
                                server.Send(msg);
                            }
                            break;

                        case "8":
                            { // Invite notification
                              // Ensure the message format is valid
                                if (trozos.Length >= 3) // Expecting at least "8/roomID/host"
                                {
                                    int receivedID;
                                    bool parseSuccess = int.TryParse(trozos[1], out receivedID);
                                    string host = trozos[2];

                                    if (!parseSuccess)
                                    {
                                        MessageBox.Show("Failed to parse Room ID from the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                    }

                                    Console.WriteLine($"Invite received. Room ID: {receivedID}, Host: {host}"); // Debug log

                                    DialogResult result = MessageBox.Show(
                                        $"{host} has invited you to a chatroom. Do you accept?",
                                        "Chatroom Invitation",
                                        MessageBoxButtons.YesNo);

                                    int response = (result == DialogResult.Yes) ? 1 : -1;

                                    // Send response back to the server
                                    string answer = $"8/{receivedID}/{myname}/{response}";
                                    byte[] msg = Encoding.ASCII.GetBytes(answer);
                                    server.Send(msg);

                                    if (response == 1)
                                    {
                                        NewChat(receivedID, host); // Open the chatroom if accepted
                                    }
                                }
                                else
                                {
                                    // Log incomplete data
                                    Console.WriteLine($"Incomplete invite data received: {string.Join("/", trozos)}");
                                    MessageBox.Show("Invalid invitation data received from the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                break;
                            }


                        case "9": //get id if you are the onw that invites
                            int gameID = Convert.ToInt32(trozos[1]);
                            string player1 = trozos[2];
                            string player2 = trozos[3];

                            if (ng != null)
                            {
                                ng.SetGameID(gameID);
                                ng.UpdatePlayerInfo(player1, player2);
                                ng.ShowPlaceShipsMessage();
                            }

                            break;

                        case "10": // Chatroom messages
                            if (trozos.Length >= 4)
                            {
                                int roomID = Convert.ToInt32(trozos[1]);
                                string sender = trozos[2];
                                string chatMessage = trozos[3];

                                // Combine sender and message for display
                                string formattedMessage = $"{sender}: {chatMessage}";

                                if (activeChatRooms.ContainsKey(roomID))
                                {
                                    activeChatRooms[roomID].UpdateChat(formattedMessage);
                                }
                                else
                                {
                                    MessageBox.Show($"Received message for unknown Room ID {roomID}: {formattedMessage}");
                                }
                            }
                            break;
                        case "11":
                            query4(trozos);
                            break;
                        case "12":
                            query5(trozos);
                            break;
                        case "13":
                            query6(trozos);
                            break;
                    }
                }
            }

            catch (SocketException ex)
            {
                MessageBox.Show($"Socket error (Atender): {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error (Atender): {ex.Message}");
            }
        }

        private void PonerEnMarchaFormulario(string p1, string p2, int id, string myname)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => PonerEnMarchaFormulario(p1, p2, id,myname)));
            }
            else
            {
                ng = new NewGame(server, p1, p2, id,myname);
                ng.Show();
            }
        }
        private void NewChat(int roomID, string host)
        {
            // Ensure the chatroom doesn't already exist
            if (!activeChatRooms.ContainsKey(roomID))
            {
                ChatRoom newRoom = new ChatRoom(server, myname, roomID, host);
                activeChatRooms[roomID] = newRoom; // Add to active chatrooms
                newRoom.Show();
            }
            else
            {
                MessageBox.Show($"Chatroom with Room ID {roomID} is already open.");
            }
        }

        private void mygames_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void resultofthegame_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Playername2_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void invite_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedNames = new List<string>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var cellValue = row.Cells["invitation"].Value;
                    bool isChecked = cellValue is bool && (bool)cellValue;

                    if (isChecked)
                    {
                        string playerName = row.Cells["Column1"].Value.ToString();
                        if (!string.IsNullOrEmpty(playerName) && playerName != myname)
                        {
                            selectedNames.Add(playerName);
                        }
                    }
                }

                if (selectedNames.Count > 0 && selectedNames.Count <= 5)
                {
                    string namesForServer = string.Join("/", selectedNames);
                    string message = $"7/{myname}/{namesForServer}";
                    byte[] msg = Encoding.ASCII.GetBytes(message);
                    server.Send(msg);

                    // Open ChatRoom for the host
                    if (!activeChatRooms.ContainsKey(-1)) // -1 is temporary until room ID is assigned
                    {
                        ChatRoom newRoom = new ChatRoom(server, myname, -1, myname); // -1 for roomID
                        activeChatRooms[-1] = newRoom;
                        newRoom.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Please select 1–5 players for the chatroom.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /*private void chat_Click(object sender, EventArgs e)
        {
            try
            {
                // Chatroom 
                NewChat(myname);
                Console.WriteLine("ChatRoom open: " + myname);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening chat: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}
