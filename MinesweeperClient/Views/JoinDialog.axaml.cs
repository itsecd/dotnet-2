using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperClient.Views
{
    public partial class JoinDialog : Window
    {
        private readonly TextBox _nickField;
        private readonly TextBox _servField;
        public JoinDialog()
        {
            InitializeComponent();
            _nickField = this.FindControl<TextBox>("NickTextBox");
            _servField = this.FindControl<TextBox>("ServTextBox");
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
            Close(new string[] { _nickField.Text, _servField.Text });
        }
    }
}