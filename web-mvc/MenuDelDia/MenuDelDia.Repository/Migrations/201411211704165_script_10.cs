namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Identifier", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Identifier");
        }
    }
}
