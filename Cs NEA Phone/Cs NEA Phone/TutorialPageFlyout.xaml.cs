using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cs_NEA_Phone
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TutorialPageFlyout : ContentPage
    {
        public ListView ListView;

        public TutorialPageFlyout()
        {
            InitializeComponent();

            BindingContext = new TutorialPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class TutorialPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<TutorialPageFlyoutMenuItem> MenuItems { get; set; }

            public TutorialPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<TutorialPageFlyoutMenuItem>(new[]
                {
                    new TutorialPageFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new TutorialPageFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new TutorialPageFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new TutorialPageFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new TutorialPageFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}