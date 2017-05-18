namespace WebEnvang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BW : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookInfo", "PaymentMethodId", c => c.Int(nullable: false));
            AddColumn("dbo.BookInfo", "PaymentMethodMessage", c => c.String());
            AddColumn("dbo.WebConfig", "KetQuaDatVe", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebConfig", "KetQuaDatVe");
            DropColumn("dbo.BookInfo", "PaymentMethodMessage");
            DropColumn("dbo.BookInfo", "PaymentMethodId");
        }
    }
}
