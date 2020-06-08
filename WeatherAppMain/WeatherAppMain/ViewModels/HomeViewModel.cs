using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherAppMain.ViewModels
{
    class HomeViewModel
    {
        protected readonly INavigation _navigation ; 
        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;
             Init();
        }
        private async Task Init()
        {
            var location = await GetDeviceLocation();
            var http = GetHttpClient();
           
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Console.WriteLine(http);
        }

        private async Task<Location> GetDeviceLocation()
        {
            try
            {
                Location location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                System.Diagnostics.Debug.WriteLine(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                System.Diagnostics.Debug.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                System.Diagnostics.Debug.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return null;
        }
        private static HttpClient GetHttpClient()
        {
            var http = new HttpClient();
            http.BaseAddress = new Uri("https://airapi.airly.eu/v2/");
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("apikey", "kKd8vd7FZyrcEfuULfVOfnDaJAJHUEw1");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            return http;

        }
        private ICommand _goToDetailsCommand;
        public ICommand GoToDetailsCommand => _goToDetailsCommand ?? (_goToDetailsCommand = new Command(OnGoToDetails));

        private void OnGoToDetails()
        {
            _navigation.PushAsync(new DetailsPage());
        }
    }
}
