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
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Computer_Side
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("User32.dll")] //importing 
        private static extern bool SetCursorPos(int X, int Y);
        public bool connected = true;
        public bool online = false;
        public MainWindow()
        {
            InitializeComponent();

            
        }

        public void moveMouse(int x, int y)
        {
            var point = Control.MousePosition;
            SetCursorPos(Convert.ToInt32(x) + point.X, Convert.ToInt32(y) + point.Y); //adds current coords to parameters passed.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (online)
            {
                online = false;
            }
            else { 
                online = true;
                setUpConnection();
            }
        }

        private void setUpConnection() //NEED TO MAKE THIS ASYNCHRONOUS
        {
            Console.WriteLine("PASS");
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 1333);
            listener.Start();
            Console.WriteLine("Waiting for a connection.");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client accepted.");
            while (connected) //loop while connected to check for messages
            {
                
                System.Threading.Thread.Sleep(500);


                NetworkStream stream = client.GetStream();

                StreamWriter streamWrite = new StreamWriter(client.GetStream());//set up read/write abilities
                try
                {
                    byte[] buffer = new byte[8];
                    stream.Read(buffer, 0, 8);
                    int x = BitConverter.ToInt32(buffer, 0);
                    int y = BitConverter.ToInt32(buffer, 4);

                    Console.WriteLine("X val recieved: " + x);
                    Console.WriteLine("Y val recieved: " + y);
                    moveMouse(x, y);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Incoming message wasn't of the correct format");
                    streamWrite.WriteLine(e.ToString());
                    stream.Flush();

                    break;
                }


            }

            Console.WriteLine("Connection ended");
        }

        
    }

            }
      
  
