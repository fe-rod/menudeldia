using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fooddily.Models;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Tag;

namespace Fooddily.Mappers
{
    public static class MenuMapper
    {
        public static MenuViewModel ToViewModel(this Menu menu)
        {
            return new MenuViewModel
            {
                Id = menu.Id,
                Dias = Enum.GetValues(typeof(Dia)).Cast<Enum>().Where(value => menu.Dias != null && ((Dia)menu.Dias).HasFlag(value)).Cast<Dia>().ToList(),
                Recurrente = menu.Recurrente,
                Fecha = menu.Fecha,
                Nombre = menu.Nombre,
                Descripcion = menu.Descripcion,
                ComercioId = menu.ComercioId,
                Comercio = menu.Comercio.ToViewModel(),
                Tags = menu.Tags != null ? menu.Tags.Select(m => m.ToViewModel()).ToList() : new List<TagViewModel>(),
                Comentarios = menu.Comentarios != null ? menu.Comentarios.Select(m => m.ToViewModel()).ToList() : new List<ComentarioViewModel>(),
                Puntuacion = menu.Comentarios != null && menu.Comentarios.Any() ? (Int32)menu.Comentarios.Average(c => c.Puntuacion) : 0,
            };
        }

        public static MenuViewModel ToComercioViewModel(this Menu menu)
        {
            var m = new MenuViewModel
            {
                Id = menu.Id,
                Dias = Enum.GetValues(typeof(Dia)).Cast<Enum>().Where(value => menu.Dias != null && ((Dia)menu.Dias).HasFlag(value)).Cast<Dia>().ToList(),
                Recurrente = menu.Recurrente,
                Fecha = menu.Fecha,
                Nombre = menu.Nombre,
                Descripcion = menu.Descripcion,
                ComercioId = menu.ComercioId,
                Tags = menu.Tags != null ? menu.Tags.Select(t => t.ToViewModel()).ToList() : new List<TagViewModel>(),
                Comentarios = menu.Comentarios != null ? menu.Comentarios.Select(c => c.ToViewModel()).ToList() : new List<ComentarioViewModel>(),
                Puntuacion = menu.Comentarios != null && menu.Comentarios.Any() ? (Int32)menu.Comentarios.Average(c => c.Puntuacion) : 0,
            };

            m.FechaString = "";
            if (m.Recurrente)
            {
                foreach(var dia in m.Dias)
                {
                    m.FechaString += dia.ToString().ElementAt(0) + " ";
                }
            }else
            {
                if (m.Fecha.HasValue) m.FechaString = m.Fecha.Value.ToShortDateString();
            }

            return m;
        }

        public static Menu ToModel(this MenuViewModel menu)
        {
            var m = new Menu
            {
                Id = menu.Id,
                Recurrente = menu.Recurrente,
                Fecha = menu.Fecha,
                Nombre = menu.Nombre,
                Descripcion = menu.Descripcion,
                ComercioId = menu.ComercioId,
            };

            if (menu.Recurrente)
            {
                m.Dias = menu.Dias.Aggregate(new Dia(), (current, p) => current | p);
            }

            return m;
        }		
	}
}

