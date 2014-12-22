using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MenuDelDia.Presentacion.Models.ApiModels;
using MenuDelDia.Repository;


namespace MenuDelDia.Presentacion.Controllers.Api
{
    public class MenusController : ApiController
    {

        #region Private Methods

        private IList<MenusApiModel> QueryMenus()
        {
            //todo: filtrar por restaurant activo

            using (var db = new AppContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;


                var dayOfWeek = DateTime.Now.DayOfWeek;
                var today = DateTime.Now.Date;

                var menus = db.Menus
                    .Include(m => m.Locations.Select(l => l.OpenDays))
                    .Include(m => m.Locations.Select(l => l.Restaurant))
                    .Include(m => m.Tags)
                    .Where(m => m.Active
                        && (
                            (dayOfWeek == DayOfWeek.Monday && m.MenuDays.Monday) ||
                            (dayOfWeek == DayOfWeek.Tuesday && m.MenuDays.Tuesday) ||
                            (dayOfWeek == DayOfWeek.Wednesday && m.MenuDays.Wednesday) ||
                            (dayOfWeek == DayOfWeek.Thursday && m.MenuDays.Thursday) ||
                            (dayOfWeek == DayOfWeek.Friday && m.MenuDays.Friday) ||
                            (dayOfWeek == DayOfWeek.Saturday && m.MenuDays.Saturday) ||
                            (dayOfWeek == DayOfWeek.Sunday && m.MenuDays.Sunday) ||
                            (
                                (m.SpecialDay.Date.HasValue && m.SpecialDay.Date == today) ||
                                (m.SpecialDay.Date.HasValue && m.SpecialDay.Recurrent && m.SpecialDay.Date.Value.Day == today.Day)
                            )
                           )
                    && m.Locations.Any(l =>
                                            l.OpenDays.Any(od =>
                                            (od.DayOfWeek == DayOfWeek.Monday) ||
                                            (od.DayOfWeek == DayOfWeek.Tuesday) ||
                                            (od.DayOfWeek == DayOfWeek.Wednesday) ||
                                            (od.DayOfWeek == DayOfWeek.Thursday) ||
                                            (od.DayOfWeek == DayOfWeek.Friday) ||
                                            (od.DayOfWeek == DayOfWeek.Saturday) ||
                                            (od.DayOfWeek == DayOfWeek.Sunday))
                                        )
                        )
                .Select(m =>
                    new MenusApiModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Ingredients = m.Ingredients,
                        Cost = m.Cost,
                        MenuDays = new MenuDaysApiModel
                        {
                            Monday = m.MenuDays.Monday,
                            Tuesday = m.MenuDays.Tuesday,
                            Wednesday = m.MenuDays.Wednesday,
                            Thursday = m.MenuDays.Thursday,
                            Friday = m.MenuDays.Friday,
                            Sunday = m.MenuDays.Sunday,
                            Saturday = m.MenuDays.Saturday,
                        },
                        SpecialDay = new SpecialDayApiModel
                        {
                            Date = m.SpecialDay.Date,
                            Recurrent = m.SpecialDay.Recurrent
                        },
                        Locations = m.Locations.Select(l => new LocationApiModel
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
                            Logo = new LogoApiModel
                            {
                                LogoExtension = l.Restaurant.LogoExtension,
                                LogoName = l.Restaurant.LogoName,
                                LogoPath = l.Restaurant.LogoPath,
                            }
                        }).ToList(),

                        Tags = m.Tags.Select(t => new TagApiModel { Id = t.Id, Name = t.Name }).ToList(),
                    })
               .ToList();
                return menus;
            }
        }

        private IList<MenusApiModel> FilterMenus(IList<MenusApiModel> menus, MenusFilter filter)
        {
            if (filter.Latitude.HasValue && filter.Longitude.HasValue)
            {
                var filteredMenus = new List<MenusApiModel>();

                var hashDistances = new Dictionary<Guid, double>();
                var geoCoordinateOrigin = new GeoCoordinate(filter.Latitude.Value, filter.Longitude.Value);

                foreach (var menu in menus)
                {
                    MenusApiModel localMenuVariable = menu;

                    var locations = localMenuVariable.Locations.ToList();
                    localMenuVariable.Locations = new List<LocationApiModel>();

                    locations.ForEach(l =>
                    {
                        if (hashDistances.ContainsKey(l.Id))
                        {
                            l.Distance = hashDistances[l.Id];
                        }
                        else
                        {
                            l.Distance =
                                geoCoordinateOrigin.GetDistanceTo(new GeoCoordinate
                                {
                                    Latitude = l.Latitude,
                                    Longitude = l.Longitude
                                });
                            hashDistances.Add(l.Id, l.Distance);
                        }

                        if (filter.Radius.HasValue == false || l.Distance <= filter.Radius)
                        {
                            localMenuVariable.Locations.Add(l);
                            if (localMenuVariable.NearestLocation == null)
                            {
                                localMenuVariable.NearestLocation = l;
                            }
                            else
                            {
                                if (localMenuVariable.NearestLocation.Distance > l.Distance)
                                {
                                    localMenuVariable.NearestLocation = l;
                                }
                            }
                        }
                    });

                    if (localMenuVariable.Locations.Any())
                    {
                        filteredMenus.Add(localMenuVariable);
                    }
                }

                return filteredMenus.OrderBy(m => m.NearestLocation.Distance)
                .Skip(filter.Start ?? 0)
                .Take(filter.Size ?? menus.Count)
                .ToList();
            }

            return menus.OrderBy(m => m.Name)
                             .Skip(filter.Start ?? 0)
                             .Take(filter.Size ?? menus.Count)
                             .ToList();
        }


        #endregion


        [HttpGet]
        [Route("api/menus")]
        public HttpResponseMessage Get()
        {
            var menus = FilterMenus(QueryMenus(), new MenusFilter());
            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }

        [HttpGet]
        [Route("api/menus/{start:int}/{size:int}")]
        public HttpResponseMessage Get(int start, int size)
        {
            var menus = FilterMenus(QueryMenus(), new MenusFilter { Start = start, Size = size });
            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }

        [HttpGet]
        [Route("api/menus/{latitude:double}/{longitude:double}")]
        public HttpResponseMessage Get(double latitude, double longitude)
        {
            var menus = FilterMenus(QueryMenus(), new MenusFilter { Latitude = latitude, Longitude = longitude });
            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }

        [HttpGet]
        [Route("api/menus/{latitude:double}/{longitude:double}/{start:int}/{size:int}")]
        public HttpResponseMessage Get(double latitude, double longitude, int start, int size)
        {
            var menus = FilterMenus(QueryMenus(), new MenusFilter
            {
                Latitude = latitude,
                Longitude = longitude,
                Start = start,
                Size = size
            });
            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }

        [HttpGet]
        [Route("api/menus/{latitude:double}/{longitude:double}/{radius:int}")]
        public HttpResponseMessage Get(double latitude, double longitude, int radius)
        {
            var menus = FilterMenus(QueryMenus(), new MenusFilter
            {
                Latitude = latitude,
                Longitude = longitude,
                Radius = radius,
            });
            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }

        [HttpGet]
        [Route("api/menus/{latitude:double}/{longitude:double}/{radius:int}/{start:int}/{size:int}")]
        public HttpResponseMessage Get(double latitude, double longitude, int radius, int start, int size)
        {
            var menus = FilterMenus(QueryMenus(), new MenusFilter
            {
                Latitude = latitude,
                Longitude = longitude,
                Radius = radius,
                Start = start,
                Size = size
            });
            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }
    }

    public class MenusFilter
    {
        public MenusFilter()
        {
            Latitude = null;
            Longitude = null;
            Radius = null;
            Start = null;
            Size = null;
        }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Radius { get; set; }
        public int? Start { get; set; }
        public int? Size { get; set; }

    }
}
