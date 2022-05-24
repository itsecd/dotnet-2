using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Newtonsoft.Json;
using Server.Model;
using Client.Properties;
using System.Windows;

namespace Client.ViewModels
{
    public sealed class AuthViewModel : ReactiveObject
    {
        private readonly string _serverAddress = app.Default.serverAddress;
        private readonly char[] _keyForXor = "12341234123412341234123412341234".ToCharArray();
        public AuthWindowBase AuthWindow;

        [Reactive]
        public string UserName { get; set; } = string.Empty;

        [Reactive]
        public string Code { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> Continue { get; }

        private async Task ContinueImpl()
        {
            if (UserName == "")
            {
                MessageBox.Show("Name of user not found");
                return;
            }
            string code = "";
            for (int i = 0; i < UserName.Length; i++)
            {
                code += UserName[i] ^ _keyForXor[i];
            }
            code = code[..7];
            if (Code == code)
            {
                AuthWindow.Hide();
                _ = await OpenMainWindow.Handle(await GetUser(UserName));
                AuthWindow.Close();
                App.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Incorrect code");
            }
        }

        public Interaction<User, Unit> OpenMainWindow { get; } = new();

        private async Task<User> GetUser(string userName)
        {
            var httpClient = new HttpClient();
            var getResponse = await httpClient.GetAsync($"{_serverAddress}/api/User");
            var returnedUsers = JsonConvert.DeserializeObject<List<User>>(await getResponse.Content.ReadAsStringAsync());
            return (from User user in returnedUsers
                    where user.Name == userName
                    select user).Single();
        }

        public AuthViewModel(AuthWindowBase authWindowBase)
        {
            AuthWindow = authWindowBase;
            {
                var canExecute = new Subject<bool>();
                var isEnteredUsername = this.WhenAnyValue(o => o.UserName, (string? o) => !string.IsNullOrWhiteSpace(o));
                var isEnteredCode = this.WhenAnyValue(o => o.Code, (string? o) => !string.IsNullOrWhiteSpace(o));
                var canExecuteAndIsEntered = canExecute.CombineLatest(isEnteredUsername, isEnteredCode,
                    (canExecute, isEnteredUsername, isEnteredCode) => canExecute && isEnteredUsername && isEnteredCode);
                Continue = ReactiveCommand.CreateFromTask(() => ExclusiveWrapper(ContinueImpl), canExecuteAndIsEntered);

                async Task ExclusiveWrapper(Func<Task> impl)
                {
                    try
                    {
                        canExecute.OnNext(false);
                        await impl();
                    }
                    finally
                    {
                        canExecute.OnNext(true);
                    }
                }

                canExecute.OnNext(true);
            }
        }
    }

}

