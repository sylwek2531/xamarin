using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAppMain.Models
{
    public class Address
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("displayAddress1")]
        public string DisplayAddress1 { get; set; }

        [JsonProperty("displayAddress2")]
        public string DisplayAddress2 { get; set; }

        public string Description => $"{Street} {Number}, {City}";
    }
}
