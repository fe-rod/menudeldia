namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CardType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        LogoPath = c.String(nullable: false),
                        Description = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Streets = c.String(),
                        Phone = c.String(),
                        Description = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Delivery = c.Boolean(nullable: false),
                        Restaurant_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_Id)
                .Index(t => t.Restaurant_Id);
            
            CreateTable(
                "dbo.OpenDays",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        OpenHour = c.Int(nullable: false),
                        OpenMinutes = c.Int(nullable: false),
                        CloseHour = c.Int(nullable: false),
                        CloseMinutes = c.Int(nullable: false),
                        Location_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Ingredients = c.String(),
                        MenuDays_Monday = c.Boolean(nullable: false),
                        MenuDays_Tuesday = c.Boolean(nullable: false),
                        MenuDays_Wednesday = c.Boolean(nullable: false),
                        MenuDays_Thursday = c.Boolean(nullable: false),
                        MenuDays_Friday = c.Boolean(nullable: false),
                        MenuDays_Saturday = c.Boolean(nullable: false),
                        MenuDays_Sunday = c.Boolean(nullable: false),
                        MenuDays_Recurrent = c.Boolean(nullable: false),
                        SpecialDay_Date = c.DateTime(nullable: false),
                        SpecialDay_Recurrent = c.Boolean(nullable: false),
                        RestaurantId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        DateTimeUtc = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                        Message = c.String(),
                        MenuId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.MenuId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RestaurantCards",
                c => new
                    {
                        Restaurant_Id = c.Guid(nullable: false),
                        Card_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Restaurant_Id, t.Card_Id })
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_Id)
                .ForeignKey("dbo.Cards", t => t.Card_Id)
                .Index(t => t.Restaurant_Id)
                .Index(t => t.Card_Id);
            
            CreateTable(
                "dbo.TagLocations",
                c => new
                    {
                        Tag_Id = c.Guid(nullable: false),
                        Location_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Location_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Tag_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.MenuTags",
                c => new
                    {
                        Menu_Id = c.Guid(nullable: false),
                        Tag_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Menu_Id, t.Tag_Id })
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Menu_Id)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "dbo.TagRestaurants",
                c => new
                    {
                        Tag_Id = c.Guid(nullable: false),
                        Restaurant_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Restaurant_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_Id)
                .Index(t => t.Tag_Id)
                .Index(t => t.Restaurant_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Locations", "Restaurant_Id", "dbo.Restaurants");
            DropForeignKey("dbo.TagRestaurants", "Restaurant_Id", "dbo.Restaurants");
            DropForeignKey("dbo.TagRestaurants", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.MenuTags", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.MenuTags", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.Menus", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Comments", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.TagLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.TagLocations", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.OpenDays", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.RestaurantCards", "Card_Id", "dbo.Cards");
            DropForeignKey("dbo.RestaurantCards", "Restaurant_Id", "dbo.Restaurants");
            DropIndex("dbo.TagRestaurants", new[] { "Restaurant_Id" });
            DropIndex("dbo.TagRestaurants", new[] { "Tag_Id" });
            DropIndex("dbo.MenuTags", new[] { "Tag_Id" });
            DropIndex("dbo.MenuTags", new[] { "Menu_Id" });
            DropIndex("dbo.TagLocations", new[] { "Location_Id" });
            DropIndex("dbo.TagLocations", new[] { "Tag_Id" });
            DropIndex("dbo.RestaurantCards", new[] { "Card_Id" });
            DropIndex("dbo.RestaurantCards", new[] { "Restaurant_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Comments", new[] { "MenuId" });
            DropIndex("dbo.Menus", new[] { "RestaurantId" });
            DropIndex("dbo.OpenDays", new[] { "Location_Id" });
            DropIndex("dbo.Locations", new[] { "Restaurant_Id" });
            DropTable("dbo.TagRestaurants");
            DropTable("dbo.MenuTags");
            DropTable("dbo.TagLocations");
            DropTable("dbo.RestaurantCards");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Comments");
            DropTable("dbo.Menus");
            DropTable("dbo.Tags");
            DropTable("dbo.OpenDays");
            DropTable("dbo.Locations");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Cards");
        }
    }
}
