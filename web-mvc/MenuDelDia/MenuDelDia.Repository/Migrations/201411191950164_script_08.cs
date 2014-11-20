namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_08 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MenuTags", newName: "TagMenus");
            DropForeignKey("dbo.Menus", "RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Menus", new[] { "RestaurantId" });
            DropPrimaryKey("dbo.TagMenus");
            CreateTable(
                "dbo.MenuLocations",
                c => new
                    {
                        Menu_Id = c.Guid(nullable: false),
                        Location_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Menu_Id, t.Location_Id })
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Menu_Id)
                .Index(t => t.Location_Id);
            
            AddColumn("dbo.Restaurants", "Email", c => c.String());
            AddPrimaryKey("dbo.TagMenus", new[] { "Tag_Id", "Menu_Id" });
            DropColumn("dbo.Menus", "RestaurantId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "RestaurantId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.MenuLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.MenuLocations", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.MenuLocations", new[] { "Location_Id" });
            DropIndex("dbo.MenuLocations", new[] { "Menu_Id" });
            DropPrimaryKey("dbo.TagMenus");
            DropColumn("dbo.Restaurants", "Email");
            DropTable("dbo.MenuLocations");
            AddPrimaryKey("dbo.TagMenus", new[] { "Menu_Id", "Tag_Id" });
            CreateIndex("dbo.Menus", "RestaurantId");
            AddForeignKey("dbo.Menus", "RestaurantId", "dbo.Restaurants", "Id");
            RenameTable(name: "dbo.TagMenus", newName: "MenuTags");
        }
    }
}
