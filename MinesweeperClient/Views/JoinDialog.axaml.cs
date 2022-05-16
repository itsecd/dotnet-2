using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperClient.Views
{
    public partial class JoinDialog : Window
    {
        TextBox nickField;
        TextBox servField;
        public JoinDialog()
        {
            InitializeComponent();
            nickField = this.FindControl<TextBox>("nick_textbox");
            servField = this.FindControl<TextBox>("serv_textbox");
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void OnJoinClick(object sender, RoutedEventArgs e)
        {
            Close(new string[] { nickField.Text, servField.Text });
        }
    }
}