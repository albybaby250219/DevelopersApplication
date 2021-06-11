namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programminglanguagecareer : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProgrammingLanguageCoders", newName: "CoderProgrammingLanguages");
            DropPrimaryKey("dbo.CoderProgrammingLanguages");
            CreateTable(
                "dbo.ProgrammingLanguageCareers",
                c => new
                    {
                        ProgrammingLanguage_LanguageId = c.Int(nullable: false),
                        Career_CareerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgrammingLanguage_LanguageId, t.Career_CareerId })
                .ForeignKey("dbo.ProgrammingLanguages", t => t.ProgrammingLanguage_LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Careers", t => t.Career_CareerId, cascadeDelete: true)
                .Index(t => t.ProgrammingLanguage_LanguageId)
                .Index(t => t.Career_CareerId);
            
            AddPrimaryKey("dbo.CoderProgrammingLanguages", new[] { "Coder_CoderId", "ProgrammingLanguage_LanguageId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgrammingLanguageCareers", "Career_CareerId", "dbo.Careers");
            DropForeignKey("dbo.ProgrammingLanguageCareers", "ProgrammingLanguage_LanguageId", "dbo.ProgrammingLanguages");
            DropIndex("dbo.ProgrammingLanguageCareers", new[] { "Career_CareerId" });
            DropIndex("dbo.ProgrammingLanguageCareers", new[] { "ProgrammingLanguage_LanguageId" });
            DropPrimaryKey("dbo.CoderProgrammingLanguages");
            DropTable("dbo.ProgrammingLanguageCareers");
            AddPrimaryKey("dbo.CoderProgrammingLanguages", new[] { "ProgrammingLanguage_LanguageId", "Coder_CoderId" });
            RenameTable(name: "dbo.CoderProgrammingLanguages", newName: "ProgrammingLanguageCoders");
        }
    }
}
