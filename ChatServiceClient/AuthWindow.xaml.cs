using ChatServiceClient.ViewModel;
using ReactiveUI;
using System.Reactive;

namespace ChatServiceClient
{
    public class AuthWindowBase : ReactiveWindow<AuthWindowViewModel>
    {
    }

    public partial class AuthWindow : AuthWindowBase
    {
        public AuthWindow()
        {
            InitializeComponent();

            _ = this.WhenActivated(cd =>
            {
                if (ViewModel is null)
                    return;

                cd.Add(ViewModel.Close.RegisterHandler(interaction =>
                {
                    Tag = interaction.Input;
                    interaction.SetOutput(Unit.Default);
                    Close();
                }));
            });
        }
    }
}
