﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuDelDia.Entities
{
    [Table("Locations")]
    public class Location : EntityBase
    {
        public Location()
        {
            OpenDays = new Collection<OpenDay>();
            Tags = new Collection<Tag>();
            Menus = new Collection<Menu>();
        }

        [Key]
        public override Guid Id { get; set; }

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
        public virtual ICollection<OpenDay> OpenDays { get; set; }

        [DisplayName("Tags")]
        public virtual ICollection<Tag> Tags { get; set; }
        
        [DisplayName("Menús")]
        public virtual ICollection<Menu> Menus { get; set; }
    }
}