﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAppMain.Models
{
    public class MapLocation
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public Xamarin.Forms.Maps.Position Position { get; set; }

        public MapLocation()
        {

        }
    }
}
