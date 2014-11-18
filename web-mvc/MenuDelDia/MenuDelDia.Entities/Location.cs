using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuDelDia.Entities
{
    [Table("Locations")]
    public class Location:EntityBase
    {
        public Location()
        {
            OpenDays = new Collection<OpenDay>();
            Tags = new Collection<Tag>();
        }

        [Key]
        public override Guid Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Streets { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public bool Delivery { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        
        public virtual ICollection<OpenDay> OpenDays { get; set; }
        
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
