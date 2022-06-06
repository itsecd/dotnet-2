using ReactiveUI;
using System.Reactive;
using System.Windows;
using TaskListKhvatskovaClient.ViewModels;

namespace TaskListKhvatskovaClient
{
    public class AddTaskBase : ReactiveWindow<AddTaskViewModel> { }
    public partial class AddTask : AddTaskBase
    {
        public AddTask()
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
