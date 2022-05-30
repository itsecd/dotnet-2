using Grpc.Net.Client;
using OrderAccountingSystemClient.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace OrderAccountingSystemClient
{
    public class AddOrderWindowBase : ReactiveWindow<AddOrderViewModel>
    {
    }

    public partial class AddOrderWindow : AddOrderWindowBase
    {
        public AddOrderWindow()
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
