using System;
using System.Collections.Generic;

namespace MenuDelDia.Presentacion.Models.ApiModels
{
    public class LocationApiModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public bool Delivery { get; set; }
        public string Phone { get; set; }
        public string Streets { get; set; }
        public IList<OpenDayApiModel> OpenDays { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Distance { get; set; }
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }
}