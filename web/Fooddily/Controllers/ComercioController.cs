using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fooddily.Mappers;
using Fooddily.Models;
using Fooddily.ViewModels.Comercio;

namespace Fooddily.Controllers
{
    public class ComercioController : ApiController
    {
        private AppContext db = new AppContext();

        // GET api/Comercio
        public IEnumerable<ComercioViewModel> GetComercios()
        {
            return db.Comercios.ToList().Select(c => c.ToViewModel()).ToList();
        }

        // GET api/Comercio/5
        public ComercioViewModel GetComercio(int id)
        {
            Comercio comercio = db.Comercios.Find(id);
            if (comercio == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return comercio.ToViewModel();
        }

    }
}
