using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Helpers;
using MenuDelDia.Presentacion.Models.ApiModels;
using MenuDelDia.Repository;

namespace MenuDelDia.Presentacion.Controllers.Api
{
    public class SuggestionsController : ApiController
    {
        [HttpPost]
        [Route("api/suggestions")]
        public HttpResponseMessage Post([FromBody]SuggestionApiModel suggestion)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppContext())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;

                    db.Suggestion.Add(new Suggestion
                    {
                        Id = Guid.NewGuid(),
                        CreationDateTime = DateTimeOffset.Now,
                        Message = suggestion.Message,
                        Ip = HttpContext.Current.Request.GetIPAddress(),
                        Uuid = suggestion.Uuid
                    });
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
