using OrderAccountingSystemClient.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace OrderAccountingSystemClient
{
    public class AddProductWindowBase : ReactiveWindow<AddProductViewModel>
    {
    }

    public partial class AddProductWindow : AddProductWindowBase
    {
        public AddProductWindow()
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
