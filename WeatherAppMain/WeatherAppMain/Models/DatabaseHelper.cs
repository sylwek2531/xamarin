using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WeatherAppMain.Models
{
    public class DatabaseHelper
    {

        /*Katalog, który służy jako wspólne repozytorium dla dokumentów.Ten element członkowski jest równoważny MyDocuments.*/
        public DatabaseHelper()
        {

        }
        public static SQLiteConnection initDb()
        {

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "airquality.db");
            var db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);

            db.CreateTable<InstallationEntity>();
            db.CreateTable<MeasurementEntity>();
            db.CreateTable<MeasurementItemEntity>();
            db.CreateTable<MeasurementValue>();
            db.CreateTable<AirQualityIndex>();
            db.CreateTable<AirQualityStandard>();


            return db;
        }

    }
}
