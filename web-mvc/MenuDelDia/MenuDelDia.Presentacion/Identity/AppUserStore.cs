using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MenuDelDia.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuDelDia.Presentacion.Identity
{
    public interface IAppUserStore : IUserStore<ApplicationUser, Guid> { }

    public class AppUserStore : UserStore<ApplicationUser, AppRole, Guid, AppUserLogin, AppUserRole, AppUserClaim>, IAppUserStore
    {
        public AppUserStore(DbContext context)
            : base(context)
        {

        }

        public override Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            return Users.Include(u => u.Roles)
                        .Include(u => u.Claims)
                        .Include(u => u.Logins)
                        .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}