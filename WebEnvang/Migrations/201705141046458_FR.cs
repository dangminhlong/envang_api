namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FR : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlightRoute",
                c => new
                    {
                        SrcLocationId = c.Int(nullable: false),
                        DestLocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SrcLocationId, t.DestLocationId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FlightRoute");
        }
    }
}
