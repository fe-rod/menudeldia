namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "LogoExtension", c => c.String());
            AddColumn("dbo.Restaurants", "LogoName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restaurants", "LogoName");
            DropColumn("dbo.Restaurants", "LogoExtension");
        }
    }
}
