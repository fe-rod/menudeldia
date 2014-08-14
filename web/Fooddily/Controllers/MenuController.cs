using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fooddily.Mappers;
using Fooddily.Models;
using Fooddily.ViewModels.Comentario;
using Fooddily.ViewModels.Menu;

namespace Fooddily.Controllers
{
    public class MenuController : ApiController
    {
        private AppContext db = new AppContext();

        // GET api/Menu/5
        public MenuViewModel GetMenu(int id)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return menu.ToViewModel();
        }

        [HttpGet]
        public IEnumerable<MenuViewModel> SearchMenus(string dt)
        {
            var menus = db.Menus.ToList();

            //Listo los menús de la fecha de hoy
            var menusDelDia = new List<Menu>();
            foreach (var menu in menus)
            {
                dt = dt.Replace("\"","");
                var dia = DateTime.Parse(dt);
                var diaEnum = Enum.GetValues(typeof(Dia)).Cast<Enum>().ElementAt((int)dia.DayOfWeek);
                if (menu.Recurrente && menu.Dias != null && ((Dia)menu.Dias).HasFlag(diaEnum))
                {
                    menusDelDia.Add(menu);
                }
                if (!menu.Recurrente && menu.Fecha.HasValue && menu.Fecha.Value.Date == dia.Date)
                {
                    menusDelDia.Add(menu);
                }
            }

            return menusDelDia.Select(m => m.ToViewModel()).ToList();
        }

        public HttpResponseMessage SaveMenu(HttpRequestMessage request, MenuViewModel menu)
        {
            if (ModelState.IsValid)
            {
                if ((menu.Recurrente && menu.Dias.Count == 0) || (!menu.Recurrente && menu.Fecha == null))
                    return request.CreateResponse(HttpStatusCode.BadRequest, new[] { "Debe seleccionar al menos un día" });

                var model = menu.ToModel();
                model.Tags = new List<Tag>();
                if (menu.Tags != null)
                {
                    foreach (var tag in menu.Tags)
                    {
                        model.Tags.Add(db.Tags.Find(tag.Id));
                    }
                }
                db.Menus.Add(model);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, GetErrorMessages());
        }

        private IEnumerable<string> GetErrorMessages()
        {
            return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
        }

        // DELETE api/Menu/5
        public HttpResponseMessage DeleteMenu(int id)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var comentarios = menu.Comentarios.ToList();
            foreach(var comentario in comentarios)
            {
                db.Comentarios.Remove(comentario);
            }

            db.Menus.Remove(menu);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, menu);
        }
    }
}
