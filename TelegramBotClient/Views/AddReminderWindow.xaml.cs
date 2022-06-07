using System.Windows;
using TelegramBotClient.ViewModels;

namespace TelegramBotClient.Views
{
    public partial class AddReminderWindow: Window
    {
        public AddReminderWindow()
        {
            InitializeComponent();
        }
        public AddReminderWindow(ReminderViewModel reminderViewModel)
        {
            InitializeComponent();
            DataContext = reminderViewModel;
        }
    }
}
