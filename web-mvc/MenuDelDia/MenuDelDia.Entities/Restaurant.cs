using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuDelDia.Entities
{
    [Table("Restaurants")]
    public class Restaurant : EntityBase
    {
        public Restaurant()
        {
            Locations = new Collection<Location>();
            Tags = new Collection<Tag>();
            Cards = new Collection<Card>();
            Menus = new Collection<Menu>();
        }

        [Key]
        public override Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LogoPath { get; set; }

        public string Description { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
