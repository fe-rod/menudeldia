using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fooddily.Models
{

    [Flags]
    public enum Dia { Domingo = 1, Lunes = 2, Martes = 4, Miercoles = 8, Jueves = 16, Viernes = 32, Sabado = 64 }

    public class Menu
    {
        [Key]
        public int Id { get; set; }

        public virtual Dia? Dias { get; set; }

        public bool Recurrente { get; set; }
        
        public DateTime? Fecha { get; set; }
        
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Foto { get; set; }
        
        public virtual ICollection<Tag> Tags { get; set; }

        public int ComercioId { get; set; }
        public virtual Comercio Comercio { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; }
    }
}