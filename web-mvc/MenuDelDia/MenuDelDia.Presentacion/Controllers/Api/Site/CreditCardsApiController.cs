using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using MenuDelDia.Entities;
using MenuDelDia.Entities.Enums;
using MenuDelDia.Presentacion.Helpers;
using MenuDelDia.Presentacion.Models;
using Microsoft.AspNet.Identity;

namespace MenuDelDia.Presentacion.Controllers.Api.Site
{
    public class CreditCardsApiController : ApiBaseController
    {

        public IList<CardModel> LoadCards(IList<Guid> selectedCardIds = null)
        {
            return CurrentAppContext.Cards.Select(c => new CardModel
               {
                   Id = c.Id,
                   Name = c.Name,
                   Type = (c.CardType == CardType.Credit ? "Crédito" : "Débito"),
               }).ToList();
        }

        [HttpGet]
        [Route("api/creditcards")]
        public HttpResponseMessage Get()
        {
            var result = LoadCards();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
