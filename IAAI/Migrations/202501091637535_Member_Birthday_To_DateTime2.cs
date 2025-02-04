namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Member_Birthday_To_DateTime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Birthday", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Birthday", c => c.DateTime(nullable: false));
        }
    }
}
