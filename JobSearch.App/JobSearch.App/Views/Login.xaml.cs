﻿using JobSearch.App.Models;
using JobSearch.App.Services;
using JobSearch.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JobSearch.App.Resources.Load;

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

            await Navigation.PushPopupAsync(new Loading());

            ResponseService<User> responseService = await _userService.GetUser(email, password);

            if (responseService.IsSuccess)
            {
                App.Current.Properties.Add("User", JsonConvert.SerializeObject(responseService.Data));
                await App.Current.SavePropertiesAsync();
                App.Current.MainPage = new NavigationPage(new Initial());
            }
            else
            {                              
                if (responseService.StatusCode == 404)
                {
                    await DisplayAlert("Erro", "Usuário ou senha incorretos!", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
                }
            }
            await Navigation.PopAllPopupAsync();
        }
    }
}