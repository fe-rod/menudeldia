using System;
using System.Collections.Generic;
using System.Linq;
using Fooddily.Models;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Comercio;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Tarjeta;

namespace Fooddily.Mappers
{
    public static class ComercioMapper
    {
        public static ComercioViewModel ToViewModel(this Comercio comercio)
        {
            return new ComercioViewModel
            {
                Id = comercio.Id,
                Nombre = comercio.Nombre,
                Direccion = comercio.Direccion,
                Zona = comercio.Zona.ToViewModel(),
                Telefono = comercio.Telefono,
                Horario = comercio.Horario,
                Url = comercio.Url,
                Delivery = comercio.Delivery,
                Puntuacion = comercio.Comentarios != null && comercio.Comentarios.Any() ? (Int32)comercio.Comentarios.Average(c => c.Puntuacion) : 0,
                Menus = comercio.Menus != null ? comercio.Menus.Select(m => m.ToComercioViewModel()).ToList() : new List<MenuViewModel>(),
                Comentarios = comercio.Comentarios != null ? comercio.Comentarios.Select(m => m.ToViewModel()).ToList() : new List<ComentarioViewModel>(),
                Tarjetas = comercio.Tarjetas != null ? comercio.Tarjetas.Select(t => t.ToViewModel()).ToList() : new List<TarjetaViewModel>(),
            };
        }

        public static Comercio ToModel(this ComercioViewModel comercio)
        {
            return new Comercio
            {
                Id = comercio.Id,
                Nombre = comercio.Nombre,
                Direccion = comercio.Direccion,
                Zona = comercio.Zona.ToModel(),
                Telefono = comercio.Telefono,
                Horario = comercio.Horario,
                Url = comercio.Url,
                Delivery = comercio.Delivery,
            };
        }		
	}
}

