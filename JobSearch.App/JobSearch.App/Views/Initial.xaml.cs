using JobSearch.App.Models;
using JobSearch.App.Resources.Load;
using JobSearch.App.Services;
using JobSearch.Domain.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSearch.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Initial : ContentPage
    {
        private JobService _jobService;
        private ObservableCollection<Job> _jobsList;
        private SearchParams _searchParams;

        public Initial()
        {
            InitializeComponent();
            _jobService = new JobService();
        }

        private void GoVisualizer(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Visualizer());
        }

        private void GoRegisterJob(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterJob());
        }

        private void FocusSearch(object sender, EventArgs e)
        {
            TxtWord.Focus();
        }

        private void FocusCityState(object sender, EventArgs e)
        {
            TxtCityState.Focus();
        }

        private void Logout(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("User");
            App.Current.SavePropertiesAsync();
            App.Current.MainPage = new Login();
        }

        private async void Search(object sender, EventArgs e)
        {
            _searchParams = new SearchParams() { Word = TxtWord.Text, CityState = TxtCityState.Text, PageNumber = 1 };

            ResponseService<List<Job>> responseService = await _jobService.GetJobs(_searchParams.Word, _searchParams.CityState, _searchParams.PageNumber);

            if (responseService.IsSuccess)
            {
                _jobsList = new ObservableCollection<Job>(responseService.Data);
                JobsList.ItemsSource = _jobsList;
                JobsList.RemainingItemsThreshold = 1;
            }
            else
            {
                await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
            }
        }

        private async void InfinityScroll(object sender, EventArgs e)
        {
            _searchParams.PageNumber++;

            ResponseService<List<Job>> responseService = await _jobService.GetJobs(_searchParams.Word, _searchParams.CityState, _searchParams.PageNumber);

            if (responseService.IsSuccess)
            {
                var jobs = responseService.Data;
                foreach (var item in jobs)
                {
                    _jobsList.Add(item);
                }
            }
            else
            {
                await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
            }
        }
    }
}