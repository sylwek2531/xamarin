using System;
using WeatherAppMain.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherAppMain
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainTabbedPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
