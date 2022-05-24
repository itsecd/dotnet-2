using System.Reactive;
using ReactiveUI;
using Client.ViewModels;

namespace Client
{
    public class EventWindowBase : ReactiveWindow<EventViewModel>
    {
    }
    public partial class EventWindow : EventWindowBase
    {
        public EventWindow()
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
