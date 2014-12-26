using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Authorize;
using MenuDelDia.Presentacion.Models;
using WebGrease.Css.Extensions;


namespace MenuDelDia.Presentacion.Controllers
{
    [CustomAuthorize(Roles = "Administrator,User")]
    public class LocationsController : BaseController
    {
        #region Private Methods
        public IList<TagModel> LoadTags(IList<Guid> selectedTags = null)
        {
            if (selectedTags == null)
            {
                return CurrentAppContext.Tags
                    .Where(t => t.ApplyToLocation)
                    .Select(t => new TagModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList();
            }

            return CurrentAppContext.Tags
                .Where(t => t.ApplyToLocation)
                .Select(t => new TagModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Selected = selectedTags.Contains(t.Id),
                }).ToList();
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                                     "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }
        #endregion


        public LocationsController()
        {

        }

        public string DayOfWeekToString(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "Lunes";
                case DayOfWeek.Tuesday: return "Martes";
                case DayOfWeek.Wednesday: return "Miércoles";
                case DayOfWeek.Thursday: return "Juéves";
                case DayOfWeek.Friday: return "Viernes";
                case DayOfWeek.Saturday: return "Sábado";
                case DayOfWeek.Sunday: return "Domingo";
                default: return string.Empty;
            }
        }
        public IEnumerable<SelectListItem> DayOfWeeksSelectListItems()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{Value = DayOfWeek.Monday.ToString(),Text = DayOfWeekToString(DayOfWeek.Monday)},
                    new SelectListItem{Value = DayOfWeek.Tuesday.ToString(),Text =DayOfWeekToString(DayOfWeek.Tuesday)},
                    new SelectListItem{Value = DayOfWeek.Wednesday.ToString(),Text = DayOfWeekToString(DayOfWeek.Wednesday)},
                    new SelectListItem{Value = DayOfWeek.Thursday.ToString(),Text = DayOfWeekToString(DayOfWeek.Thursday)},
                    new SelectListItem{Value = DayOfWeek.Friday.ToString(),Text = DayOfWeekToString(DayOfWeek.Friday)},
                    new SelectListItem{Value = DayOfWeek.Saturday.ToString(),Text = DayOfWeekToString(DayOfWeek.Saturday)},
                    new SelectListItem{Value = DayOfWeek.Sunday.ToString(),Text = DayOfWeekToString(DayOfWeek.Sunday)},
                };
        }


        // GET: Locations
        public async Task<ActionResult> Index()
        {
            var currentUserRestaurantId = (await CurrentUser()).RestaurantId;
            var locations = CurrentAppContext.Locations.Where(l => l.RestaurantId == currentUserRestaurantId).ToList();

            return View(locations);
        }

        // GET: Locations/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = CurrentAppContext.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            if (await ValidateUserRequestWithUserLoggedIn(location.RestaurantId) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var locationModel = new LocationModel
            {
                Id = location.Id,
                Identifier = location.Identifier,
                Delivery = location.Delivery,
                Description = location.Description,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone,
                Streets = location.Streets,
                OpenDays = location.OpenDays.Select(od => new OpenDaysModel
                {
                    Id = Guid.NewGuid(),
                    DayOfWeek = od.DayOfWeek,
                    DayOfWeekStr = DayOfWeekToString(od.DayOfWeek),
                    OpenHour = od.OpenHour,
                    OpenMinutes = od.OpenMinutes,
                    CloseHour = od.CloseHour,
                    CloseMinutes = od.CloseMinutes,
                }).ToList(),
                Tags = LoadTags(location.Tags.Select(t => t.Id).ToList()),
            };

            return View(locationModel);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            var model = new LocationModel
            {
                Tags = LoadTags()
            };
            return View(model);
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Identifier,Streets,Phone,Description,Delivery,Latitude,Longitude,OpenDays,Tags")] LocationModel location)
        {
            if (ModelState.IsValid)
            {
                var currentUserRestaurantId = (await CurrentUser()).RestaurantId;
                if (currentUserRestaurantId.HasValue == false)
                {
                    ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
                    return View(location);
                }

                var selectedTags = location.Tags.Where(t => t.Selected).Select(t => t.Id).ToList();
                var entityTags = CurrentAppContext.Tags.Where(t => selectedTags.Contains(t.Id)).ToList();

                var entityLocation = new Location
                {
                    Id = Guid.NewGuid(),
                    Identifier = location.Identifier,
                    Streets = location.Streets,
                    Delivery = location.Delivery,
                    Description = location.Description,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    SpatialLocation = CreatePoint(location.Latitude, location.Longitude),
                    Phone = location.Phone,
                    RestaurantId = currentUserRestaurantId.Value,
                    OpenDays = location.OpenDays.Select(od => new OpenDay
                    {
                        Id = Guid.NewGuid(),
                        DayOfWeek = od.DayOfWeek,
                        OpenHour = od.OpenHour,
                        OpenMinutes = od.OpenMinutes,
                        CloseHour = od.CloseHour,
                        CloseMinutes = od.CloseMinutes,
                    }).ToList()
                };

                entityTags.ForEach(t => entityLocation.Tags.Add(t));


                CurrentAppContext.Locations.Add(entityLocation);
                CurrentAppContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = CurrentAppContext.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            if (await ValidateUserRequestWithUserLoggedIn(location.RestaurantId) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tagsIds = location.Tags.Select(t => t.Id).ToList();

            var locationModel = new LocationModel
            {
                Id = location.Id,
                Identifier = location.Identifier,
                Delivery = location.Delivery,
                Description = location.Description,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone,
                Streets = location.Streets,
                OpenDays = location.OpenDays.Select(od => new OpenDaysModel
                {
                    Id = Guid.NewGuid(),
                    OpenHour = od.OpenHour,
                    OpenMinutes = od.OpenMinutes,
                    CloseHour = od.CloseHour,
                    CloseMinutes = od.CloseMinutes,
                    DayOfWeek = od.DayOfWeek,
                    DayOfWeekStr = DayOfWeekToString(od.DayOfWeek)
                }).ToList(),
                Tags = LoadTags(tagsIds),
            };

            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(locationModel);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Identifier,Streets,Phone,Description,Delivery,Latitude,Longitude,OpenDays,Tags")] LocationModel location)
        {
            if (ModelState.IsValid)
            {
                var entityLocation = CurrentAppContext.Locations.FirstOrDefault(l => l.Id == location.Id);

                if (entityLocation != null)
                {
                    if (await ValidateUserRequestWithUserLoggedIn(entityLocation.RestaurantId) == false)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    entityLocation.Identifier = location.Identifier;
                    entityLocation.Delivery = location.Delivery;
                    entityLocation.Description = location.Description;
                    entityLocation.Latitude = location.Latitude;
                    entityLocation.Longitude = location.Longitude;
                    entityLocation.SpatialLocation = CreatePoint(location.Latitude, location.Longitude);
                    entityLocation.Phone = location.Phone;
                    entityLocation.Streets = location.Streets;

                    entityLocation.OpenDays.Clear();
                    location.OpenDays.ForEach(openDay =>
                        entityLocation.OpenDays.Add(new OpenDay
                        {
                            Id = Guid.NewGuid(),
                            DayOfWeek = openDay.DayOfWeek,
                            OpenHour = openDay.OpenHour,
                            OpenMinutes = openDay.OpenMinutes,
                            CloseHour = openDay.CloseHour,
                            CloseMinutes = openDay.CloseMinutes,
                        }));

                    entityLocation.Tags.Clear();
                    var selectedTags = location.Tags.Where(t => t.Selected).Select(t => t.Id).ToList();
                    var entityTags = CurrentAppContext.Tags.Where(t => selectedTags.Contains(t.Id)).ToList();
                    entityTags.ForEach(t => entityLocation.Tags.Add(t));

                    CurrentAppContext.Entry(entityLocation).State = EntityState.Modified;
                    CurrentAppContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = CurrentAppContext.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            if (await ValidateUserRequestWithUserLoggedIn(location.RestaurantId) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var locationModel = new LocationModel
            {
                Id = location.Id,
                Identifier = location.Identifier,
                Delivery = location.Delivery,
                Description = location.Description,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone,
                Streets = location.Streets,
                OpenDays = location.OpenDays.Select(od => new OpenDaysModel
                {
                    Id = Guid.NewGuid(),
                    DayOfWeek = od.DayOfWeek,
                    DayOfWeekStr = DayOfWeekToString(od.DayOfWeek),
                    OpenHour = od.OpenHour,
                    OpenMinutes = od.OpenMinutes,
                    CloseHour = od.CloseHour,
                    CloseMinutes = od.CloseMinutes,
                }).ToList(),
                Tags = LoadTags(location.Tags.Select(t => t.Id).ToList()),
            };

            return View(locationModel);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Location location = CurrentAppContext.Locations.Find(id);

            if (await ValidateUserRequestWithUserLoggedIn(location.RestaurantId) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            location.Menus.Clear();
            location.Tags.Clear();

            foreach (var openDay in location.OpenDays.ToList())
            {
                location.OpenDays.Remove(openDay);
            }

            CurrentAppContext.Locations.Remove(location);
            CurrentAppContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CurrentAppContext.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult AddDayOfWeek(LocationModel locationModel)
        {
            if (locationModel.OpenDays.Any(l => l.DayOfWeek == locationModel.DayOfWeek &&
                                                l.OpenHour == locationModel.OpenHour &&
                                                l.OpenMinutes == locationModel.OpenMinutes &&
                                                l.CloseHour == locationModel.CloseHour &&
                                                l.CloseMinutes == locationModel.CloseMinutes
                ))
            {
                return PartialView("_OpenDays", locationModel);
            }
            locationModel.OpenDays.Add(new OpenDaysModel
            {
                Id = Guid.NewGuid(),
                OpenHour = locationModel.OpenHour,
                OpenMinutes = locationModel.OpenMinutes,
                CloseHour = locationModel.CloseHour,
                CloseMinutes = locationModel.CloseMinutes,
                DayOfWeek = locationModel.DayOfWeek,
                DayOfWeekStr = DayOfWeekToString(locationModel.DayOfWeek),
            });

            locationModel.OpenDays = locationModel.OpenDays.OrderBy(l => l.DayOfWeek).ToList();
            return PartialView("_OpenDays", locationModel);
        }

        public ActionResult RemoveDayOfWeek(LocationModel locationModel)
        {
            if (locationModel.RemoveDayOfWeek.HasValue)
            {
                var item = locationModel.OpenDays.FirstOrDefault(l => l.Id == locationModel.RemoveDayOfWeek.Value);
                if (item != null)
                    locationModel.OpenDays.Remove(item);
            }
            return PartialView("_OpenDays", locationModel);
        }
    }
}