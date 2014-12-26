using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using MenuDelDia.Entities.Enums;
using MenuDelDia.Presentacion.Helpers;
using MenuDelDia.Presentacion.Models.ApiModels;
using MenuDelDia.Presentacion.Resources;
using MenuDelDia.Repository;

namespace MenuDelDia.Presentacion.Controllers.Api
{
    public class RestaurantsController : ApiController
    {

        #region Private Methods

        private IList<RestaurantApiModel> QueryRestaurants(Guid? id = null)
        {
            using (var db = new AppContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;


                var restaurants = db.Restaurants
                    .Include(r => r.Locations)
                    .Include(r => r.Cards)
                    .Include(r => r.Tags)
                    .Where(r => r.Active && (id.HasValue == false || r.Id == id.Value))
                    .Select(r => new RestaurantApiModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Email = r.Email,
                        LogoPath = r.LogoPath,
                        LogoName = r.LogoName,
                        LogoExtension = r.LogoExtension,
                        Url = r.Url,
                        Tags = r.Tags.Select(t => new TagApiModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                        }).ToList(),
                        Cards = r.Cards.Select(c => new CardApiModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CardType = (c.CardType == CardType.Credit) ? ViewResources.CardTypeCredit : ViewResources.CardTypeDebit,
                        }).ToList(),
                        Locations = r.Locations.Select(l => new LocationApiModel
                        {
                            Id = l.Id,
                            Identifier = l.Identifier,
                            Description = l.Description,
                            Delivery = l.Delivery,
                            Phone = l.Phone,
                            Streets = l.Streets,
                            OpenDays = l.OpenDays.Select(od => new OpenDayApiModel
                            {
                                DayOfWeek = od.DayOfWeek,
                                OpenHour = od.OpenHour,
                                OpenMinutes = od.OpenMinutes,
                                CloseHour = od.CloseHour,
                                CloseMinutes = od.CloseMinutes,
                            }).ToList(),
                            Latitude = l.Latitude,
                            Longitude = l.Longitude,
                            Distance = -1.0,
                        }).ToList()
                    }).ToList();


                restaurants.ForEach(r =>
                {
                    if (string.IsNullOrEmpty(r.LogoPath) == false)
                    {
                        var path = Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FolderLogos"]), string.Format("{0}{1}", r.LogoName, r.LogoExtension));
                        var file = new FileInfo(path);
                        if (file.Exists)
                        {
                            r.LogoBase64 = StringHelper.EncodeToBase64(file.FullName);
                            r.LogoExtension = r.LogoExtension.Replace(".", "");
                        }
                    }
                });
                return restaurants;
            }
        }

        private IList<RestaurantApiModel> FilterRestaurant(IList<RestaurantApiModel> data, RestaurantFilter filter)
        {
            if (filter.Start.HasValue && filter.Size.HasValue)
            {
                return data.Skip(filter.Start.Value)
                           .Take(filter.Size.Value)
                           .ToList();
            }

            return data;
        }
        #endregion

        [HttpGet]
        [Route("api/restaurants/{id:guid}")]
        public HttpResponseMessage Get(Guid id)
        {
            var restaurants = FilterRestaurant(QueryRestaurants(id), new RestaurantFilter());
            var restaurant = restaurants.FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, restaurant);
        }


        [HttpGet]
        [Route("api/restaurants/{start:int}/{size:int}")]
        public HttpResponseMessage Get(int start, int size)
        {
            var restaurants = FilterRestaurant(QueryRestaurants(), new RestaurantFilter { Start = start, Size = size });
            return Request.CreateResponse(HttpStatusCode.OK, restaurants);
        }
    }

    public class RestaurantFilter
    {
        public RestaurantFilter()
        {
            Start = null;
            Size = null;
        }

        public int? Start { get; set; }
        public int? Size { get; set; }

    }
}

