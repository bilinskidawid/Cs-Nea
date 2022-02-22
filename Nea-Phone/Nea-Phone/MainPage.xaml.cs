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
            


            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) =>
            {
                
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
                        connection(); //calls connection establishing method
                        connectTxt.Text = "Trying to connect....";
                        
                        tryingToConnect = true;
                        
                    }
                    else if (connected)//if its connected
                    {
                        close(); //calls closing method
                        connectTxt.Text = "Disconnected";
                    }
                    else //its attempting to connect
                    {
                        _tokenSource.Cancel();//cancels the async method

                        connectTxt.Text = "Cancelled....";

                    }
                }
                else
                {
                    DisplayAlert("Network", "There is no internet connection: Try again", "Ok");
                }





            }) });



            //networking here
            

            string getIp()//external function to get the Ip of the device
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
                        var token = _tokenSource.Token;//gets a token to be able to be cancelled mid-execution of the async method
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
                                   
                                    connected = true;
                                   
                                    tryingToConnect = false;
                                   
                                   
                                    return;
                                }
                                catch 
                                {
                                    client = null;//makes sure that the client and stream arent partly changed etc
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
                    if (connected)//if theres an active tcp connection
                    {
                        byte[] x = new byte[4];
                        x = BitConverter.GetBytes(a); //makes the "x" value into a 4 byte array
                        byte[] y = new byte[4];
                        y = BitConverter.GetBytes(b);//makes the "y" value into a 4 byte array

                        byte[] msg = new byte[8];//final byte array that gets sent
                        Buffer.BlockCopy(x, 0, msg, 0, 4); //copies the two different 4 byte arrays into one 8 byte array
                        Buffer.BlockCopy(y, 0, msg, 4, 4);

                   
                    stream.Write(msg, 0, 8);//writes an 8 byte byte array to stream
                    
                   
                  
                        Console.WriteLine("Pass: Sent");
                    }
                    else
                    {
                        Console.WriteLine("Not connected g"); // doesnt attempt to send to a non-connected client
                    }

                }

                void close()
                {
                

                client.Close();//closes the tcp connection
                stream.Dispose();//releases all stream resources
            }



            async Task ping(IPAddress ip)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Pinging...");
                    Ping pingSender = new Ping();//creates a ping instance
                    PingOptions options = new PingOptions();

                    
                    options.DontFragment = true;//sends only one packet

                    // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    int timeout = 3600;//3.6 second time to live
                    PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)//outputs stats on the successful ping
                    {
                        Console.WriteLine("PING SUCCESS!!!!");
                        Console.WriteLine("Address: " + reply.Address.ToString());
                        Console.WriteLine("RoundTrip time: " + reply.RoundtripTime);
                        Console.WriteLine("Time to live: " + reply.Options.Ttl);
                        Console.WriteLine("Don't fragment: " + reply.Options.DontFragment);
                        Console.WriteLine("Buffer size: " + reply.Buffer.Length);
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

