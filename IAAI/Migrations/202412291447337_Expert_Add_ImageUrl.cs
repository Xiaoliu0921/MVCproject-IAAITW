namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expert_Add_ImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experts", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Experts", "ImageUrl");
        }
    }
}
