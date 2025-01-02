using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOProject
{
    public partial class NewGame : Form
    {

        Socket server;

        private const int CellSize = 25;
        // Margen del tablero (espacio para las letras y números en el borde)
        private const int BoardOffsetX = 25; // Offset para el margen izquierdo
        private const int BoardOffsetY = 25; // Offset para el margen superior
        private const int GridSize = 200;    // Tamaño total de la cuadrícula (8 celdas de 25 px)
        string invites;
        string invited;
        int IDGame;
        string myname;
        private int[,] board = new int[8, 8]; // Representa el tablero como una matriz de 8x8
        public NewGame(Socket s, string invites, string invited, int IDGame,string myname)
        {
            InitializeComponent();
            
            this.server = s;
            this.invites = invites;
            this.invited = invited;
            this.IDGame = IDGame;
            this.myname = myname;

            // add Game ID and players name 
            Label playerInfo = new Label
            {
                Text = $"Game ID: {IDGame}\nPlayer 1: {invites}\nPlayer 2: {invited}",
                Location = new Point(send.Location.X, send.Location.Y + 40),
                AutoSize = true
            };
            this.Controls.Add(playerInfo);

            // Configurar el panel para permitir operaciones de soltar
            panel1.AllowDrop = true;

            // Conectar eventos para arrastrar y soltar en el panel
            panel1.DragEnter += Panel1_DragEnter;
            panel1.DragDrop += Panel1_DragDrop;

            // Configurar cada PictureBox (barco) para iniciar el arrastre
            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseDown += RotatePictureBox;
            pictureBox2.MouseDown += PictureBox_MouseDown;
            pictureBox2.MouseDown += RotatePictureBox;
            pictureBox4.MouseDown += PictureBox_MouseDown;
            pictureBox4.MouseDown += RotatePictureBox;
            pictureBox5.MouseDown += PictureBox_MouseDown;
            pictureBox5.MouseDown += RotatePictureBox;
            pictureBox6.MouseDown += PictureBox_MouseDown;
            pictureBox6.MouseDown += RotatePictureBox;

            // Hacer que cada PictureBox permita arrastre
            pictureBox1.AllowDrop = true;
            pictureBox2.AllowDrop = true;
            pictureBox4.AllowDrop = true;
            pictureBox5.AllowDrop = true;
            pictureBox6.AllowDrop = true;

        }

        private void NewGame_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void start_Click(object sender, EventArgs e)
        {
            string message = "9/" + invites + "/" + invited; ; // to the server
            byte[] msg = Encoding.ASCII.GetBytes(message);
            server.Send(msg);
            MessageBox.Show("Waiting for the other player to place ships...");


        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;
            Pen mypen1 = new Pen(Color.Black);
            int i = 0;
            while (i<=225)
            {
                graphics.DrawLine(mypen1, Convert.ToSingle(0), Convert.ToSingle(i), Convert.ToSingle(225), Convert.ToSingle(i));
                i=i+25;
            }

            int j= 0;
            while (j <=225)
            {
                graphics.DrawLine(mypen1, Convert.ToSingle(j), Convert.ToSingle(0), Convert.ToSingle(j), Convert.ToSingle(225));
                j = j + 25;
            }
            Font font = new Font("Arial", 12);
            graphics.DrawString("A",font, Brushes.Black,0, 25);
            graphics.DrawString("B", font, Brushes.Black, 0, 50);
            graphics.DrawString("C", font, Brushes.Black, 0, 75);
            graphics.DrawString("D", font, Brushes.Black, 0, 100);
            graphics.DrawString("E", font, Brushes.Black, 0, 125);
            graphics.DrawString("F", font, Brushes.Black, 0, 150);
            graphics.DrawString("G", font, Brushes.Black, 0, 175);
            graphics.DrawString("H", font, Brushes.Black, 0, 200);

            graphics.DrawString("1", font, Brushes.Black, 25,0);
            graphics.DrawString("2", font, Brushes.Black, 50,0);
            graphics.DrawString("3", font, Brushes.Black, 75, 0);
            graphics.DrawString("4", font, Brushes.Black, 100,0);
            graphics.DrawString("5", font, Brushes.Black, 125,0);
            graphics.DrawString("6", font, Brushes.Black, 150,0);
            graphics.DrawString("7", font, Brushes.Black, 175,0);
            graphics.DrawString("8", font, Brushes.Black, 200,0);


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;
            Pen mypen1 = new Pen(Color.Black);
            int i = 0;
            while (i <= 225)
            {
                graphics.DrawLine(mypen1, Convert.ToSingle(0), Convert.ToSingle(i), Convert.ToSingle(225), Convert.ToSingle(i));
                i = i + 25;
            }

            int j = 0;
            while (j <= 225)
            {
                graphics.DrawLine(mypen1, Convert.ToSingle(j), Convert.ToSingle(0), Convert.ToSingle(j), Convert.ToSingle(225));
                j = j + 25;
            }

            Font font = new Font("Arial", 12);
            graphics.DrawString("A", font, Brushes.Black, 0, 25);
            graphics.DrawString("B", font, Brushes.Black, 0, 50);
            graphics.DrawString("C", font, Brushes.Black, 0, 75);
            graphics.DrawString("D", font, Brushes.Black, 0, 100);
            graphics.DrawString("E", font, Brushes.Black, 0, 125);
            graphics.DrawString("F", font, Brushes.Black, 0, 150);
            graphics.DrawString("G", font, Brushes.Black, 0, 175);
            graphics.DrawString("H", font, Brushes.Black, 0, 200);

            graphics.DrawString("1", font, Brushes.Black, 25, 0);
            graphics.DrawString("2", font, Brushes.Black, 50, 0);
            graphics.DrawString("3", font, Brushes.Black, 75, 0);
            graphics.DrawString("4", font, Brushes.Black, 100, 0);
            graphics.DrawString("5", font, Brushes.Black, 125, 0);
            graphics.DrawString("6", font, Brushes.Black, 150, 0);
            graphics.DrawString("7", font, Brushes.Black, 175, 0);
            graphics.DrawString("8", font, Brushes.Black, 200, 0);
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Inicia el proceso de arrastre con el PictureBox actual
                PictureBox pictureBox = sender as PictureBox;
                pictureBox.DoDragDrop(pictureBox, DragDropEffects.Move);
            }
        }

        // Evento DragEnter del panel para permitir el arrastre
        private void Panel1_DragEnter(object sender, DragEventArgs e)
        {
            // Verificar que el objeto arrastrado es un PictureBox
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Move; // Permitir el movimiento
            }
            else
            {
                e.Effect = DragDropEffects.None; // No permitir otros tipos de objetos
            }
        }

        // Evento DragDrop del panel para soltar y ajustar el PictureBox en la cuadrícula
        private void Panel1_DragDrop(object sender, DragEventArgs e)
        {
            // Obtener el PictureBox que fue soltado
            PictureBox pictureBox = (PictureBox)e.Data.GetData(typeof(PictureBox));

            // Convertir la posición del mouse al sistema de coordenadas del panel
            Point dropLocation = panel1.PointToClient(new Point(e.X, e.Y));

            // Ajustar la posición dentro de las celdas de la cuadrícula
            int adjustedX = ((dropLocation.X - BoardOffsetX) / CellSize) * CellSize + BoardOffsetX;
            int adjustedY = ((dropLocation.Y - BoardOffsetY) / CellSize) * CellSize + BoardOffsetY;

            // Calcular la posición de la fila y la columna
            int col = (adjustedX - BoardOffsetX) / CellSize;
            int row = (adjustedY - BoardOffsetY) / CellSize;

            // Verificar si la celda está en la fila o columna de letras/números (no permite colocar barcos allí)
            if (row == 0 || col == 0)
            {
                MessageBox.Show("¡No puedes colocar los barcos en la fila de números o la columna de letras!");
                return;
            }

            // Calcular cuántas celdas ocupa la PictureBox
            int widthInCells = (pictureBox.Width + CellSize - 1) / CellSize; // Redondear hacia arriba
            int heightInCells = (pictureBox.Height + CellSize - 1) / CellSize; // Redondear hacia arriba

            // Verificar si todas las celdas que ocupa la PictureBox están libres
            for (int i = 0; i < heightInCells; i++)
            {
                for (int j = 0; j < widthInCells; j++)
                {
                    int currentRow = row + i;
                    int currentCol = col + j;

                    // Verificar que las celdas no estén fuera de los límites
                    if (currentRow >= 8 || currentCol >= 8)
                    {
                        MessageBox.Show("¡El barco se sale del tablero!");
                        return;
                    }

                    // Verificar si la celda ya está ocupada
                    if (board[currentRow, currentCol] == 1)
                    {
                        MessageBox.Show("¡Una o más celdas están ocupadas por otro barco!");
                        return;
                    }
                }
            }

            // Ahora que hemos validado que no haya celdas ocupadas, colocar el barco
            int offsetX = (CellSize - pictureBox.Width) / 2;
            int offsetY = (CellSize - pictureBox.Height) / 2;

            // Establecer la posición final del PictureBox
            pictureBox.Location = new Point(adjustedX + offsetX, adjustedY + offsetY);

            // Agregar el PictureBox al panel si no está ya en él
            if (!panel1.Controls.Contains(pictureBox))
            {
                panel1.Controls.Add(pictureBox);
            }

            // Marcar las celdas que la PictureBox ocupa como ocupadas en la matriz
            for (int i = 0; i < heightInCells; i++)
            {
                for (int j = 0; j < widthInCells; j++)
                {
                    int currentRow = row + i;
                    int currentCol = col + j;
                    board[currentRow, currentCol] = 1;  // Marcar la celda como ocupada
                }
            }
        }


        private void RotatePictureBox(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Verificar si es clic derecho
            {
                PictureBox pictureBox = sender as PictureBox;

                // Intercambiar el ancho y el alto para rotar
                int temp = pictureBox.Width;
                pictureBox.Width = pictureBox.Height;
                pictureBox.Height = temp;

                pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            }
        }

        private void send_Click(object sender, EventArgs e)
        {
            string soutput;
            if (myname != invites)
            {
                soutput = "10/" + invites + "/" + message.Text;
            }
            else
            {
                soutput = "10/" + invited + "/" + message.Text;
            }
            byte[] output = System.Text.Encoding.ASCII.GetBytes(soutput);
            server.Send(output);
        }

        private void message_TextChanged(object sender, EventArgs e)
        {

        }

        public void UpdateChat(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateChat(message)));
            }
            else
            {
                if (listBox1 != null)
                {
                    // Agregar el mensaje recibido al ListBox
                    listBox1.Items.Add(message);

                    // Autoscroll para mostrar siempre el último mensaje
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
            }
        }

        private void Done_button(object sender, EventArgs e)
        {
            string boardData = PrepareBoardData();

            // Preparar mensaje para el servidor
            string mensaje = $"14/{myname}/{boardData}";
            MessageBox.Show(mensaje);
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);

            // Enviar mensaje
            server.Send(msg);
            MessageBox.Show("Sent. Waiting for the other player...");
        }

        private string PrepareBoardData()
        {
            StringBuilder sb = new StringBuilder();

            // Recorrer la matriz de 8x8
            for (int i = 0; i < 8; i++)
            {
                // Crear una lista para almacenar los valores de cada fila
                List<string> rowValues = new List<string>();

                for (int j = 0; j < 8; j++)
                {
                    // Agregar el valor de cada celda (0 o 1)
                    rowValues.Add(board[i, j].ToString());
                }

                // Unir los valores de la fila con comas y agregar la fila al StringBuilder
                sb.Append(string.Join(",", rowValues));

                // Agregar una barra (/) para separar las filas
                sb.Append("/");

            }

            // Eliminar la última barra (/) al final
            if (sb.Length > 0)
            {
                sb.Length--;  // Eliminar la última barra
            }

            return sb.ToString();
        }

        public void SetGameID(int gameID)
        {
            this.IDGame = gameID;
            foreach (Control control in this.Controls)
            {
                if (control is Label && control.Text.Contains("Game ID"))
                {
                    control.Text = $"Game ID: {gameID}\nPlayer 1: {invites}\nPlayer 2: {invited}";
                    break;
                }
            }
        }

        public void UpdatePlayerInfo(string player1, string player2)
        {
            this.invites = player1;
            this.invited = player2;

            foreach (Control control in this.Controls)
            {
                if (control is Label && control.Text.Contains("Player 1"))
                {
                    control.Text = $"Game ID: {IDGame}\nPlayer 1: {player1}\nPlayer 2: {player2}";
                    break;
                }
            }
        }

        public void ShowPlaceShipsMessage()
        {
            MessageBox.Show("Set all ships and press DONE!");
        }






    }

}

