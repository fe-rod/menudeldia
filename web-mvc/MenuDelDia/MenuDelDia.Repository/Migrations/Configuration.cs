using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MenuDelDia.Repository.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MenuDelDia.Repository.AppContext context)
        {


            context.Roles.AddOrUpdate(p => p.Name, new IdentityRole { Name = "Administrator" });
            context.Roles.AddOrUpdate(p => p.Name, new IdentityRole { Name = "User" });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
