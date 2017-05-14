namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PR : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageRole",
                c => new
                    {
                        PageId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PageId, t.RoleId });
            
            CreateTable(
                "dbo.Page",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RouterPath = c.String(),
                        Order = c.Int(nullable: false),
                        Group = c.String(),
                        GroupOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Page");
            DropTable("dbo.PageRole");
        }
    }
}
