using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAppMain.Models;
using WeatherAppMain.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherAppMain.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
         private HomeViewModel viewModel => BindingContext as HomeViewModel;

        public HomePage()
        {
            InitializeComponent();

            BindingContext = new HomeViewModel(Navigation);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            viewModel.GoToDetailsCommand.Execute(e.Item as Measurement);

        }
       
    }
}