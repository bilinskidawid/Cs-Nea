using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Control = System.Windows.Forms.Control;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Computer_Side
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("User32.dll")] //importing 
        private static extern bool SetCursorPos(int X, int Y); //imported method to set the cursor position on screen
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
            var point = Control.MousePosition; //current mouse position
            SetCursorPos(Convert.ToInt32(x) + point.X, Convert.ToInt32(y) + point.Y); //adds current coords to parameters passed.
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) //if its connected to a network
            {
                if (online)//checks if user wants it to be off or on
                {
                    online = false;
                    endConnection(); //calls close method
                }
                else
                {
                    setUpListener(); //calls set up connection method
                    //MessageBox.Show("Set up listener");
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

            listener.Stop();//stops listeneing
            client.Close();


        }
        

        async Task setUpListener()
        {

            await Task.Run(() =>//makes it async(run in background)
            {
                //Console.WriteLine("PASS");
                IPAddress address = IPAddress.Parse(GetLocalIPv4(NetworkInterfaceType.Wireless80211));//gets IP address of WIFI card
                IPEndPoint endpoint = new IPEndPoint(address, 50000);

                listener = new TcpListener(endpoint); //starts listening on port 50000

                listener.Start();
                MessageBox.Show("Waiting for a connection.");
                client = listener.AcceptTcpClient(); //this is where the code stops, while waiting for a connection
                
                MessageBox.Show("Connected!!!!!!!!!!!");

                stream = client.GetStream();//gets network stream of the client to then read/write

               // MessageBox.Show("Stream Gotten");
                online = true;//control booleans for flow control
                connected = true;
               

                listen();//calls the method that handles the incoming data
                return;
            });
        }
       

        async Task listen()
        {

            await Task.Run(() =>
            {


                byte[] buffer = new byte[8];//received data goes into this buffer
                while (stream.CanRead) //loop while connected to check for messages
                {
                    //MessageBox.Show("listeninggg");


                   stream.Read(buffer, 0, 8);//should read first 8 bytes from stream 

                   // MessageBox.Show("Pass 1");
                        
                       
                        int x = BitConverter.ToInt32(buffer, 0); //reads first 4 bytes, offset by 0
                        int y = BitConverter.ToInt32(buffer, 4);//reads 4 bytes, offset by 4
                        
                        moveMouse(x, y); //calls method to handle 

                    Thread.Sleep(1); //to not make it too taxing on system

                }

                MessageBox.Show("Connection","Connection ended");
            });
        }








        public string GetLocalIPv4(NetworkInterfaceType _type) //external code to just get the local address
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;

        }

    }

}


