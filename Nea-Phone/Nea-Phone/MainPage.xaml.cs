using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;

namespace Nea_Phone
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
           
            mouseClick(); //once the program starts, you can click the button

            
            


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
            { Command = new Command(() => { DisplayAlert("Task", "Clicked", "Ok"); }) });
        }

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


}
