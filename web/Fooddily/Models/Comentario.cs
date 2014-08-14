using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fooddily.Models
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }
        public int Puntuacion { get; set; }
        public string Descripcion { get; set; }

        public int? ComercioId { get; set; }
        public virtual Comercio Comercio { get; set; }

        public int? MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}