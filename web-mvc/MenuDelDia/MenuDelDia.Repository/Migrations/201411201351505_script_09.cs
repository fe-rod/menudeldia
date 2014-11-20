namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_09 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Restaurants", "LogoPath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Restaurants", "LogoPath", c => c.String(nullable: false));
        }
    }
}
