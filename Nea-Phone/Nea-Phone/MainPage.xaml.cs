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
using System.Threading;

namespace Nea_Phone
{
    public partial class MainPage : ContentPage
    {
        public bool connected = false;
        public TcpClient client = null;
        public NetworkStream stream = null;
        public bool tryingToConnect = false;


        public MainPage()
        {
            InitializeComponent();
            CancellationTokenSource _tokenSource = null;


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
                    send(50, 100);
                })
            });

                    connectBtn.GestureRecognizers.Add(new TapGestureRecognizer() //when clicked =>
            { Command = new Command(() => {

                if (client == null && !tryingToConnect)//if it isnt connected nor trying to
                { 
                    connection(client, stream);
                    connectTxt.Text = "Trying to connect....";
                }
                else if (connected)//if its connected
                {
                    close(client, stream);
                    connectTxt.Text = "Disconnected";
                }
                else //its attempting to connect
                {
                    _tokenSource.Cancel();

                    connectTxt.Text = "Cancelled....";

                }





            }) });








            //networking here

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
                    stream.Write(msg, 0, msg.Length); //writes the bytes from 0 to end
                    Console.WriteLine("Pass: Sent");
                }
                else
                {
                    Console.WriteLine("Not connected g"); // doesnt attempt to send to a non-connected client
                }

            }
            void close(TcpClient client, NetworkStream stream)
            {
                stream.Close();
                client.Close();
            }

            async Task connection(TcpClient client, NetworkStream stream)
            {
                _tokenSource = new CancellationTokenSource();
                var token = _tokenSource.Token;
                await Task.Run(() =>
                {

                    
                    tryingToConnect = true;
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
                            try
                            {
                                client = new TcpClient("192.168.0.3", 1333);
                                
                                stream = client.GetStream();
                                Console.WriteLine("Stream is not null");

                                connected = true;
                                connectTxt.Text = "Connected!";
                                tryingToConnect = false;

                                return;
                            }
                            catch 
                            {
                                Console.WriteLine("Error connecting or getting stream"); //will loop until connects
                            }
                            System.Threading.Thread.Sleep(1000);//retries every 1 second
                        }
                    }
                }
               
                );
            }
        }
      
        }
    }
