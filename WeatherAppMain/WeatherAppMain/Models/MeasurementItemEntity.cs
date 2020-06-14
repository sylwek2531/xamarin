using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAppMain.Models
{
    public class MeasurementItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public string Values { get; set; }
        public string Indexes { get; set; }
        public string Standards { get; set; }

        public MeasurementItemEntity()
        {

        }
    }
}
