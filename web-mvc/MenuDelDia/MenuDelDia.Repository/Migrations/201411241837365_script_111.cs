namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_111 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restaurants", "Active");
        }
    }
}
