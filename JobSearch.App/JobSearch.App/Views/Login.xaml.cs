using JobSearch.App.Services;
using JobSearch.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSearch.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private UserService _userService;

        public Login()
        {
            InitializeComponent();

            _userService = new UserService();
        }

        private void GoRegister(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Register());
        }

        private async void GoInitial(object sender, EventArgs e)
        {
            string email = TxtEmail.Text;
            string password = TxtPassword.Text;

            User user = await _userService.GetUser(email, password);

            if (user == null)
            {
                await DisplayAlert("Erro","Nenhum usuário encontrado!","OK");
            }
            else
            {              
                App.Current.Properties.Add("User", JsonConvert.SerializeObject(user));
                App.Current.MainPage = new NavigationPage(new Initial());
            }            
        }
    }
}