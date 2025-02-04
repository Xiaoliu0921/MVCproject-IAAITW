namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        CurrentCompany = c.String(),
                        Position = c.String(),
                        Permission = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CertifiedMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        Company = c.String(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Experts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CurrentPosition = c.String(nullable: false),
                        Education = c.String(nullable: false),
                        Introduction = c.String(nullable: false),
                        Experience = c.String(nullable: false),
                        Expertise = c.String(nullable: false),
                        AppraisalExperience = c.String(nullable: false),
                        RelatedResearch = c.String(nullable: false),
                        AcademicPublications = c.String(nullable: false),
                        ResearchProjects = c.String(nullable: false),
                        TrainingOrCertifications = c.String(nullable: false),
                        DetailedExperience = c.String(nullable: false),
                        ConcurrentPublicSector = c.String(nullable: false),
                        ConcurrentLegalAgency = c.String(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        MemberId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        MemberType = c.Int(),
                        ApplyingMemberType = c.Int(),
                        PublicContactPhoneNumber = c.String(),
                        PhoneNumber = c.String(nullable: false),
                        ContactAddress = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        IsInternationalMembership = c.Boolean(nullable: false),
                        MembershipCertificateUrl = c.String(),
                        CurrentCompany = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        HighestEducation = c.String(nullable: false),
                        ServiceUnit1 = c.String(),
                        ServiceUnit1Position = c.String(),
                        ServiceUnit1StartDate = c.DateTime(),
                        ServiceUnit1EndDate = c.DateTime(),
                        ServiceUnit2 = c.String(),
                        ServiceUnit2Position = c.String(),
                        ServiceUnit2StartDate = c.DateTime(),
                        ServiceUnit2EndDate = c.DateTime(),
                        ServiceUnit3 = c.String(),
                        ServiceUnit3Position = c.String(),
                        ServiceUnit3StartDate = c.DateTime(),
                        ServiceUnit3EndDate = c.DateTime(),
                        TotalExperienceYears = c.Int(nullable: false),
                        TotalExperienceMonths = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ForumReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        ForumPostId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        Member_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumPosts", t => t.ForumPostId)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.Members", t => t.Member_Id)
                .Index(t => t.ForumPostId)
                .Index(t => t.MemberId)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.Knowledges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        FilePath = c.String(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ImageUrl = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        IsTop = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumReplies", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.ForumReplies", "MemberId", "dbo.Members");
            DropForeignKey("dbo.ForumReplies", "ForumPostId", "dbo.ForumPosts");
            DropForeignKey("dbo.ForumPosts", "MemberId", "dbo.Members");
            DropIndex("dbo.ForumReplies", new[] { "Member_Id" });
            DropIndex("dbo.ForumReplies", new[] { "MemberId" });
            DropIndex("dbo.ForumReplies", new[] { "ForumPostId" });
            DropIndex("dbo.ForumPosts", new[] { "MemberId" });
            DropTable("dbo.News");
            DropTable("dbo.Knowledges");
            DropTable("dbo.ForumReplies");
            DropTable("dbo.Members");
            DropTable("dbo.ForumPosts");
            DropTable("dbo.Experts");
            DropTable("dbo.CertifiedMembers");
            DropTable("dbo.Admins");
        }
    }
}
