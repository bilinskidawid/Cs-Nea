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

        public TcpClient client = null;
        public NetworkStream stream = null;



        public MainPage()
        {
            InitializeComponent();
            bool connected = false;
            bool tryingToConnect = false;
            CancellationTokenSource _tokenSource = null;
            IPAddress ipAddress = IPAddress.Parse("192.168.0.3");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 13333);
            
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) =>
            {
                Console.WriteLine("x: " + Convert.ToInt32(e.TotalX));
                Console.WriteLine("y" + Convert.ToInt32(e.TotalY * -1)); //y coords in xamarin go up as u go down
                Console.WriteLine(e.StatusType);
                //send(Convert.ToInt32(e.TotalX), Convert.ToInt32(e.TotalY * -1));

            };

            theMouse.GestureRecognizers.Add(panGesture);
            theMouse.GestureRecognizers.Add(new TapGestureRecognizer() //when clicked =>
            {

                Command = new Command(() =>
                {
                DisplayAlert("Click", "You clicked", "ok");
                    //send(50, 100);
                    for (int i = 0; i < 5; i++)
                    {
                        ping(IPAddress.Parse("192.168.0.3"));
                    }
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
                    tryingToConnect = true;
                    var token = _tokenSource.Token;
                
                await Task.Run(() =>
                    {
                        while (true)
                        {
                            if (token.IsCancellationRequested)
                            {
                                _tokenSource.Dispose();

                                connected = false;
                                tryingToConnect = false;
                                return;
                            }
                            else
                            {

                                Console.WriteLine("Connecting......");

                                var result = client.BeginConnect(ipAddress, 13333, null, null);

                                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10));

                                if (!success)
                                {
                                    Console.WriteLine("Failed to connect :(");
                                }
                                else
                                {
                                    Console.WriteLine("Client connected :D");
                                   
                                    stream = client.GetStream();
                                    Console.WriteLine("Got stream");


                                    Console.WriteLine("PASSEDDDDDD");
                                    connected = true;
                                    connectTxt.Text = "Connected!";
                                    tryingToConnect = false;
                                    return;

                                }








                                /*try
                                {
                                    Console.WriteLine("Trying again.....");
                                      client = new TcpClient("192.168.0.3", 13333);
                                     
                                      Console.WriteLine("Client connected");
                                      stream = client.GetStream();
                                      Console.WriteLine("Got stream");
                                    
                                   
                                    Console.WriteLine("PASSEDDDDDD");
                                    connected = true;
                                    connectTxt.Text = "Connected!";
                                    tryingToConnect = false;

                                    return;
                                }
                                catch 
                                {
                                    sender = null;
                                    Console.WriteLine("Error connecting or getting stream"); //will loop until connects
                               }*/
                                System.Threading.Thread.Sleep(3000);//retries every 3 seconds
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
                        Buffer.BlockCopy(x, 0, msg, 0, 4);
                        Buffer.BlockCopy(y, 0, msg, 4, 4);



                    //opens memory slot for the stream data
                    stream.Write(msg, 0, 8); //writes the bytes from 0 to end
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
                client = null;
                }
            }

        }
    }

