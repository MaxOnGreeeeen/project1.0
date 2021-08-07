﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models.MapViewModel
{
    public class ShowViewModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public double GeoLong { get; set; } // долгота - для карт google
        public double GeoLat { get; set; }
    }
}
