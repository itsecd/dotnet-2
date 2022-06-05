using ChatClient.Command;
using ChatClient.Model;
using ChatClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private IChatService _chatService;

        public ObservableCollection<UserModel> Users { get; set; } = new();


        private UserModel? _selectedUser;
        public UserModel? SelectedUser
        {
            get
            {

                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        private string? _message;

        public string? _loginUser;
        public string? Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        #region Send Text Message Command
        private ICommand? _sendTextMessageCommand;
        public ICommand SendTextMessageCommand
        {
            get
            {
                return _sendTextMessageCommand ?? (_sendTextMessageCommand =
                    new RelayCommandAsync(SendTextMessage));
            }
        }
        private async Task<bool> SendTextMessage()
        {
            try
            {
                if (_selectedUser == null)
                    throw new InvalidOperationException();
                var receiver = _selectedUser.Username;
                await _chatService.SendMessage(receiver!, Message!);
                var newMessage = new MessageModel(_loginUser!, Message!);
                _selectedUser.Messages!.Add(newMessage);
                Message = string.Empty;
                return true;
            }
            catch (Exception) { return false; }
        }
        #endregion

        public MainViewModel(IChatService chatService)
        {
            _chatService = chatService;
            chatService.UserJoined.Subscribe(login =>
            {
                Users.Add(new UserModel
                {
                    Username = login
                });
            });
            chatService.MessageReceived.Subscribe(message =>
            {
                foreach (var user in Users)
                {
                    if (user.Username == message.Sender)
                        user.Messages!.Add(message);
                }
            });
        }
    }
}

