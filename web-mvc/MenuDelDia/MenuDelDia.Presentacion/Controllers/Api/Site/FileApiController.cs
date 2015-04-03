using System.Net;
using System.Net.Http;
using System.Web;

namespace MenuDelDia.Presentacion.Controllers.Api.Site
{
    public class FileApiController : ApiBaseController
    {
        public HttpResponseMessage Upload(HttpPostedFile file)
        {
            var x = file;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
