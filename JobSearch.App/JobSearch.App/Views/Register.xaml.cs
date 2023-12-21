﻿using JobSearch.App.Models;
using JobSearch.App.Resources.Load;
using JobSearch.App.Services;
using JobSearch.Domain.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSearch.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private UserService _userService;

        public Register()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void SaveAction(object sender, EventArgs e)
        {
            TxtMessages.Text = String.Empty;

            string name = TxtName.Text;
            string email = TxtEmail.Text;
            string password = TxtPassword.Text;

            User user = new User() { Name = name, Email = email, Password = password};

            await Navigation.PushPopupAsync(new Loading());

            ResponseService<User> responseService = await _userService.AddUser(user);

            if (responseService.IsSuccess)
            {
                App.Current.Properties.Add("User", JsonConvert.SerializeObject(responseService.Data));
                await App.Current.SavePropertiesAsync();
                App.Current.MainPage = new NavigationPage(new Initial());
            }
            else
            {
                if (responseService.StatusCode == 400)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var itemKey in responseService.Errors)
                    {
                        foreach (var message in itemKey.Value)
                        {
                            stringBuilder.AppendLine(message);
                        }
                    }
                    TxtMessages.Text = stringBuilder.ToString();
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