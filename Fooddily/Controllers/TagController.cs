using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fooddily.Mappers;
using Fooddily.Models;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Tag;

namespace Fooddily.Controllers
{
    public class TagController : ApiController
    {
        private AppContext db = new AppContext();

        // GET api/Tag
        public IEnumerable<TagViewModel> GetTags()
        {
            return db.Tags.ToList().Select(t => t.ToViewModel()).ToList();
        }

        [HttpGet]
        public IEnumerable<TagViewModel> SearchTags(string search)
        {
            return db.Tags.Where(t => t.Nombre.ToLower().Contains(search.ToLower())).ToList().Select(t => t.ToViewModel()).ToList();
        }
    }
}
