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
        }

        void mouseClick()
        {
            theMouse.GestureRecognizers.Add(new TapGestureRecognizer()
            { Command = new Command(() => { DisplayAlert("Task", "Clicked", "Ok"); }) });

        }

    }


}
