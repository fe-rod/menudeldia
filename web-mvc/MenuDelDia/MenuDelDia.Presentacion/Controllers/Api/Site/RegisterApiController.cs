using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Helpers;
using MenuDelDia.Presentacion.Models;
using MenuDelDia.Presentacion.Models.ApiModels.site;
using Microsoft.AspNet.Identity;

namespace MenuDelDia.Presentacion.Controllers.Api.Site
{
    public class RegisterApiController : ApiBaseController
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CurrentAppContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<bool> CreateRestaurantUser(string emailUserName, string password, Guid restaurantId)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = emailUserName, Email = emailUserName, RestaurantId = restaurantId };

                var result = await UserManager.CreateAsync(user, password);
                UserManager.AddToRole(user.Id, "User");

                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return true;
                }
            }
            return false;
        }

        private async Task<bool> ValidateUserName(string emailUserName)
        {
            return (await UserManager.FindByEmailAsync(emailUserName)) == null;
        }
        private static string FormatUrl(string url)
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




        [HttpPost]
        [Route("api/site/register")]
        public async Task<HttpResponseMessage> Register([FromBody] RegisterApiModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (await ValidateUserName(model.EmailUserName) == false)
                    {
                        ModelState.AddModelError("EmailUserName", "El nombre de usuario ingresado ya se encuentra registrado en el sistema.");
                    }

                    var entityCards = CurrentAppContext.Cards.Where(c => model.Cards.Contains(c.Id)).ToList();
                    var entityTags = CurrentAppContext.Tags.Where(t => model.Tags.Contains(t.Id)).ToList();

                    var entityRestaurant = new Restaurant
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        Email = model.Email,
                        Description = model.Description,
                        //LogoPath = string.Format("{0}{1}", fileName, fileExtension),
                        //LogoExtension = fileExtension,
                        //LogoName = fileName,
                        Url = FormatUrl(model.Url),
                        Active = true,
                    };

                    entityCards.ForEach(c => entityRestaurant.Cards.Add(c));
                    entityTags.ForEach(t => entityRestaurant.Tags.Add(t));

                    CurrentAppContext.Restaurants.Add(entityRestaurant);
                    CurrentAppContext.SaveChanges();

                    await CreateRestaurantUser(model.EmailUserName, model.Password, entityRestaurant.Id);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/site/updateregister")]
        public async Task<HttpResponseMessage> UpdateRegister([FromBody] UpdateRegisterApiModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entityCards = CurrentAppContext.Cards.Where(c => model.Cards.Contains(c.Id)).ToList();
                    var entityTags = CurrentAppContext.Tags.Where(t => model.Tags.Contains(t.Id)).ToList();

                    var entityRestaurant = new Restaurant
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        Email = model.Email,
                        Description = model.Description,
                        //LogoPath = string.Format("{0}{1}", fileName, fileExtension),
                        //LogoExtension = fileExtension,
                        //LogoName = fileName,
                        Url = FormatUrl(model.Url),
                        Active = true,
                    };

                    entityCards.ForEach(c => entityRestaurant.Cards.Add(c));
                    entityTags.ForEach(t => entityRestaurant.Tags.Add(t));

                    CurrentAppContext.Restaurants.Add(entityRestaurant);
                    CurrentAppContext.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
