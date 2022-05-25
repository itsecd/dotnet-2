using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskClient.ViewModel
{
    public class TagsViewModel: INotifyPropertyChanged
    {
        private TaskRepositoryClient _taskRepository;
        public ObservableCollection<TagViewModel> Tags { get; } = new ObservableCollection<TagViewModel>();

        public Tags _selectTag;

        public Tags SelectedTag
        {
            get => _selectTag;
            set
            {
                if (value == _selectTag)
                {
                    return;
                }

                _selectTag = value;
                OnPropertyChanged(nameof(SelectedTag));
            }
        }

        public async System.Threading.Tasks.Task InitializeAsync(TaskRepositoryClient taskRepository)
        {
            _taskRepository = taskRepository;

            var tags = await _taskRepository.GetTagsAsync();
            foreach (var tag in tags)
            {
                var tagViewModel = new TagViewModel();
                await tagViewModel.InitializeAsync(_taskRepository, tag.TagId);
                Tags.Add(tagViewModel);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
