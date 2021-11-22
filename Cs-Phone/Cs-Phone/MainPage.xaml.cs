using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Xamarin.Forms;

namespace Cs_Phone
{
    [DesignTimeVisible(true)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            PanGestureRecognizer = new PanGestureRecognizer
            {
                TouchPoints = 1
            };
            PanGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
            
        }

        public PanGestureRecognizer PanGestureRecognizer { get; set; }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {

            StatusLabel.Text = e.StatusType.ToString(); // Canceled, Completed, Running, Started
            GestureIdLabel.Text = e.GestureId.ToString();
            TotalXLabel.Text = Math.Round(e.TotalX, 1).ToString(CultureInfo.InvariantCulture);
            TotalYLabel.Text = Math.Round(e.TotalY, 1).ToString(CultureInfo.InvariantCulture);
        }
    }
}
