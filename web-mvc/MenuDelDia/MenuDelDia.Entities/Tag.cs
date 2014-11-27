using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MenuDelDia.Entities.Enums;

namespace MenuDelDia.Entities
{
    [Table("Tags")]
    public class Tag : EntityBase
    {
        public Tag()
        {
            Locations = new Collection<Location>();
            Restaurants = new Collection<Restaurant>();
            Menus = new Collection<Menu>();
        }

        [Key]
        public override Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public bool ApplyToRestaurant { get; set; }
        public bool ApplyToLocation { get; set; }
        public bool ApplyToMenu { get; set; }
        
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
