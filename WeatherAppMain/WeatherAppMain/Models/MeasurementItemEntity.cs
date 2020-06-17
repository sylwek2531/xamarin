using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAppMain.Models
{
    public class MeasurementItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public string Values { get; set; }
        public string Indexes { get; set; }
        public string Standards { get; set; }

      /*  public MeasurementItemEntity(Measurement measurementEntity)
        {
            this.FromDateTime = measurementEntity.Current.FromDateTime;
            this.TillDateTime = measurementEntity.Current.TillDateTime;
            this.Values = JsonConvert.SerializeObject(measurementEntity.Current.Values);
            this.Indexes = JsonConvert.SerializeObject(measurementEntity.Current.Indexes);
            this.Standards = JsonConvert.SerializeObject(measurementEntity.Current.Standards);
        }*/
        public MeasurementItemEntity(MeasurementItem measurementEntity)
        {
            this.FromDateTime = measurementEntity.FromDateTime;
            this.TillDateTime = measurementEntity.TillDateTime;
            this.Values = JsonConvert.SerializeObject(measurementEntity.Values);
            this.Indexes = JsonConvert.SerializeObject(measurementEntity.Indexes);
            this.Standards = JsonConvert.SerializeObject(measurementEntity.Standards);
        }
        public MeasurementItemEntity()
        {

        }
    }
}
