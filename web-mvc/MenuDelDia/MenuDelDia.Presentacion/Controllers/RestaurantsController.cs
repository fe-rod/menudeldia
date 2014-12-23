using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Entities.Enums;
using MenuDelDia.Presentacion.Authorize;
using MenuDelDia.Presentacion.Helpers;
using MenuDelDia.Presentacion.Models;
using Microsoft.AspNet.Identity;

namespace MenuDelDia.Presentacion.Controllers
{

    public class RestaurantsController : BaseController
    {

        #region Private Methods
        private bool ValidateFile(HttpPostedFileBase file)
        {
            if (file.ContentType != "image/png" &&
                file.ContentType != "image/gif" &&
                file.ContentType != "image/jpg" &&
                file.ContentType != "image/jpeg")
            {
                return false;
            }

            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();

            if (extension != ".png" &&
                extension != ".gif" &&
                extension != ".jpg" &&
                extension != ".jpeg")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save image to disk and return the filename and the extension
        /// </summary>
        /// <param name="file"></param>
        /// <returns>(filname,extension)</returns>
        private Tuple<string, string> SaveImage(HttpPostedFileBase file)
        {
            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
            var fileName = Guid.NewGuid().ToString();
            var path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["FolderLogos"]), string.Format("{0}{1}", fileName, extension));
            file.SaveAs(path);

            return new Tuple<string, string>(fileName, extension);
        }

        private void DeleteImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return;

