namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "ApplyToRestaurant", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tags", "ApplyToLocation", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tags", "ApplyToMenu", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tags", "TagType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "TagType", c => c.Int(nullable: false));
            DropColumn("dbo.Tags", "ApplyToMenu");
            DropColumn("dbo.Tags", "ApplyToLocation");
            DropColumn("dbo.Tags", "ApplyToRestaurant");
        }
    }
}
