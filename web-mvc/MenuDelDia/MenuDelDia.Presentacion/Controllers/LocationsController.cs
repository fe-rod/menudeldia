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



        private IList<OpenDaysModel> GenerateOpenDaysModels(IList<OpenDay> openDays = null)
        {
            if (openDays == null)
            {
                return new List<OpenDaysModel>
                {
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Monday},
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Tuesday},
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Wednesday},
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Thursday},
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Friday},
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Saturday},
                    new OpenDaysModel {DayOfWeek = DayOfWeek.Sunday}
                };
            }

            var monday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Monday);
            var tuesday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Tuesday);
            var wednesday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Wednesday);
            var thursday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Thursday);
            var friday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Friday);
            var saturday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Saturday);
            var sunday = openDays.FirstOrDefault(od => od.DayOfWeek == DayOfWeek.Sunday);



            return new List<OpenDaysModel>
            {
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Monday,
                    Open = (monday != null),
                    Id = (monday != null) ? monday.Id : Guid.Empty,
                    OpenHour = (monday != null) ? monday.OpenHour : 0,
                    OpenMinutes = (monday != null) ? monday.OpenMinutes : 0,
                    CloseHour = (monday != null) ? monday.CloseHour : 0,
                    CloseMinutes = (monday != null) ? monday.CloseMinutes : 0,
                },
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    Open = (tuesday != null),
                    Id = (tuesday != null) ? tuesday.Id : Guid.Empty,
                    OpenHour = (tuesday != null) ? tuesday.OpenHour : 0,
                    OpenMinutes = (tuesday != null) ? tuesday.OpenMinutes : 0,
                    CloseHour = (tuesday != null) ? tuesday.CloseHour : 0,
                    CloseMinutes = (tuesday != null) ? tuesday.CloseMinutes : 0,
                },
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    Open = (wednesday != null),
                    Id = (wednesday != null) ? wednesday.Id : Guid.Empty,
                    OpenHour = (wednesday != null) ? wednesday.OpenHour : 0,
                    OpenMinutes = (wednesday != null) ? wednesday.OpenMinutes : 0,
                    CloseHour = (wednesday != null) ? wednesday.CloseHour : 0,
                    CloseMinutes = (wednesday != null) ? wednesday.CloseMinutes : 0,
                },
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Thursday,
                    Open = (thursday != null),
                    Id = (thursday != null) ? thursday.Id : Guid.Empty,
                    OpenHour = (thursday != null) ? thursday.OpenHour : 0,
                    OpenMinutes = (thursday != null) ? thursday.OpenMinutes : 0,
                    CloseHour = (thursday != null) ? thursday.CloseHour : 0,
                    CloseMinutes = (thursday != null) ? thursday.CloseMinutes : 0,
                },
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Friday,
                    Open = (friday != null),
                    Id = (friday != null) ? friday.Id : Guid.Empty,
                    OpenHour = (friday != null) ? friday.OpenHour : 0,
                    OpenMinutes = (friday != null) ? friday.OpenMinutes : 0,
                    CloseHour = (friday != null) ? friday.CloseHour : 0,
                    CloseMinutes = (friday != null) ? friday.CloseMinutes : 0,
                },
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    Open = (saturday != null),
                    Id = (saturday != null) ? saturday.Id : Guid.Empty,
                    OpenHour = (saturday != null) ? saturday.OpenHour : 0,
                    OpenMinutes = (saturday != null) ? saturday.OpenMinutes : 0,
                    CloseHour = (saturday != null) ? saturday.CloseHour : 0,
                    CloseMinutes = (saturday != null) ? saturday.CloseMinutes : 0,
                },
                new OpenDaysModel
                {
                    DayOfWeek = DayOfWeek.Sunday,
                    Open = (sunday != null),
                    Id = (sunday != null) ? sunday.Id : Guid.Empty,
                    OpenHour = (sunday != null) ? sunday.OpenHour : 0,
                    OpenMinutes = (sunday != null) ? sunday.OpenMinutes : 0,
                    CloseHour = (sunday != null) ? sunday.CloseHour : 0,
                    CloseMinutes = (sunday != null) ? sunday.CloseMinutes : 0,
                },
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
                Delivery = location.Delivery,
                Description = location.Description,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone,
                Streets = location.Streets,
                OpenDays = GenerateOpenDaysModels(location.OpenDays.ToList())
            };

            return View(locationModel);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            var model = new LocationModel
            {
                OpenDays = GenerateOpenDaysModels()
            };
            return View(model);
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Streets,Phone,Description,Delivery,Latitude,Longitude,OpenDays")] LocationModel location)
        {
            if (ModelState.IsValid)
            {
                var openDays = location.OpenDays.Where(od => od.Open).ToList();

                var entityLocation = new Location
                {
                    Id = Guid.NewGuid(),
                    Streets = location.Streets,
                    Delivery = location.Delivery,
                    Description = location.Description,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Phone = location.Phone,
                    RestaurantId = ApplicationUser.RestaurantId,
                    OpenDays = openDays.Select(od => new OpenDay
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
                Delivery = location.Delivery,
                Description = location.Description,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone,
                Streets = location.Streets,
                OpenDays = GenerateOpenDaysModels(location.OpenDays.ToList())
            };


            return View(locationModel);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Streets,Phone,Description,Delivery,Latitude,Longitude,OpenDays")] LocationModel location)
        {
            if (ModelState.IsValid)
            {
                var entityLocation = db.Locations.FirstOrDefault(l => l.Id == location.Id);
                if (entityLocation != null)
                {
                    entityLocation.Delivery = location.Delivery;
                    entityLocation.Description = location.Description;
                    entityLocation.Latitude = location.Latitude;
                    entityLocation.Longitude = location.Longitude;
                    entityLocation.Phone = location.Phone;
                    entityLocation.Streets = location.Streets;

                    var openDays = location.OpenDays.Where(od => od.Open).ToList();
                    entityLocation.OpenDays.Clear();

                    foreach (var openDay in openDays)
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
    }
}
