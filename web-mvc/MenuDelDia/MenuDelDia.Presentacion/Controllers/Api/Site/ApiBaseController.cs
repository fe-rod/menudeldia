﻿using System;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Owin;
using MenuDelDia.Entities;
using MenuDelDia.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MenuDelDia.Presentacion.Controllers.Api.Site
{
    public class ApiBaseController : ApiController
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
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        public async Task<ApplicationUser> CurrentUser()
        {
            return await UserManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
        }


        public async Task<bool> ValidateUserRequestWithUserLoggedIn(Guid requestRestaurantId)
        {
            var currentUser = await CurrentUser();

            if (currentUser == null) return false;

            if (currentUser.RestaurantId.HasValue == false) return true;

            return currentUser.RestaurantId == requestRestaurantId;
        }
        
        /// <summary>
        /// Return userId based on current claims.
        /// </summary> 
        /// <returns>UserId</returns>
        /// <exception cref="FatalException">User must be authenticated.</exception>
        public Guid CurrentUserId
        {
            get
            {
                var userClaim = ClaimsPrincipal.Current;

                if (userClaim.Identity.IsAuthenticated == false)
                    throw new ApplicationException("User must be authenticated.");

                var userIdClaim = userClaim.Claims.First(c => c.Type == "id");
                return Guid.Parse(userIdClaim.Value);
            }
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                                     "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }
    }
}

