using ReactiveUI;
using System.Reactive;
using TaskListKhvatskovaClient.ViewModels;

namespace TaskListKhvatskovaClient
{
    public class AddTagBase : ReactiveWindow<AddTagViewModel> { }
    public partial class AddTag : AddTagBase
    {
        public AddTag()
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
