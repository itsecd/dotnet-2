using OrderAccountingSystemClient.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace OrderAccountingSystemClient
{
    public class AddCustomerWindowBase : ReactiveWindow<AddCustomerViewModel>
    {
    }

    public partial class AddCustomerWindow : AddCustomerWindowBase
    {
        public AddCustomerWindow()
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
