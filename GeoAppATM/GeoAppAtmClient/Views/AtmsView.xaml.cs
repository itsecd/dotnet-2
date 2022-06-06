using GeoAppAtmClient.ViewModels;
using System.Windows;

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
