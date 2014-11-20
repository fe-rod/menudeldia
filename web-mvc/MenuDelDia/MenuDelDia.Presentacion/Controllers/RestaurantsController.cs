using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Entities.Enums;
using MenuDelDia.Presentacion.Models;
using MenuDelDia.Repository;
using Microsoft.SqlServer.Server;

namespace MenuDelDia.Presentacion.Controllers
{
    public class RestaurantsController : Controller
    {
        private AppContext db = new AppContext();

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
        /// Save image to disk and return the filename
        /// </summary>
        /// <param name="file"></param>
        /// <returns>FileName</returns>
        private string SaveImage(HttpPostedFileBase file)
        {
            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
            var fileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
            var path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["FolderLogos"]), fileName);
            file.SaveAs(path);

            return fileName;
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
        #endregion




        // GET: Restaurants
        public ActionResult Index()
        {
            return View(db.Restaurants.Select(r => new RestaurantModel
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
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Email = restaurant.Email,
                Description = restaurant.Description,
                LogoPath = restaurant.LogoPath,
                Url = restaurant.Url,
            });
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            var model = new RestaurantModel
            {
                Cards = db.Cards.Select(c => new CardModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = (c.CardType == CardType.Credit ? "Crédito" : "Débito")
                }).ToList()
            };
            return View(model);
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Url,Cards")] RestaurantModel restaurant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var fileName = string.Empty;
                if (file != null && file.ContentLength > 0)
                {
                    if (ValidateFile(file) == false)
                        return View(restaurant);

                    fileName = SaveImage(file);
                }


                var selectedCards = restaurant.Cards.Where(c => c.Selected).Select(c => c.Id).ToList();

                var entityCards = db.Cards.Where(c => selectedCards.Contains(c.Id)).ToList();
                var entityRestaurant = new Restaurant
                {
                    Id = Guid.NewGuid(),
                    Name = restaurant.Name,
                    Email = restaurant.Email,
                    Description = restaurant.Description,
                    LogoPath = fileName,
                    Url = FormatURL(restaurant.Url),
                };

                foreach (var entityCard in entityCards)
                {
                    entityRestaurant.Cards.Add(entityCard);
                }

                db.Restaurants.Add(entityRestaurant);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            restaurant.Cards = db.Cards.Select(c => new CardModel { Id = c.Id, Name = c.Name }).ToList();
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            var cardsIds = restaurant.Cards.Select(c => c.Id).ToList();

            var cards = db.Cards.Select(c => new CardModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = (c.CardType == CardType.Credit ? "Crédito" : "Débito"),
                Selected = cardsIds.Contains(c.Id),
            }).ToList();

            return View(new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Email = restaurant.Email,
                Description = restaurant.Description,
                LogoPath = restaurant.LogoPath,
                Url = restaurant.Url,
                Cards = cards
            });
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LogoPath,Description,Url,ClearLogoPath,Cards")] RestaurantModel restaurant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var restaurantEntity = db.Restaurants.FirstOrDefault(r => r.Id == restaurant.Id);
                if (restaurantEntity != null)
                {
                    var fileModified = false;
                    if (file != null && file.ContentLength > 0)
                    {
                        if (ValidateFile(file) == false)
                            return View(restaurant);

                        DeleteImage(restaurantEntity.LogoPath);
                        restaurantEntity.LogoPath = SaveImage(file);

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


                    var cardsIds = restaurant.Cards.Where(c => c.Selected).Select(c => c.Id).ToList();
                    var entityCards = db.Cards.Where(c => cardsIds.Contains(c.Id)).ToList();

                    restaurantEntity.Cards.Clear();

                    foreach (var entityCard in entityCards)
                    {
                        restaurantEntity.Cards.Add(entityCard);
                    }


                    db.Entry(restaurantEntity).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(new RestaurantModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Email = restaurant.Email,
                Description = restaurant.Description,
                LogoPath = restaurant.LogoPath,
                Url = restaurant.Url,
            });
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
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
