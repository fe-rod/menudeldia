using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MenuDelDia.Entities;

namespace MenuDelDia.Presentacion.Models
{

    public class LocationModel
    {
        public LocationModel()
        {
            OpenDays = new Collection<OpenDaysModel>();
            Tags = new Collection<Tag>();
            Menus = new Collection<Menu>();
        }

        
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Dirección")]
        public string Streets { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Teléfono")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Delivery")]
        public bool Delivery { get; set; }

        [DisplayName("Latitud")]
        public double Latitude { get; set; }

        [DisplayName("Longitud")]
        public double Longitude { get; set; }

        public Guid? RestaurantId { get; set; }

        [DisplayName("Días abiertos")]
        public virtual IList<OpenDaysModel> OpenDays { get; set; }

        [DisplayName("Tags")]
        public virtual ICollection<Tag> Tags { get; set; }

        [DisplayName("Menús")]
        public virtual ICollection<Menu> Menus { get; set; }
    }
    
    public class OpenDaysModel
    {
        public Guid Id { get; set; }

        [DisplayName("Abierto")]
        public bool Open { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        [DisplayName("Hora de apertura")]
        [Range(0,12)]
        public int OpenHour { get; set; }
        [Range(0, 59)]
        public int OpenMinutes { get; set; }

        [DisplayName("Hora de cierre")]
        [Range(0, 12)]
        public int CloseHour { get; set; }
        [Range(0, 59)]
        public int CloseMinutes { get; set; }
    }
}
