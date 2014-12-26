using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Authorize;
using MenuDelDia.Presentacion.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace MenuDelDia.Presentacion.Controllers
{
    [CustomAuthorize(Roles = "Administrator,User")]
    public class MenusController : BaseController
    {

        #region Private Methods
        private IList<MenuLocationModel> LoadLocations(Guid restaurantId, IEnumerable<Location> selectedLocations = null)
        {
            var selectedLocationsIds = new List<Guid>();

            if (selectedLocations != null)
                selectedLocationsIds = selectedLocations.Select(sl => sl.Id).ToList();

            return CurrentAppContext.Locations
                .Where(l => l.RestaurantId == restaurantId)
                .ToList()
                .Select(l => new MenuLocationModel
                {
                    Id = l.Id,
                    Name = l.Identifier,
                    Selected = (selectedLocationsIds.Contains(l.Id)),
                }).ToList();
        }

        public IList<TagModel> LoadTags(IList<Guid> selectedTags = null)
        {
            if (selectedTags == null)
            {
                return CurrentAppContext.Tags
                    .Where(t => t.ApplyToMenu)
                    .Select(t => new TagModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList();
            }

            return CurrentAppContext.Tags
                .Where(t => t.ApplyToMenu)
                .Select(t => new TagModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Selected = selectedTags.Contains(t.Id),
                }).ToList();
        }

        #endregion


        // GET: Menus
        public async Task<ActionResult> Index()
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var menus = CurrentAppContext.Menus.Include(m => m.Locations)
                .Where(m => m.Locations.All(l => l.RestaurantId == applicationUser.RestaurantId))
                .ToList();

            return View(menus.ToList());
        }

        // GET: Menus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = CurrentAppContext.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            var restaurantIds = menu.Locations.Select(r => r.RestaurantId).Distinct().ToList();
            if (restaurantIds.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var menuModel = new MenuModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Active = menu.Active,
                Ingredients = menu.Ingredients,
                Description = menu.Description,
                Cost = menu.Cost,
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
                Locations = LoadLocations(applicationUser.RestaurantId.Value, menu.Locations),
                Tags = LoadTags(menu.Tags.Select(t => t.Id).ToList())
            };
            return View(menuModel);
        }

        // GET: Menus/Create
        public async Task<ActionResult> Create()
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var menu = new MenuModel
            {
                Locations = LoadLocations(applicationUser.RestaurantId.Value),
                Tags = LoadTags(),
                Active = true,
            };
            menu.Locations.ForEach(l => l.Selected = true);
            return View(menu);
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Ingredients,Cost,MenuDays,SpecialDay,Active,Locations,Tags")] MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (applicationUser.RestaurantId.HasValue == false)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var selectedLocations = menu.Locations.Where(l => l.Selected).Select(l => l.Id).ToList();
                var entityLocations = CurrentAppContext.Locations.Where(l => selectedLocations.Contains(l.Id)).ToList();

                var restaurantIds = entityLocations.Select(r => r.RestaurantId).Distinct().ToList();
                if (restaurantIds.Count() > 1)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var selectedTags = menu.Tags.Where(t => t.Selected).Select(t => t.Id).ToList();
                var entityTags = CurrentAppContext.Tags.Where(t => selectedTags.Contains(t.Id)).ToList();

                var entityMenu = new Menu
                {
                    Id = Guid.NewGuid(),
                    Name = menu.Name,
                    Active = menu.Active,
                    Ingredients = menu.Ingredients,
                    Description = menu.Description,
                    Cost = menu.Cost,
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

                entityLocations.ForEach(l => entityMenu.Locations.Add(l));
                entityTags.ForEach(t => entityMenu.Tags.Add(t));

                CurrentAppContext.Menus.Add(entityMenu);
                CurrentAppContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = CurrentAppContext.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            var restaurantIds = menu.Locations.Select(r => r.RestaurantId).Distinct().ToList();
            if (restaurantIds.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var menuModel = new MenuModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Active = menu.Active,
                Ingredients = menu.Ingredients,
                Description = menu.Description,
                Cost = menu.Cost,
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
                Locations = LoadLocations(applicationUser.RestaurantId.Value, menu.Locations),
                Tags = LoadTags(menu.Tags.Select(t => t.Id).ToList()),
            };
            return View(menuModel);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Ingredients,Cost,MenuDays,SpecialDay,Active,Locations,Tags")] MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (applicationUser.RestaurantId.HasValue == false)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var selectedLocations = menu.Locations.Where(l => l.Selected).Select(l => l.Id).ToList();
                var entityLocations = CurrentAppContext.Locations.Where(l => selectedLocations.Contains(l.Id)).ToList();

                var restaurantIds = entityLocations.Select(r => r.RestaurantId).Distinct().ToList();
                if (restaurantIds.Count() > 1)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var entityMenu = CurrentAppContext.Menus.FirstOrDefault(m => m.Id == menu.Id);

                if (entityMenu != null)
                {
                    restaurantIds = entityMenu.Locations.Select(r => r.RestaurantId).Distinct().ToList();
                    if (restaurantIds.Count() > 1)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    entityMenu.Name = menu.Name;
                    entityMenu.Active = menu.Active;
                    entityMenu.Ingredients = menu.Ingredients;
                    entityMenu.Description = menu.Description;
                    entityMenu.Cost = menu.Cost;

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
                    entityMenu.Tags.Clear();

                    var tagsIds = menu.Tags.Where(t => t.Selected).Select(t => t.Id).ToList();
                    var entityTags = CurrentAppContext.Tags.Where(t => tagsIds.Contains(t.Id)).ToList();

                    entityLocations.ForEach(l => entityMenu.Locations.Add(l));
                    entityTags.ForEach(t => entityMenu.Tags.Add(t));

                    CurrentAppContext.Entry(entityMenu).State = EntityState.Modified;
                    CurrentAppContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            var applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser.RestaurantId.HasValue == false)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = CurrentAppContext.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            var restaurantIds = menu.Locations.Select(r => r.RestaurantId).Distinct().ToList();
            if (restaurantIds.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var menuModel = new MenuModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Active = menu.Active,
                Ingredients = menu.Ingredients,
                Description = menu.Description,
                Cost = menu.Cost,
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
                Locations = LoadLocations(applicationUser.RestaurantId.Value, menu.Locations),
                Tags = LoadTags(menu.Tags.Select(t => t.Id).ToList())
            };
            return View(menuModel);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Menu menu = CurrentAppContext.Menus.Find(id);

            var restaurantIds = menu.Locations.Select(r => r.RestaurantId).Distinct().ToList();
            if (restaurantIds.Count() > 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (await ValidateUserRequestWithUserLoggedIn(restaurantIds.First()) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            menu.Tags.Clear();
            menu.Locations.Clear();

            foreach (var comment in menu.Comments.ToList())
            {
                menu.Comments.Remove(comment);
            }

            CurrentAppContext.Menus.Remove(menu);
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
    }
}
