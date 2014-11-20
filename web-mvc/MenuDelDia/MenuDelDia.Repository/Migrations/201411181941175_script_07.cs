namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_07 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Locations", name: "Restaurant_Id", newName: "RestaurantId");
            RenameIndex(table: "dbo.Locations", name: "IX_Restaurant_Id", newName: "IX_RestaurantId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Locations", name: "IX_RestaurantId", newName: "IX_Restaurant_Id");
            RenameColumn(table: "dbo.Locations", name: "RestaurantId", newName: "Restaurant_Id");
        }
    }
}
