namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HotlineTop = c.String(),
                        HotlineFull = c.String(),
                        Email = c.String(),
                        MobileNumber = c.String(),
                        Country = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebConfig");
        }
    }
}
