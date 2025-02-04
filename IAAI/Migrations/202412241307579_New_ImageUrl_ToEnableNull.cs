namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_ImageUrl_ToEnableNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "ImageUrl", c => c.String(nullable: false));
        }
    }
}
