using Newtonsoft.Json;
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

/*        public MeasurementEntity(Measurement measurement, MeasurementItem current)
*/        public MeasurementEntity(Measurement measurement, MeasurementItemEntity current)
        {
            this.CurrentDisplayValue = measurement.CurrentDisplayValue;
/*            MeasurementItemE
 *            ntity current = new MeasurementItemEntity(measurement.Current);
*/            this.CurrentId = current.Id;
            this.History = JsonConvert.SerializeObject(measurement.History);
            this.Forecast = JsonConvert.SerializeObject(measurement.Forecast);
            this.InstallationId = measurement.Installation.Id;

        }
        public MeasurementEntity()
        {

        }

    }
}
