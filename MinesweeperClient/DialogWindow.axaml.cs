using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperClient
{
    public partial class DialogWindow : Window
    {
        TextBox nickField;
        TextBox servField;
        public DialogWindow()
        {
            InitializeComponent();
            nickField = this.FindControl<TextBox>("nickBox");
            servField = this.FindControl<TextBox>("servBox");
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