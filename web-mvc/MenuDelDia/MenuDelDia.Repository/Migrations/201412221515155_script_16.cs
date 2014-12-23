namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suggestions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Message = c.String(nullable: false),
                        CreationDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                        Ip = c.String(),
                        Uuid = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Comments", "DateTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Comments", "DateTimeUtc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "DateTimeUtc", c => c.DateTime(nullable: false));
            DropColumn("dbo.Comments", "DateTime");
            DropTable("dbo.Suggestions");
        }
    }
}
