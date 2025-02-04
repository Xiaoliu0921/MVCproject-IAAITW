namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminAndMember_Salt_Fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Admins", "PasswordSalt", c => c.String());
            AlterColumn("dbo.Members", "PasswordSalt", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "PasswordSalt", c => c.String(nullable: false));
            AlterColumn("dbo.Admins", "PasswordSalt", c => c.String(nullable: false));
        }
    }
}
