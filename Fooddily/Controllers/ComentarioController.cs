using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fooddily.Mappers;
using Fooddily.Models;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Tag;

namespace Fooddily.Controllers
{
    public class ComentarioController : ApiController
    {
        private AppContext db = new AppContext();


        public HttpResponseMessage SaveComentario(HttpRequestMessage request, ComentarioViewModel comentario)
        {
            if (ModelState.IsValid)
            {
                if (comentario.Puntuacion == 0)
                    return request.CreateResponse(HttpStatusCode.BadRequest, new[] { "Debe ingresar una puntuacion entre 1 y 5" });

                var model = comentario.ToModel();
                db.Comentarios.Add(model);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, GetErrorMessages());
        }

        private IEnumerable<string> GetErrorMessages()
        {
            return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
        }
    }
}
