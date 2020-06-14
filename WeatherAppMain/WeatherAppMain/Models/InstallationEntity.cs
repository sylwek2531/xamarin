using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace WeatherAppMain.Models
{
    public class InstallationEntity
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public double Elevation { get; set; }
        [JsonProperty(PropertyName = "airly")]
        public bool IsAirlyInstallation { get; set; }

        public InstallationEntity()
        {

        }
    }

}
