namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Locations", new[] { "RestaurantId" });
            AlterColumn("dbo.Locations", "RestaurantId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Locations", "RestaurantId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Locations", new[] { "RestaurantId" });
            AlterColumn("dbo.Locations", "RestaurantId", c => c.Guid());
            CreateIndex("dbo.Locations", "RestaurantId");
        }
    }
}
