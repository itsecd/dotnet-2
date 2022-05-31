using Lab2TaskClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskClientWPF.ViewModels
{
    public class InputViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public string InputExecutorName { get; set; }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
