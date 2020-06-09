using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAppMain.Models
{
    public class MeasurementItem
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public MeasurementValue[] Values { get; set; }
        public AirQualityIndex[] Indexes { get; set; }
        public AirQualityStandard[] Standards { get; set; }
    }
}
