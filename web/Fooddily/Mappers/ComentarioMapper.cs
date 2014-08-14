using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fooddily.Models;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Tag;

namespace Fooddily.Mappers
{
    public static class ComentarioMapper
    {
        public static ComentarioViewModel ToViewModel(this Comentario comentario)
        {
            return new ComentarioViewModel
            {
                Id = comentario.Id,
                Descripcion = comentario.Descripcion,
                Puntuacion = comentario.Puntuacion,
                ComercioId = comentario.ComercioId,
                MenuId = comentario.MenuId,
                Fecha = comentario.Fecha.ToShortDateString() + " a las " + comentario.Fecha.ToShortTimeString()
            };
        }

        public static Comentario ToModel(this ComentarioViewModel comentario)
        {
            return new Comentario
            {
                Id = comentario.Id,
                Descripcion = comentario.Descripcion,
                Puntuacion = comentario.Puntuacion,
                ComercioId = comentario.ComercioId,
                MenuId = comentario.MenuId,
                Fecha = System.DateTime.Now
            };
        }		
	}
}

