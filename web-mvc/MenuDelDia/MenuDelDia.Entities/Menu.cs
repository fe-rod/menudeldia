using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuDelDia.Entities
{
    [Table("Menus")]
    public class Menu:EntityBase
    {
        public Menu()
        {
            Comments = new Collection<Comment>();
            Tags = new Collection<Tag>();
        }

        [Key]
        public override Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public string Ingredients { get; set; }

        public MenuDays MenuDays { get; set; }
        public SpecialDay SpecialDay { get; set; }






        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }



        public Guid RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
    }

    [ComplexType]
    public class MenuDays
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }

    [ComplexType]
    public class SpecialDay
    {
        public DateTime Date { get; set; }
        public bool Recurrent { get; set; }
    }

}
