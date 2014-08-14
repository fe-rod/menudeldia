using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fooddily.Mappers;
using Fooddily.Models;
using Fooddily.ViewModels.Menu;
using Fooddily.ViewModels.Zona;

namespace Fooddily.Controllers
{
    public class ZonaController : ApiController
    {
        private AppContext db = new AppContext();

        // GET api/Zona
        public IEnumerable<ZonaViewModel> GetZonas()
        {
            return db.Zonas.ToList().Select(t => t.ToViewModel()).ToList();
        }
    }
}
