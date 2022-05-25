using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskClient.ViewModel
{
    public class TagViewModel : INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        private Tags _tag;

        public TagViewModel()
        {
            _tag = new Tags();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get => _tag.TagId; }

        public string Name
        {
            get => _tag.Name;
            set
            {
                if (value == _tag.Name)
                {
                    return;
                }

                _tag.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Color
        {
            get => _tag.Color;
            set
            {
                if (value == _tag.Color)
                {
                    return;
                }

                _tag.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository, int tagId)
        {
            _taskRepository = taskRepository;

            var tags = await _taskRepository.GetTagsAsync();
            _tag = tags.FirstOrDefault(tag => tag.TagId == tagId);
        }
    }
}
