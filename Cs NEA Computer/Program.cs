using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cs_NEA_Computer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Enter program class");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserControl1 app = new UserControl1();
            Application.Run(new ComputerMain());
            app.InitializeComponent();
            
        }
    }
}
