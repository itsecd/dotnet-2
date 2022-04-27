using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperClient
{
    public partial class DialogWindow : Window
    {
        TextBox nickname;
        TextBox servAddress;
        public DialogWindow()
        {
            InitializeComponent();
            nickname = this.FindControl<TextBox>("nickTextBox");
            servAddress = this.FindControl<TextBox>("serverTextBox");
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
            Console.WriteLine(nickname.Text);
            Console.WriteLine(servAddress.Text);
            Close(new string[] {nickname.Text, servAddress.Text});
        }
    }
}