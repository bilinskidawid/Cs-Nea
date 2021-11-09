using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Computer_Side
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            setUpServer();
        }
        public void setUpTestClient()
        {
            Console.WriteLine("TestClient method started");
            int port = 503;
            string msg = "elloo laddie";
            var client = new SimpleTcpClient();

            try //checking to make sure there is an open port
            {
                client.Connect("127.0.0.1", port);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot connect, port is not open");
                return;
            }


            if (msg != "")
            {
                client.WriteLine(msg);
            }
            client.Disconnect();
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
                var ep = e.TcpClient.Client.RemoteEndPoint;
                var msg = Encoding.UTF8.GetString(e.Data);
                Console.WriteLine($"SERVER: Received {msg} from client");


            };
            server.Start(503);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setUpTestClient();
        }

    }
}
