using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Fooddily.Models;
using Fooddily.ViewModels.Comercio;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Tag;

namespace Fooddily.ViewModels.Comentario
{
    public class ComentarioViewModel
    {
        public int Id { get; set; }

        public string Fecha { get; set; }
        public int Puntuacion { get; set; }
        [Required(ErrorMessage = "Por favor ingrese una descripcion")]
        public string Descripcion { get; set; }

        public int? ComercioId { get; set; }
        public virtual ComercioViewModel Comercio { get; set; }

        public int? MenuId { get; set; }
        public virtual MenuViewModel Menu { get; set; }
    }
}