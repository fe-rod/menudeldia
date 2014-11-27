namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Cost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Cost");
        }
    }
}
