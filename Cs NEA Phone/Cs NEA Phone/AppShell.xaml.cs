using Cs_NEA_Phone.ViewModels;
using Cs_NEA_Phone.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Cs_NEA_Phone
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(WalkthroughItemPage), typeof(WalkthroughItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
