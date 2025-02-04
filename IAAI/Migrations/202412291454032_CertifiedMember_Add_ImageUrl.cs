namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CertifiedMember_Add_ImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertifiedMembers", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CertifiedMembers", "ImageUrl");
        }
    }
}
