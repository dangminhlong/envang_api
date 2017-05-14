namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeatureArticleConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Style = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FeatureArticleConfig");
        }
    }
}
