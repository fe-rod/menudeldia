﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuDelDia.Presentacion.Models.ApiModels.site
{
    public class LocationApiModel
    {
        public LocationApiModel()
        {
            Days = new List<DaysApiModel>();
        }

        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Identifier { get; set; }

        public string Zone { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Features { get; set; }

        public bool Delivery { get; set; }

        public LatLong Location { get; set; }

        public IList<DaysApiModel> Days { get; set; }
    }

    public class LatLong
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DaysApiModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Open { get; set; }
    }
}
