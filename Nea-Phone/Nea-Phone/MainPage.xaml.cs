using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Nea_Phone
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
           
            mouseClick(); //once the program starts, you can click the button
            setUpSocketServer();
            
            


            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) => {
                Console.WriteLine("x: " + e.TotalX);
               Console.WriteLine("y" + e.TotalY*-1); //y coords in xamarin go up as u go down
                Console.WriteLine(e.StatusType);
                
            };
            theMouse.GestureRecognizers.Add(panGesture);
        }




        private void mouseClick()
        {
            theMouse.GestureRecognizers.Add(new TapGestureRecognizer()
            { Command = new Command(() => { Send(); }) });

            //{ DisplayAlert("Task", "Clicked", "Ok"); }
        }


        public void Send()
        {
           
            var client = new TcpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 503);
            try
            {
                client.Connect(ipEndPoint);
                if (client.Connected)
                {
                    Console.WriteLine("Connected to server" + "\n");
                    
                    var writer = new StreamWriter(client.GetStream());
                    writer.AutoFlush = true;

                    Console.WriteLine("Connected: " + client.Connected);
                    if (client.Connected)
                    {
                        writer.WriteLine("HEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEy");
                    }
                    else
                    {
                        Console.WriteLine("send failed !");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Didnt connect");
            }
        }

        public void setUpSocketServer()
        {
            Console.WriteLine("Ready");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 503;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                Console.WriteLine("Past 1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                Console.WriteLine("Past 2");
                // Start listening for client requests.
                
                server.Start();
                Console.WriteLine("Past 3");
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
