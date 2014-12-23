namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "TagType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "TagType");
        }
    }
}
