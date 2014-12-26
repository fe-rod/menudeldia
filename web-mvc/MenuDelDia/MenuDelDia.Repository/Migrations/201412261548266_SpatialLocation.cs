namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class SpatialLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "SpatialLocation", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "SpatialLocation");
        }
    }
}
