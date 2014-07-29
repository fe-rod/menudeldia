using System.Collections.Generic;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Tarjeta;
using Fooddily.ViewModels.Zona;

namespace Fooddily.ViewModels.Comercio
{
    public class ComercioViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Horario { get; set; }
        public string Url { get; set; }
        public bool Delivery { get; set; }

        public int Puntuacion { get; set; }

        public int ZonaId { get; set; }
        public virtual ZonaViewModel Zona { get; set; }

        public virtual ICollection<MenuViewModel> Menus { get; set; }

        public virtual ICollection<ComentarioViewModel> Comentarios { get; set; }

        public virtual ICollection<TarjetaViewModel> Tarjetas { get; set; }
    }
}