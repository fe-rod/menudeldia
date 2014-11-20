namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_05 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Menus", "SpecialDay_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Menus", "SpecialDay_Date", c => c.DateTime(nullable: false));
        }
    }
}
