using GeoAppAtmClient.ViewModels;
using System.Windows;

namespace GeoAppAtmClient.Views
{
    public partial class AtmView : Window
    {
        public AtmView()
        {
            InitializeComponent();
        }

        public AtmView(AtmViewModel atmViewModel)
        {
            InitializeComponent();
            DataContext = atmViewModel;
        }
    }
}
