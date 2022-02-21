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

using System.Net.NetworkInformation;
using System.Threading;


namespace Nea_Phone
{
    public partial class MainPage : ContentPage
    {

        //public TcpClient client = null;
        //public NetworkStream stream = null;
        public CancellationTokenSource _tokenSource = null;
        //public Socket sender = null;
        public TcpClient client = null;
        public NetworkStream stream = null;
        public MainPage()
        {
            InitializeComponent();
            bool connected = false;
            bool tryingToConnect = false;

            IPAddress ipAddress = IPAddress.Parse("192.168.0.2");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 50000);










            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) =>
            {
                //Console.WriteLine("x: " + Convert.ToInt32(e.TotalX));
                //Console.WriteLine("y" + Convert.ToInt32(e.TotalY * -1)); //y coords in xamarin go up as u go down
                //Console.WriteLine(e.StatusType);
                if(e.StatusType == GestureStatus.Completed)
                {
                   //find a way to reset the x/y values
                }
                else { send(Convert.ToInt32(e.TotalX), Convert.ToInt32(e.TotalY)); }
               

            };

            theMouse.GestureRecognizers.Add(panGesture);
            theMouse.GestureRecognizers.Add(new TapGestureRecognizer() //when clicked =>
            {

                Command = new Command(() =>
                {
                    DisplayAlert("Click", "You clicked", "ok");
                    
                    //send(2147483647, 2147483647); //for testing sends 2 maximum int32 integers

                })
                
            });










            connectBtn.GestureRecognizers.Add(new TapGestureRecognizer() //when clicked =>
            { Command = new Command(() => {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (client == null && !tryingToConnect)//if it isnt connected nor trying to
                    {
                        connection();
                        connectTxt.Text = "Trying to connect....";
                        
                        tryingToConnect = true;
                        
                    }
                    else if (connected)//if its connected
                    {
                        close();
                        connectTxt.Text = "Disconnected";
                    }
                    else //its attempting to connect
                    {
                        _tokenSource.Cancel();

                        connectTxt.Text = "Cancelled....";

                    }
                }
                else
                {
                    DisplayAlert("Network", "There is no internet connection: Try again", "Ok");
                }





            }) });


           





            //networking here
            

            string getIp()
            {
                foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                   
                    return (address.ToString());
                }
                return null;
            }

                async Task connection()
            {
                _tokenSource = new CancellationTokenSource();
               
                await Task.Run(() =>
                    {
                        Console.WriteLine("Connection called");
                        var token = _tokenSource.Token;
                        tryingToConnect = true;
                        while (true)
                        {
                            if (token.IsCancellationRequested) //check that the impending connection request isnt cancelled
                            {

                                Console.WriteLine("Cancelled");


                                connected = false;
                                tryingToConnect = false;
                                return;
                            }
                            else
                            {
                                try
                                {
                                    Console.WriteLine("Trying again.....");
                                      client = new TcpClient("192.168.0.2", 50000); //tries to connect to a client on specified IP endpoint

                                      Console.WriteLine("Client connected");
                                      stream = client.GetStream(); //gets underlying network stream of the client
                                      Console.WriteLine("Got stream");

                                    //control variables(irrelevant to TCP connection)
                                    Console.WriteLine("PASSEDDDDDD");
                                    connected = true;
                                   
                                    tryingToConnect = false;
                                    Console.WriteLine("YOOOO");
                                   
                                    return;
                                }
                                catch 
                                {
                                    client = null;
                                    stream = null;
                                    Console.WriteLine("Error connecting or getting stream"); //will loop until connects
                               }
                                Thread.Sleep(1000); //retry every 1000 milliseconds

                            }
                        }
                        
                    }

                    );
                }

            
          
                    
                
            







                void send(int a, int b)
                {
                    if (connected)
                    {
                        byte[] x = new byte[4];
                        x = BitConverter.GetBytes(a);
                        byte[] y = new byte[4];
                        y = BitConverter.GetBytes(b);

                        byte[] msg = new byte[8];
                        Buffer.BlockCopy(x, 0, msg, 0, 4); //copies the two different 4 byte arrays into one 8 byte array
                        Buffer.BlockCopy(y, 0, msg, 4, 4);

                   
                    stream.Write(msg, 0, 8);//writes an 8 byte byte array to stream
                    
                    //opens memory slot for the stream data
                  
                        Console.WriteLine("Pass: Sent");
                    }
                    else
                    {
                        Console.WriteLine("Not connected g"); // doesnt attempt to send to a non-connected client
                    }

                }

                void close()
                {
                

                client.Close();
                stream.Dispose();
            }











            async Task ping(IPAddress ip)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Pinging...");
                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();

                    // Use the default Ttl value which is 128,
                    // but change the fragmentation behavior.
                    options.DontFragment = true;

                    // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    int timeout = 3600;
                    PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("PING SUCCESS!!!!");
                        Console.WriteLine("Address: {0}", reply.Address.ToString());
                        Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                        Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                        Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                        Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                    }
                    else
                    {
                        Console.WriteLine("Failed ping");
                    }
                });
            }
        }

        }
    }

