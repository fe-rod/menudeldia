using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MenuDelDia.Presentacion.Controllers
{
    public class BaseController : Controller
    {
        private AppContext _db = new AppContext();
        private ApplicationUserManager _userManager;

        public AppContext CurrentAppContext
        {
            get
            {
                if (_db == null)
                    _db = new AppContext();

                return _db;
            }
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

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        public async Task<ApplicationUser> CurrentUser()
        {
            return await UserManager.FindByIdAsync(User.Identity.GetUserId());
        }


        public async Task<bool> ValidateUserRequestWithUserLoggedIn(Guid requestRestaurantId)
        {
            var currentUser = await CurrentUser();

            if (currentUser == null) return false;

            if (currentUser.RestaurantId.HasValue == false) return true;

            return currentUser.RestaurantId == requestRestaurantId;
        }
    }
}
