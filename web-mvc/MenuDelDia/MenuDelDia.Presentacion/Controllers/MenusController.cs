using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Models;
using MenuDelDia.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MenuDelDia.Presentacion.Controllers
{
    public class MenusController : Controller
    {
        private ApplicationUserManager _userManager;

        private AppContext db = new AppContext();
        public MenusController()
        {

        }

        public MenusController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
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


        private IList<MenuLocationModel> LoadLocations(Guid restaurantId, IEnumerable<Location> selectedLocations = null)
        {
            var selectedLocationsIds = new List<Guid>();

            if (selectedLocations != null)
                selectedLocationsIds = selectedLocations.Select(sl => sl.Id).ToList();

            return db.Locations
                .Where(l => l.RestaurantId == restaurantId)
                .ToList()
                .Select(l => new MenuLocationModel
                {
                    Id = l.Id,
                    Name = l.Identifier,
                    Selected = (selectedLocationsIds.Contains(l.Id)),
                }).ToList();
        }

        // GET: Menus
        public async Task<ActionResult> Index()
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var menus = db.Menus.Include(m => m.Locations)
                //.Where(m => m.RestaurantId == applicationUser.RestaurantId)
                                .ToList();

            return View(menus.ToList());
        }

        // GET: Menus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                throw new Exception();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            var menuModel = new MenuModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Active = menu.Active,
                Ingredients = menu.Ingredients,
                Description = menu.Description,
                MenuDays = new MenuDaysModel
                {
                    Friday = menu.MenuDays.Friday,
                    Monday = menu.MenuDays.Monday,
                    Saturday = menu.MenuDays.Saturday,
                    Sunday = menu.MenuDays.Sunday,
                    Thursday = menu.MenuDays.Thursday,
                    Tuesday = menu.MenuDays.Tuesday,
                    Wednesday = menu.MenuDays.Wednesday,
                },
                SpecialDay = new SpecialDayModel
                {
                    Date = menu.SpecialDay.Date,
                    Recurrent = menu.SpecialDay.Recurrent,
                },
                //SpecialDayDate = menu.SpecialDay.Date,
                //SpecialDayRecurrent = menu.SpecialDay.Recurrent,

                Locations = LoadLocations(applicationUser.RestaurantId.Value, menu.Locations)
            };
            return View(menuModel);
        }

        // GET: Menus/Create
        public async Task<ActionResult> Create()
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                throw new Exception();

            var menu = new MenuModel
            {
                Locations = LoadLocations(applicationUser.RestaurantId.Value),
            };

            return View(menu);
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Ingredients,MenuDays,SpecialDay,Active,Locations")] MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (applicationUser.RestaurantId.HasValue == false)
                    throw new Exception();

                var selectedLocations = menu.Locations.Where(l => l.Selected).Select(l => l.Id).ToList();
                var entityLocations = db.Locations.Where(l => selectedLocations.Contains(l.Id)).ToList();

                var entityMenu = new Menu
                {
                    Id = Guid.NewGuid(),
                    Name = menu.Name,
                    Active = menu.Active,
                    Ingredients = menu.Ingredients,
                    Description = menu.Description,
                    MenuDays = new MenuDays
                    {
                        Friday = menu.MenuDays.Friday,
                        Monday = menu.MenuDays.Monday,
                        Saturday = menu.MenuDays.Saturday,
                        Sunday = menu.MenuDays.Sunday,
                        Thursday = menu.MenuDays.Thursday,
                        Tuesday = menu.MenuDays.Tuesday,
                        Wednesday = menu.MenuDays.Wednesday,
                    },
                    SpecialDay = new SpecialDay
                    {
                        Date = menu.SpecialDay.Date,
                        Recurrent = menu.SpecialDay.Recurrent,
                    }
                };

                foreach (var entityLocation in entityLocations)
                {
                    entityMenu.Locations.Add(entityLocation);
                }

                db.Menus.Add(entityMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                throw new Exception();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            var menuModel = new MenuModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Active = menu.Active,
                Ingredients = menu.Ingredients,
                Description = menu.Description,
                MenuDays = new MenuDaysModel
                {
                    Friday = menu.MenuDays.Friday,
                    Monday = menu.MenuDays.Monday,
                    Saturday = menu.MenuDays.Saturday,
                    Sunday = menu.MenuDays.Sunday,
                    Thursday = menu.MenuDays.Thursday,
                    Tuesday = menu.MenuDays.Tuesday,
                    Wednesday = menu.MenuDays.Wednesday,
                },
                SpecialDay = new SpecialDayModel
                {
                    Date = menu.SpecialDay.Date,
                    Recurrent = menu.SpecialDay.Recurrent,
                },

                //SpecialDayDate = menu.SpecialDay.Date,
                //SpecialDayRecurrent = menu.SpecialDay.Recurrent,

                Locations = LoadLocations(applicationUser.RestaurantId.Value, menu.Locations)
            };


            return View(menuModel);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Ingredients,MenuDays,SpecialDay,Active,Locations")] MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (applicationUser.RestaurantId.HasValue == false)
                    throw new Exception();

                var selectedLocations = menu.Locations.Where(l => l.Selected).Select(l => l.Id).ToList();
                var entityLocations = db.Locations.Where(l => selectedLocations.Contains(l.Id)).ToList();

                var entityMenu = db.Menus.FirstOrDefault(m => m.Id == menu.Id);

                if (entityMenu != null)
                {
                    entityMenu.Name = menu.Name;
                    entityMenu.Active = menu.Active;
                    entityMenu.Ingredients = menu.Ingredients;
                    entityMenu.Description = menu.Description;

                    entityMenu.MenuDays.Friday = menu.MenuDays.Friday;
                    entityMenu.MenuDays.Monday = menu.MenuDays.Monday;
                    entityMenu.MenuDays.Saturday = menu.MenuDays.Saturday;
                    entityMenu.MenuDays.Sunday = menu.MenuDays.Sunday;
                    entityMenu.MenuDays.Thursday = menu.MenuDays.Thursday;
                    entityMenu.MenuDays.Tuesday = menu.MenuDays.Tuesday;
                    entityMenu.MenuDays.Wednesday = menu.MenuDays.Wednesday;

                    entityMenu.SpecialDay.Date = menu.SpecialDay.Date;
                    entityMenu.SpecialDay.Recurrent = menu.SpecialDay.Recurrent;

                    entityMenu.Locations.Clear();

                    foreach (var entityLocation in entityLocations)
                    {
                        entityMenu.Locations.Add(entityLocation);
                    }
                    db.Entry(entityMenu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(menu);
        }

        // GET: Menus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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
