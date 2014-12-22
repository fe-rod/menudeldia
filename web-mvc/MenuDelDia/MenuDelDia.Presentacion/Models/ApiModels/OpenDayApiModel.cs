using System;

namespace MenuDelDia.Presentacion.Models.ApiModels
{
    public class OpenDayApiModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int OpenHour { get; set; }
        public int OpenMinutes { get; set; }
        public int CloseHour { get; set; }
        public int CloseMinutes { get; set; }
    }
}