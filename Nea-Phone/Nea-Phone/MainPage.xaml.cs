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
        /*connection:
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 1333);

                string messageToSend = "YO Hows it going G";
                int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

                NetworkStream stream = client.GetStream(); //opens memory slot for the stream data
                stream.Write(sendData, 0, sendData.Length); //writes the bytes from 0 to end
                Console.WriteLine("Pass: Sent");

                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to connect...");
                goto connection; //retries connection if failed
            }
        */


            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) =>
            {
                Console.WriteLine("x: " + e.TotalX);
                Console.WriteLine("y" + e.TotalY * -1); //y coords in xamarin go up as u go down
                Console.WriteLine(e.StatusType);

            };
            theMouse.GestureRecognizers.Add(panGesture);

            theMouse.GestureRecognizers.Add(new TapGestureRecognizer()
            { Command = new Command(() => { DisplayAlert("U CLICKED!", "", "ok"); }) });
        }
      
        }
    }
