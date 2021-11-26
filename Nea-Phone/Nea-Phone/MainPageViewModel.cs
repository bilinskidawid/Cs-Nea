using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nea_Phone
{
    public class MainPageViewModel : INotifyPropertyChanged  //viewmodel for mainpage
    {
        string name = string.Empty;
        public string Name {
            get => name;
          set
            {
                if (name == value) //if its already the same thing, no need to change
                {
                    return;
                }
                name = value;
                onPropertyChanged(nameof(Name));
                onPropertyChanged(nameof(DisplayName));

            }
        }

        public string DisplayName => $"Name entered : {name}";

        public event PropertyChangedEventHandler PropertyChanged; //propertychanged handler
        void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); // lets xamarin kno to update the text
        }

    }


}

