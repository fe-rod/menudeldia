namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_03 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Menus", "MenuDays_Recurrent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "MenuDays_Recurrent", c => c.Boolean(nullable: false));
        }
    }
}
