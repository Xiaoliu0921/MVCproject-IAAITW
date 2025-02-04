namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Experts_fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Experts", "Education", c => c.String());
            AlterColumn("dbo.Experts", "Introduction", c => c.String());
            AlterColumn("dbo.Experts", "Experience", c => c.String());
            AlterColumn("dbo.Experts", "Expertise", c => c.String());
            AlterColumn("dbo.Experts", "AppraisalExperience", c => c.String());
            AlterColumn("dbo.Experts", "AcademicPublications", c => c.String());
            AlterColumn("dbo.Experts", "ResearchProjects", c => c.String());
            AlterColumn("dbo.Experts", "TrainingOrCertifications", c => c.String());
            AlterColumn("dbo.Experts", "DetailedExperience", c => c.String());
            AlterColumn("dbo.Experts", "ConcurrentPublicSector", c => c.String());
            AlterColumn("dbo.Experts", "ConcurrentLegalAgency", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Experts", "ConcurrentLegalAgency", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "ConcurrentPublicSector", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "DetailedExperience", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "TrainingOrCertifications", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "ResearchProjects", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "AcademicPublications", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "AppraisalExperience", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "Expertise", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "Experience", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "Introduction", c => c.String(nullable: false));
            AlterColumn("dbo.Experts", "Education", c => c.String(nullable: false));
        }
    }
}
