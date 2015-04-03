using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Helpers;
using MenuDelDia.Presentacion.Models;
using Microsoft.AspNet.Identity;

namespace MenuDelDia.Presentacion.Controllers.Api.Site
{
    public class TagsApiController : ApiBaseController
    {

        public IList<TagModel> LoadTags()
        {
            return CurrentAppContext.Tags
                .Where(t => t.ApplyToRestaurant)
                .Select(t => new TagModel
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var result = LoadTags();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