            var path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["FolderLogos"]), imageName);

            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
                fileInfo.Delete();
        }

        private string FormatURL(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            url = url.ToLower();
            if (url.Contains("https://"))
            {
                url = url.Replace("https://", "");
                url = url.Replace("www.", "");
                return string.Format("https://www.{0}", url);
            }
            else
            {
                url = url.Replace("http://", "");
                url = url.Replace("www.", "");
                return string.Format("http://www.{0}", url);
            }
        }


        public IList<CardModel> LoadCards(IList<Guid> selectedCardIds = null)
        {
            if (selectedCardIds == null)
            {
                return CurrentAppContext.Cards.Select(c => new CardModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = (c.CardType == CardType.Credit ? "Crédito" : "Débito"),
                }).ToList();
            }

            return CurrentAppContext.Cards.Select(c => new CardModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = (c.CardType == CardType.Credit ? "Crédito" : "Débito"),
                Selected = selectedCardIds.Contains(c.Id),
            }).ToList();
        }


        public IList<TagModel> LoadTags(IList<Guid> selectedTags = null)
        {
            if (selectedTags == null)
            {
                return CurrentAppContext.Tags
                    .Where(t => t.ApplyToRestaurant)
                    .Select(t => new TagModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                    }).ToList();
            }

            return CurrentAppContext.Tags
                .Where(t => t.ApplyToRestaurant)
                .Select(t => new TagModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Selected = selectedTags.Contains(t.Id),
                }).ToList();
        }

        #endregion


        // GET: Restaurants
        //[Authorize(Roles = "Administrator")]
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(CurrentAppContext.Restaurants.Select(r => new RestaurantModel
            {
                Id = r.Id,
                Name = r.Name,
                Email = r.Email,
                Description = r.Description,
                LogoPath = r.LogoPath,
                Url = r.Url,
            }).ToList());
        }

        // GET: Restaurants/Details/5
        [CustomAuthorize(Roles = "Administrator")]
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = CurrentAppContext.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            if (await ValidateUserRequestWithUserLoggedIn(restaurant.Id) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Email = restaurant.Email,
                Description = restaurant.Description,
                LogoPath = restaurant.LogoPath,
                Url = restaurant.Url,
                Tags = LoadTags(restaurant.Tags.Select(t => t.Id).ToList()),
                Cards = LoadCards(restaurant.Cards.Select(c => c.Id).ToList()),
            });
        }

        // GET: Restaurants/Create
        [CustomAuthorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var model = new RestaurantModel
            {
                Cards = LoadCards(),
                Tags = LoadTags(),
            };
            return View(model);
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Administrator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Url,Cards,EmailUserName,Active,Tags")] RestaurantModel restaurant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var fileName = string.Empty;
                var fileExtension = string.Empty;

                if (file != null && file.ContentLength > 0)
                {
                    if (ValidateFile(file) == false)
                        return View(restaurant);

                    var dataTuple = SaveImage(file);
                    fileName = dataTuple.Item1;
                    fileExtension = dataTuple.Item2;
                }

                if (await ValidateUserName(restaurant.EmailUserName) == false)
                {
                    ModelState.AddModelError("EmailUserName", "El nombre de usuario ingresado ya se encuentra registrado en el sistema.");
                    return View(restaurant);
                }

                var selectedCards = restaurant.Cards.Where(c => c.Selected).Select(c => c.Id).ToList();
                var selectedTags = restaurant.Tags.Where(t => t.Selected).Select(t => t.Id).ToList();

                var entityCards = CurrentAppContext.Cards.Where(c => selectedCards.Contains(c.Id)).ToList();
                var entityTags = CurrentAppContext.Tags.Where(t => selectedTags.Contains(t.Id)).ToList();

                var entityRestaurant = new Restaurant
                {
                    Id = Guid.NewGuid(),
                    Name = restaurant.Name,
                    Email = restaurant.Email,
                    Description = restaurant.Description,
                    LogoPath = string.Format("{0}{1}", fileName, fileExtension),
                    LogoExtension = fileExtension,
                    LogoName = fileName,
                    Url = FormatURL(restaurant.Url),
                    Active = restaurant.Active,
                };

                entityCards.ForEach(c => entityRestaurant.Cards.Add(c));
                entityTags.ForEach(t => entityRestaurant.Tags.Add(t));

                CurrentAppContext.Restaurants.Add(entityRestaurant);
                CurrentAppContext.SaveChanges();

                await CreateRestaurantUser(restaurant.EmailUserName, entityRestaurant.Id);

                return RedirectToAction("Index");

            }
            restaurant.Cards = CurrentAppContext.Cards.Select(c => new CardModel { Id = c.Id, Name = c.Name }).ToList();
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        [CustomAuthorize(Roles = "Administrator,User")]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                id = (await CurrentUser()).RestaurantId;

                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = CurrentAppContext.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }

            if (await ValidateUserRequestWithUserLoggedIn(restaurant.Id) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cardsIds = restaurant.Cards.Select(c => c.Id).ToList();
            var tagsIds = restaurant.Tags.Select(t => t.Id).ToList();

            return View(new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Email = restaurant.Email,
                Description = restaurant.Description,
                LogoPath = restaurant.LogoPath,
                Url = restaurant.Url,
                Cards = LoadCards(cardsIds),
                Tags = LoadTags(tagsIds),
            });
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Administrator,User")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,LogoPath,Description,Url,ClearLogoPath,Cards,Active,Tags")] RestaurantModel restaurant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var restaurantEntity = CurrentAppContext.Restaurants.FirstOrDefault(r => r.Id == restaurant.Id);
                if (restaurantEntity != null)
                {
                    if (await ValidateUserRequestWithUserLoggedIn(restaurantEntity.Id) == false)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    var fileModified = false;
                    if (file != null && file.ContentLength > 0)
                    {
                        if (ValidateFile(file) == false)
                            return View(restaurant);

                        DeleteImage(restaurantEntity.LogoPath);

                        var dataTuple = SaveImage(file);
                        restaurantEntity.LogoName= dataTuple.Item1;
                        restaurantEntity.LogoExtension = dataTuple.Item2;
                        restaurantEntity.LogoPath = string.Format("{0}{1}", dataTuple.Item1, dataTuple.Item2);
                        fileModified = true;
                    }

                    if (fileModified == false && (restaurant.ClearLogoPath && string.IsNullOrEmpty(restaurantEntity.LogoPath) == false))
                    {
                        DeleteImage(restaurantEntity.LogoPath);
                        restaurantEntity.LogoPath = string.Empty;
                    }

                    restaurantEntity.Name = restaurant.Name;
                    restaurantEntity.Email = restaurant.Email;
                    restaurantEntity.Url = FormatURL(restaurant.Url);
                    restaurantEntity.Description = restaurant.Description;
                    restaurantEntity.Active = restaurant.Active;


                    var cardsIds = restaurant.Cards.Where(c => c.Selected).Select(c => c.Id).ToList();
                    var entityCards = CurrentAppContext.Cards.Where(c => cardsIds.Contains(c.Id)).ToList();

                    var selectedTags = restaurant.Tags.Where(t => t.Selected).Select(t => t.Id).ToList();
                    var entityTags = CurrentAppContext.Tags.Where(t => selectedTags.Contains(t.Id)).ToList();

                    restaurantEntity.Cards.Clear();
                    restaurantEntity.Tags.Clear();

                    entityCards.ForEach(c => restaurantEntity.Cards.Add(c));
                    entityTags.ForEach(t => restaurantEntity.Tags.Add(t));

                    CurrentAppContext.Entry(restaurantEntity).State = EntityState.Modified;
                    CurrentAppContext.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        [CustomAuthorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = CurrentAppContext.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            if (await ValidateUserRequestWithUserLoggedIn(restaurant.Id) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cardsIds = restaurant.Cards.Select(c => c.Id).ToList();
            var tagsIds = restaurant.Tags.Select(t => t.Id).ToList();

            return View(new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Email = restaurant.Email,
                Description = restaurant.Description,
                LogoPath = restaurant.LogoPath,
                Url = restaurant.Url,
                Cards = LoadCards(cardsIds),
                Tags = LoadTags(tagsIds),
            });
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Restaurant restaurant = CurrentAppContext.Restaurants.Find(id);

            if (await ValidateUserRequestWithUserLoggedIn(restaurant.Id) == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CurrentAppContext.Restaurants.Remove(restaurant);
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

        private async Task<bool> CreateRestaurantUser(string emailUserName, Guid restaurantId)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = emailUserName, Email = emailUserName, RestaurantId = restaurantId };

                var password = StringHelper.GenerateRandomString(6);

                var result = await UserManager.CreateAsync(user, password);
                UserManager.AddToRole(user.Id, "User");

                if (result.Succeeded)   
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return true;
                }
            }
            return false;
        }

        private async Task<bool> ValidateUserName(string emailUserName)
        {
            return (await UserManager.FindByEmailAsync(emailUserName)) == null;
        }

    }
}
