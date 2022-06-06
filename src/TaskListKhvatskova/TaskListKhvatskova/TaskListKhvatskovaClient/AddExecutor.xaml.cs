using ReactiveUI;
using System.Reactive;
using TaskListKhvatskovaClient.ViewModels;

namespace TaskListKhvatskovaClient
{

    public class AddExecutorBase : ReactiveWindow<AddExecutorViewModel> { }
    public partial class AddExecutor : AddExecutorBase
    {
        public AddExecutor()
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
