using System;

namespace MenuDelDia.Presentacion.Models.ApiModels
{
    public class SpecialDayApiModel
    {
        public DateTime? Date { get; set; }
        public bool Recurrent { get; set; }
    }
}