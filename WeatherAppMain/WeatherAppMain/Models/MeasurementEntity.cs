using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAppMain.Models
{
    public class MeasurementEntity
    {
       
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CurrentDisplayValue { get; set; }
        public int CurrentId { get; set; }
        public string History { get; set; }
        public string Forecast { get; set; }
        public int InstallationId { get; set; }

        MeasurementEntity()
        {

        }

    }
}
