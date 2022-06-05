using System.Windows;
using GeoAppAtmClient.ViewModels;

namespace GeoAppAtmClient.Views
{
    public partial class AtmsView : Window
    {
        public AtmsView()
        {
            InitializeComponent();
        }
        public AtmsView(AtmsViewModel atmsViewModel)
        {
            InitializeComponent();
            DataContext = atmsViewModel;
        }
    }
}
