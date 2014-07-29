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
using Fooddily.ViewModels.Tarjeta;

namespace Fooddily.Controllers
{
    public class TarjetaController : ApiController
    {
        private AppContext db = new AppContext();

        // GET api/Tarjeta
        public IEnumerable<TarjetaViewModel> GetTarjetas()
        {
            return db.Tarjetas.ToList().Select(t => t.ToViewModel()).ToList();
        }
    }
}
