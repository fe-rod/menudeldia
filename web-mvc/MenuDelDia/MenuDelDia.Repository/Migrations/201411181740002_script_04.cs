namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_04 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cards", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "Streets", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Menus", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Menus", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Menus", "Description", c => c.String());
            AlterColumn("dbo.Menus", "Name", c => c.String());
            AlterColumn("dbo.Tags", "Name", c => c.String());
            AlterColumn("dbo.Locations", "Description", c => c.String());
            AlterColumn("dbo.Locations", "Phone", c => c.String());
            AlterColumn("dbo.Locations", "Streets", c => c.String());
            AlterColumn("dbo.Cards", "Name", c => c.String());
        }
    }
}
