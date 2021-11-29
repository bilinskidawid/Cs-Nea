using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleTCP;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using System.Net.Sockets;
using System.Net;

namespace Computer_Side
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("User32.dll")] //importing 
        private static extern bool SetCursorPos(int X, int Y);
        public MainWindow()
        {
            InitializeComponent();
            
            setUpSocketServer();
            
        }


        public void moveMouse(double x, double y)
        {
            var point = Control.MousePosition;
            SetCursorPos(Convert.ToInt32(x) + point.X, Convert.ToInt32(y) + point.Y); //testing to make sure setting beyond screen limits doesnt throw errors
        }
        public void setUpTestClient()
        {
            Console.WriteLine("TestClient method started");
            int port = 503;
            string msg = "250,300";
            


            var client = new SimpleTcpClient();

            try //checking to make sure there is an open port
            {
                client.Connect("127.0.0.1", port);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot connect, port/destination is not open");
                return;
            }



            client.Write(msg);
        }

        public void setUpServer()
        {
            var server = new SimpleTcpServer();

            server.ClientConnected += (sender, e) =>
                        Console.WriteLine($"Server: {e.Client.RemoteEndPoint} connected!");

            server.ClientDisconnected += (sender, e) =>
                Console.WriteLine($"SERVER: Client disconnected!");

            server.DataReceived += (sender, e) =>
            {
                var ep = e.TcpClient.Client.RemoteEndPoint; //clients IP address and port communication
                var msg = Encoding.UTF8.GetString(e.Data); 
                string x = msg.Split(',')[0]; //first part of msg, before comma
                string y = msg.Split(',')[1];
                moveMouse(Double.Parse(x), Double.Parse(y));
                
                


            };
            server.Start(503);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            setUpTestClient();



           
        }
        public void setUpSocketServer()
        {
            Console.WriteLine("Ready");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 503;
                

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

    }
}
