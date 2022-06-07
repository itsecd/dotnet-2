using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Lab2Server.Models;
using BotClient.Properties;
using System.ComponentModel;
using BotClient.Commands;
using BotClient.Views;

namespace BotClient.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if (value == _user) return;
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        
        public MainViewModel()
        {
            AddCall = new Command(_ =>
            {
                AddReminderWindow addReminderWindow = new AddReminderWindow();
                addReminderWindow.Show();
            }, _=>true);
        }

        public Command AddCall { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}