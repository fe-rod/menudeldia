using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fooddily.Models
{
    public class Zona
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<Comercio> Comercios { get; set; }
    }
}