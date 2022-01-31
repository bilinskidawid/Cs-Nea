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

        public TcpListener listener = null;
        public TcpClient client = null;
        
        public NetworkStream stream = null;
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        public void moveMouse(int x, int y)
        {
            var point = Control.MousePosition;
            SetCursorPos(x + point.X, y + point.Y); //adds current coords to parameters passed.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                if (online)//checks if user wants it to be off or on
                {
                    online = false;
                    endConnection();
                }
                else
                {
                    setUpListener();
                }
            }
            else
            {
                Console.WriteLine("Not connected to the internet g");
            }
        }

        private void endConnection()
        {
            Console.WriteLine("Ending connection now.....");

            if (listener != null)
            {
                listener.Stop();
            }
                client = null;
        }

        async Task setUpListener()
        {

            await Task.Run(() =>
            {
                Console.WriteLine("PASS");
                
                listener = new TcpListener(System.Net.IPAddress.Any, 1333);
                listener.Start();
                Console.WriteLine("Waiting for a connection.");
                client = listener.AcceptTcpClient();
                Console.WriteLine("Connected!!!!!!!!!!!");
                stream = client.GetStream();
                
                Console.WriteLine("Client accepted.");
                listen();




            });
        }


        async Task listen()
        {

            await Task.Run(() =>
            {
                while (connected) //loop while connected to check for messages
                {
                  
                    try
                    {
                        byte[] buffer = new byte[8];
                        stream.Read(buffer,0,8);
                        int x = BitConverter.ToInt32(buffer, 0); //reads first 4 bytes, offset by 0
                        int y = BitConverter.ToInt32(buffer, 4); //reads 4 bytes, offset by 4

                        Console.WriteLine("X val recieved: " + x);
                        Console.WriteLine("Y val recieved: " + y);
                        moveMouse(x, y);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Incoming message wasn't of the correct format");
                        


                        break;
                    }


                }

                Console.WriteLine("Connection ended");
            });
        }





            
            

        
    }

            }
      
  
