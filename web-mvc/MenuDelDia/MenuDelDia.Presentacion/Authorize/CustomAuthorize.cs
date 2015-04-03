using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MenuDelDia.Entities;
using MenuDelDia.Presentacion.Identity;
using MenuDelDia.Repository;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace MenuDelDia.Presentacion.Authorize
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        private readonly AppContext _context = new AppContext();
        private ApplicationUserManager _userManager = new ApplicationUserManager(new AppUserStore(new AppContext()));
        

        public CustomAuthorize(params string[] roles)
        {
            allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated == false)
                return false;

            //var usr = _userManager.FindByEmail(httpContext.User.Identity.Name);
            var usr = _context.Users.Where(u => u.Email == httpContext.User.Identity.Name).Select(u => new { u.UserName, u.RestaurantId }).FirstOrDefault();

            if (usr == null)
                return false;

            if (usr.RestaurantId != null)
            {
                var active = _context.Restaurants.Where(r => r.Id == usr.RestaurantId).Select(r => r.Active).FirstOrDefault();
                if (active == false)
                    return false;
            }

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

    }
}