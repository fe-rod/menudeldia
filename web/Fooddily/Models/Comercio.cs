using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fooddily.Models
{
    public class Comercio
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Horario { get; set; }
        public string Url { get; set; }
        public bool Delivery { get; set; }

        public int ZonaId { get; set; }
        public virtual Zona Zona { get; set; }

        public virtual ICollection<Tarjeta> Tarjetas { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; }
    }
}