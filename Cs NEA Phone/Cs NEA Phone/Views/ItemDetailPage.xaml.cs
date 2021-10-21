using Cs_NEA_Phone.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Cs_NEA_Phone.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}