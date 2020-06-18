﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
    public class HomeViewModel : BaseViewModel
    {
        protected readonly INavigation _navigation ;
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }

        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;
             Init();
        }
        private async Task Init()
        {
            Loading = true;

            IEnumerable<Installation> installations = null;
            IEnumerable<Measurement> measurements = null;
           await Task.Run(async() => { 
            
                var location = await GetDeviceLocation();
               //przeniesc bo teraz dw razy odpala sie jeden i ten asam kod zapisu do bazy
              
               if (IsRefreshing == true ? false : checkSourceDataFromDb())
               {
                   installations = DatabaseHelper.getInstallation();
                   measurements = DatabaseHelper.getMeasurements();
               }
               else
               {
                   installations = await GetInstalationByLocation(location);
                   measurements = await GetMeasurementsByIdInstallation(installations);
                   DatabaseHelper.saveInstallation(installations.ToList());
                   DatabaseHelper.saveMeasurement(measurements.ToList());
               }

           });

            Loading = false;

            ItemsList = new List<Measurement>(measurements);

        }
        private bool loading;
        public bool Loading
        {
            get => loading;
            set => SetProperty(ref loading, value);
        }
        private List<Measurement> listMeasurements;
        public List<Measurement> ItemsList
        {
            get => listMeasurements;
            set => SetProperty(ref listMeasurements, value);
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
        private async Task<IEnumerable<Installation>> GetInstalationByLocation(Location location, double maxDistanceKM = 3, int maxResults = -1)
        {
            /*TESTED VALUE*/
          /*  location.Latitude = 50.017942;
            location.Longitude = 20.976090;*/
            try
            {

                string path = App.ApiInstallationUrl;
                Dictionary<string, object> querry = new Dictionary<string, object>
                {
                    {"lat",  location.Latitude},
                    {"lng", location.Longitude},
                    {"maxDistanceKM", maxDistanceKM },
                    {"maxResults", maxResults }

                };
                IEnumerable<Installation> response = await GET<IEnumerable<Installation>>(GeneratePath(path, querry));

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

            string path = App.ApiMeasurementUrl;
            if (installations == null)
            {
                System.Diagnostics.Debug.WriteLine("No installations.");
                return null;
            }
          

            List<Measurement> measurements = new List<Measurement>();

            foreach (Installation instalation in installations)
            {
                Dictionary<string, object> querry = new Dictionary<string, object>
                {
                    { "installationId", instalation.Id }
                };

                Measurement response = await GET<Measurement>(GeneratePath(path, querry));
                if (response != null)
                {
                    response.Installation = instalation;
                    response.CurrentDisplayValue = (int)Math.Round(response.Current?.Indexes?.FirstOrDefault()?.Value ?? 0);
                    measurements.Add(response);
                }
            }

            return measurements;
        }
     

        private async Task<T> GET<T>(string URL)
        {
            try
            {

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("apikey", App.ApiKey);
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
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
            return default;
        }
        private string GeneratePath(string URL, IDictionary<string, object> queryParams)
        {
            var builder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Port = -1,
                Host = App.ApiUrl,
                Path = URL,
            };

            if(queryParams.Count > 0 && queryParams != null)
            {
                NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);

                foreach (KeyValuePair<string, object> item in queryParams)
                {
                    if (item.Value is double number)
                    {
                        query[item.Key] = number.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        query[item.Key] = item.Value?.ToString();
                    }
                }
                builder.Query = query.ToString();
            }

            return builder.ToString();
        }
        private ICommand _goToDetailsCommand;
        public ICommand GoToDetailsCommand => _goToDetailsCommand ?? (_goToDetailsCommand = new Command<Measurement>(OnGoToDetails));

        private void OnGoToDetails(Measurement itemElement)
        {
            _navigation.PushAsync(new DetailsPage(itemElement));
        }
      
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await Init();

                    IsRefreshing = false;
                });
            }
        }



        public static bool checkSourceDataFromDb()
        {
            List<Measurement> measurements = DatabaseHelper.getMeasurements();
            DateTime time = DateTime.Now;
            //musze odjac bo dane tillfatetime sa sprzedwoch godzin? ni emusze oddawac wtedy czas jest dobry tak mi sie ywdaje
            

            if (measurements == null || measurements.Count == 0 || measurements.Any(measurement => (time - measurement.Current.TillDateTime).TotalMinutes >= 60))
            {
                return false;
              
            }
            else
            {
                return true;
            }

         
        }
    }
}
