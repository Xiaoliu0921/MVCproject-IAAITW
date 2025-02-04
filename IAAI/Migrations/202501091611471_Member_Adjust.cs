namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Member_Adjust : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "ServiceUnit1StartYear", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit1StartMonth", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit1EndYear", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit1EndMonth", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit2StartYear", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit2StartMonth", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit2EndYear", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit2EndMonth", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit3StartYear", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit3StartMonth", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit3EndYear", c => c.String());
            AddColumn("dbo.Members", "ServiceUnit3EndMonth", c => c.String());
            DropColumn("dbo.Members", "ServiceUnit1StartDate");
            DropColumn("dbo.Members", "ServiceUnit1EndDate");
            DropColumn("dbo.Members", "ServiceUnit2StartDate");
            DropColumn("dbo.Members", "ServiceUnit2EndDate");
            DropColumn("dbo.Members", "ServiceUnit3StartDate");
            DropColumn("dbo.Members", "ServiceUnit3EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "ServiceUnit3EndDate", c => c.DateTime());
            AddColumn("dbo.Members", "ServiceUnit3StartDate", c => c.DateTime());
            AddColumn("dbo.Members", "ServiceUnit2EndDate", c => c.DateTime());
            AddColumn("dbo.Members", "ServiceUnit2StartDate", c => c.DateTime());
            AddColumn("dbo.Members", "ServiceUnit1EndDate", c => c.DateTime());
            AddColumn("dbo.Members", "ServiceUnit1StartDate", c => c.DateTime());
            DropColumn("dbo.Members", "ServiceUnit3EndMonth");
            DropColumn("dbo.Members", "ServiceUnit3EndYear");
            DropColumn("dbo.Members", "ServiceUnit3StartMonth");
            DropColumn("dbo.Members", "ServiceUnit3StartYear");
            DropColumn("dbo.Members", "ServiceUnit2EndMonth");
            DropColumn("dbo.Members", "ServiceUnit2EndYear");
            DropColumn("dbo.Members", "ServiceUnit2StartMonth");
            DropColumn("dbo.Members", "ServiceUnit2StartYear");
            DropColumn("dbo.Members", "ServiceUnit1EndMonth");
            DropColumn("dbo.Members", "ServiceUnit1EndYear");
            DropColumn("dbo.Members", "ServiceUnit1StartMonth");
            DropColumn("dbo.Members", "ServiceUnit1StartYear");
        }
    }
}
