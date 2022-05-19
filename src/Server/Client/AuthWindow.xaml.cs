using Client.Services;
using Client.ViewModels;
using Newtonsoft.Json;
using Server.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    public partial class AuthWindow : Window
    {
        private readonly string _address = "https://localhost:44349";

        public AuthWindow()
        {
            InitializeComponent();
        }

        private async Task<User> GetUser(string userName)
        {
            var httpClient = new HttpClient();
            var getResponse = await httpClient.GetAsync($"{_address}/api/User");
            var returnedUsers = JsonConvert.DeserializeObject<List<User>>(await getResponse.Content.ReadAsStringAsync());
            return (from User user in returnedUsers
                    where user.Name == userName
                    select user).Single();
        }

        private async void ButtonContinueClick(object sender, RoutedEventArgs e)
        {
            string userName = Username.Text;
            if (userName == "")
            {
                MessageBox.Show("Name of user not found");
                return;
            }
            string code = "";
            var key = "12341234123412341234123412341234".ToCharArray();
            for (int i = 0; i < userName.Length; i++)
            {
                code += userName[i] ^ key[i];
            }
            code = code[..7];
            if (Code.Text == code)
            {
                var userEventListService = new UserEventListService();
                var mainWindowViewModel = new MainViewModel(userEventListService, await GetUser(userName));
                var mainWindow = new MainWindow { ViewModel = mainWindowViewModel };
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect code");
            }
        }
    }
}
