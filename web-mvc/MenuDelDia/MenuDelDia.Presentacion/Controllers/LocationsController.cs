using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Models;
using MenuDelDia.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace MenuDelDia.Presentacion.Controllers
{
    [Authorize]
    public class LocationsController : Controller
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private ApplicationUserManager _userManager;
        private ApplicationUser _applicationUser;
        private AppContext db = new AppContext();


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationUser ApplicationUser
        {
            get
            {
                return _applicationUser ?? UserManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            }
            private set
            {
                _applicationUser = value;
            }
        }


        public LocationsController()
        {

        }

        public LocationsController(ApplicationUserManager applicationUserManager)
        {
            UserManager = applicationUserManager;
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
        public ActionResult Index()
        {
            var locations = db.Locations.Where(l => l.RestaurantId == ApplicationUser.RestaurantId).ToList();

            return View(locations);
        }

        // GET: Locations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
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
                    OpenHour = od.OpenHour,
                    OpenMinutes = od.OpenMinutes,
                    CloseHour = od.CloseHour,
                    CloseMinutes = od.CloseMinutes,
                }).ToList()
            };

            return View(locationModel);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(new LocationModel());
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Identifier,Streets,Phone,Description,Delivery,Latitude,Longitude,OpenDays")] LocationModel location)
        {
            if (ModelState.IsValid)
            {
                var entityLocation = new Location
                {
                    Id = Guid.NewGuid(),
                    Identifier = location.Identifier,
                    Streets = location.Streets,
                    Delivery = location.Delivery,
                    Description = location.Description,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Phone = location.Phone,
                    RestaurantId = ApplicationUser.RestaurantId,
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

                db.Locations.Add(entityLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
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
                    OpenHour = od.OpenHour,
                    OpenMinutes = od.OpenMinutes,
                    CloseHour = od.CloseHour,
                    CloseMinutes = od.CloseMinutes,
                    DayOfWeek = od.DayOfWeek,
                    DayOfWeekStr = DayOfWeekToString(od.DayOfWeek)
                }).ToList()
            };

            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(locationModel);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Identifier,Streets,Phone,Description,Delivery,Latitude,Longitude,OpenDays")] LocationModel location)
        {
            if (ModelState.IsValid)
            {
                var entityLocation = db.Locations.FirstOrDefault(l => l.Id == location.Id);
                if (entityLocation != null)
                {
                    entityLocation.Identifier = location.Identifier;
                    entityLocation.Delivery = location.Delivery;
                    entityLocation.Description = location.Description;
                    entityLocation.Latitude = location.Latitude;
                    entityLocation.Longitude = location.Longitude;
                    entityLocation.Phone = location.Phone;
                    entityLocation.Streets = location.Streets;

                    entityLocation.OpenDays.Clear();
                    foreach (var openDay in location.OpenDays)
                    {
                        entityLocation.OpenDays.Add(new OpenDay
                        {
                            Id = Guid.NewGuid(),
                            DayOfWeek = openDay.DayOfWeek,
                            OpenHour = openDay.OpenHour,
                            OpenMinutes = openDay.OpenMinutes,
                            CloseHour = openDay.CloseHour,
                            CloseMinutes = openDay.CloseMinutes,
                        });
                    }
                    db.Entry(entityLocation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.DayOfWeeks = DayOfWeeksSelectListItems();
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
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
