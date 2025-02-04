namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expert_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "ViewCount", c => c.Int(nullable: false));
            DropColumn("dbo.Experts", "Education");
            DropColumn("dbo.Experts", "Introduction");
            DropColumn("dbo.Experts", "Experience");
            DropColumn("dbo.Experts", "Expertise");
            DropColumn("dbo.Experts", "AppraisalExperience");
            DropColumn("dbo.Experts", "RelatedResearch");
            DropColumn("dbo.Experts", "AcademicPublications");
            DropColumn("dbo.Experts", "ResearchProjects");
            DropColumn("dbo.Experts", "TrainingOrCertifications");
            DropColumn("dbo.Experts", "DetailedExperience");
            DropColumn("dbo.Experts", "ConcurrentPublicSector");
            DropColumn("dbo.Experts", "ConcurrentLegalAgency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Experts", "ConcurrentLegalAgency", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "ConcurrentPublicSector", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "DetailedExperience", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "TrainingOrCertifications", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "ResearchProjects", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "AcademicPublications", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "RelatedResearch", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "AppraisalExperience", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Expertise", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Experience", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Introduction", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Education", c => c.String(nullable: false));
            DropColumn("dbo.News", "ViewCount");
        }
    }
}
