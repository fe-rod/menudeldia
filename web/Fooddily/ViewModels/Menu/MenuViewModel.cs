using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Fooddily.Models;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Comercio;
using Fooddily.ViewModels.Tag;

namespace Fooddily.ViewModels.Menu
{
    public class MenuViewModel
    {
        public int Id { get; set; }

        public ICollection<Dia> Dias { get; set; }

        public bool Recurrente { get; set; }

        public DateTime? Fecha { get; set; }

        public string FechaString { get; set; }
        
        [Required(ErrorMessage = "Por favor ingrese un nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor ingrese una descripcion")]
        public string Descripcion { get; set; }

        public int ComercioId { get; set; }
        public virtual ComercioViewModel Comercio { get; set; }

        public virtual ICollection<TagViewModel> Tags { get; set; }

        public int Puntuacion { get; set; }

        public virtual ICollection<ComentarioViewModel> Comentarios { get; set; }
    }
}