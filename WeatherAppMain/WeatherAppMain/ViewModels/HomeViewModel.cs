using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using WeatherAppMain.Models;
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
            var test = GeneratePath("installations/nearest");
            Console.WriteLine(test);
           /* var location = await GetDeviceLocation();
           
            var installations = await GetInstalationByLocation(location);

            var measurements = await GetMeasurementsByIdInstallation(installations);

            Console.WriteLine(measurements);*/
        }

        private async Task<Location> GetDeviceLocation()
        {
            try
            {
                Location location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    location = await Geolocation.GetLocationAsync(request);
                }

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
        private async Task<IEnumerable<Installation>> GetInstalationByLocation(Location location, double maxDistanceKM = 3, int maxResults = 1)
        {
            /*TESTED VALUE*/
            location.Latitude = 50.017942;
            location.Longitude = 20.976090;
            try
            {

                string path = "installations/nearest";
                var response = await GET<IEnumerable<Installation>>(GetURI(path) + "/?lat=" + location.Latitude + "&lng=" + location.Longitude + "&maxDistanceKM=" + maxDistanceKM + "&maxResults=" + maxResults);

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return default;

        }
        private async Task<IEnumerable<Measurement>> GetMeasurementsByIdInstallation(IEnumerable<Installation> installations)
        {
            string path = "measurements/installation";

            if(installations.Count() == 0 || installations == null)
            {
                System.Diagnostics.Debug.WriteLine("No installations.");
                return null;
            }
            var measurements = new List<Measurement>();

            foreach (var instalation in installations)
            {
                var response = await GET<Measurement>(GetURI(path) + "/?installationId=" + instalation.Id);
                if (response != null)
                {
                    response.Installation = instalation;
                    response.CurrentDisplayValue = (int)Math.Round(response.Current?.Indexes?.FirstOrDefault()?.Value ?? 0);
                    measurements.Add(response);
                }
            }

            return measurements;
        }
     
        protected string GetURI(string path)
        {
            return "https://airapi.airly.eu/v2/" + path;
        }
        private async Task<T> GET<T>(string URL)
        {
            try
            {

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("apikey", "kKd8vd7FZyrcEfuULfVOfnDaJAJHUEw1");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
                var response = await client.GetAsync(URL);


                if (response.Headers.TryGetValues("X-RateLimit-Limit-day", out var limitPerDay) &&
                 response.Headers.TryGetValues("X-RateLimit-Remaining-day", out var limitRemainingPerDay))
                {
                    System.Diagnostics.Debug.WriteLine($"Limit per day: {limitPerDay?.FirstOrDefault()}, remaining: {limitRemainingPerDay?.FirstOrDefault()}");
                }


                switch ((int)response.StatusCode)
                {
                    case 200:
                        string content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(content);
                        return result;
                    case 429:
                        System.Diagnostics.Debug.WriteLine("Too many requests, error: 429");
                        break;
                    default:
                        var errorMsg = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"Error: {errorMsg}");
                        return default;
                }

            }
            catch (Exception ex)
            {

            }
            return default;
        }
        private string GeneratePath(string URL)
        {
            var builder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Port = -1,
                Host = "airapi.airly.eu/v2",
                Path = URL,
            };

            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
            query["action"] = "login1";
            query["attempts"] = "11";
            builder.Query = query.ToString();
//DOKONCZYC TWORZENIE URLA            
            return builder.ToString();
        }
        private ICommand _goToDetailsCommand;
        public ICommand GoToDetailsCommand => _goToDetailsCommand ?? (_goToDetailsCommand = new Command(OnGoToDetails));

        private void OnGoToDetails()
        {
            _navigation.PushAsync(new DetailsPage());
        }
    }
}
