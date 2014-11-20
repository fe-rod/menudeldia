namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Active");
        }
    }
}
