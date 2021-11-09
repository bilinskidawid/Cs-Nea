using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTCP;

namespace Cs_NEA_Computer
{
    public partial class ComputerMain : Form
    {
        public ComputerMain()
        {
            InitializeComponent();

            Console.WriteLine("Main mark 1");
            setUpServer();
            
            
            
        }

        public void setUpTestClient()
        {
            Console.WriteLine("TestClient method started");
            int port = 503;
            string msg = "elloo laddie";
            var client = new SimpleTcpClient();
            client.Connect("127.0.0.1", port);
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

        private void button1_Click(object senders, EventArgs es)
        {
            setUpTestClient();
        }

        private void ComputerMain_Load(object sender, EventArgs e)
        {

        }
    }
}
