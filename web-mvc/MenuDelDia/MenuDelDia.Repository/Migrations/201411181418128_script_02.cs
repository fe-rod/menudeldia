namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RestaurantId", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "RestaurantId");
            AddForeignKey("dbo.AspNetUsers", "RestaurantId", "dbo.Restaurants", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.AspNetUsers", new[] { "RestaurantId" });
            DropColumn("dbo.AspNetUsers", "RestaurantId");
        }
    }
}
