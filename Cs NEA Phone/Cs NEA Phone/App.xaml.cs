using Cs_NEA_Phone.Services;
using Cs_NEA_Phone.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cs_NEA_Phone
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
