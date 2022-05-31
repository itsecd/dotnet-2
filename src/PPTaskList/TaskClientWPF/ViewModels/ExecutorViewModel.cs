using System.ComponentModel;
using Lab2TaskClient;

namespace TaskClientWPF.ViewModels
{
    public class ExecutorViewModel : INotifyPropertyChanged
    {
        public Executor Executor { get; set; }

        public ExecutorViewModel()
        {
            Executor = new Executor();
        }

        public ExecutorViewModel(Executor executor)
        {
            Executor = executor;
        }


        public string ExecutorName
        {
            get => Executor.Name;
            set
            {
                if (value == Executor.Name) return;
                Executor.Name = value;
                OnPropertyChanged(nameof(ExecutorName));
            }
        }
        public int ExecutorId
        {
            get => Executor.ExecutorId;
            set
            {
                if (value == Executor.ExecutorId) return;
                Executor.ExecutorId = value;
                OnPropertyChanged(nameof(ExecutorId));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
