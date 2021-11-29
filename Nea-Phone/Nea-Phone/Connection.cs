using Android.Widget;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Nea_Phone
{

    public class Connection
    {
        private static Connection _instance;

        public static Connection Instance
        {
            get
            {
                if (_instance == null) _instance = new Connection();
                return _instance;
            }
        }

        

    }
}


    
       
    
