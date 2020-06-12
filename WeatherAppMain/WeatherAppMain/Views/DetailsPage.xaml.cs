using System;
using System.ComponentModel;
using WeatherAppMain.Models;
using WeatherAppMain.ViewModels;
using Xamarin.Forms;

namespace WeatherAppMain
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Measurement itemElement)
        {
            InitializeComponent();

            var detailVM = BindingContext as DetailsViewModel;
            detailVM.Item = itemElement;
        }
        private void Help_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Co to jest CAQI?", "Lorem ipsum.", "Zamknij");
        }
    }
}
