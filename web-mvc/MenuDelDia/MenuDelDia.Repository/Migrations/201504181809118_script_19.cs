namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Zone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Zone");
        }
    }
}
