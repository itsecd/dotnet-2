using System.Windows;
using BotClient.ViewModels;

namespace BotClient.Views
{
    public partial class AddReminderWindow : Window
    {
        public AddReminderWindow()
        {
            InitializeComponent();
            DataContext = new AddReminderViewModel();
        }

        public AddReminderWindow(AddReminderViewModel addReminderViewModel)
        {
            InitializeComponent();
            DataContext = addReminderViewModel;
        }
    }
}
