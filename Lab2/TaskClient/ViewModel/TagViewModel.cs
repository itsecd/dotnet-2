using System.ComponentModel;
using System.Windows;
using TaskClient.Commands;

namespace TaskClient.ViewModel
{
    public class TagViewModel : INotifyPropertyChanged
    {
        private Tags _tag;
        public int Num { get; set; }
        public string NameTag
        {
            get => _tag?.Name;
            set
            {
                if (value == _tag.Name)
                {
                    return;
                }

                _tag.Name = value;
                OnPropertyChanged(nameof(NameTag));
            }
        }
        public TagViewModel()
        {
            _tag = new Tags()
            {
                Name = string.Empty,
            };
            OkTagCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.DialogResult = true;
                window.Close();
            }, null);
            CancelTagCommand = new Command(commandParameter =>
            {
                var window = (Window)commandParameter;
                window.DialogResult = false;
                window.Close();
            }, null);
        }
        public void Initialize(Tags tag)
        {
            _tag = tag;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public Command OkTagCommand { get; }
        public Command CancelTagCommand { get; }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
