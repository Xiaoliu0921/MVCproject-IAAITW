namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expert_fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experts", "Education", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Introduction", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Experience", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "Expertise", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "AppraisalExperience", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "AcademicPublications", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "ResearchProjects", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "TrainingOrCertifications", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "DetailedExperience", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "ConcurrentPublicSector", c => c.String(nullable: false));
            AddColumn("dbo.Experts", "ConcurrentLegalAgency", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Experts", "ConcurrentLegalAgency");
            DropColumn("dbo.Experts", "ConcurrentPublicSector");
            DropColumn("dbo.Experts", "DetailedExperience");
            DropColumn("dbo.Experts", "TrainingOrCertifications");
            DropColumn("dbo.Experts", "ResearchProjects");
            DropColumn("dbo.Experts", "AcademicPublications");
            DropColumn("dbo.Experts", "AppraisalExperience");
            DropColumn("dbo.Experts", "Expertise");
            DropColumn("dbo.Experts", "Experience");
            DropColumn("dbo.Experts", "Introduction");
            DropColumn("dbo.Experts", "Education");
        }
    }
}
