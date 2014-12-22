namespace MenuDelDia.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script_17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Message = c.String(nullable: false),
                        CreationDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                        Ip = c.String(),
                        Uuid = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppComments");
        }
    }
}
