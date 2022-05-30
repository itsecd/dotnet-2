using System.Windows;
using TaskClient.ViewModel;

namespace TaskClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TagView.xaml
    /// </summary>
    public partial class TagView : Window
    {
        public TagView()
        {
            InitializeComponent();
        }
        public TagView(TagViewModel tagViewModel)
        {
            InitializeComponent();
            DataContext = tagViewModel;
        }
    }
}
