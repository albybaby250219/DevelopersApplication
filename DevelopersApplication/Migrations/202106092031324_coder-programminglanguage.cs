namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coderprogramminglanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgrammingLanguageCoders",
                c => new
                    {
                        ProgrammingLanguage_LanguageId = c.Int(nullable: false),
                        Coder_CoderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgrammingLanguage_LanguageId, t.Coder_CoderId })
                .ForeignKey("dbo.ProgrammingLanguages", t => t.ProgrammingLanguage_LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Coders", t => t.Coder_CoderId, cascadeDelete: true)
                .Index(t => t.ProgrammingLanguage_LanguageId)
                .Index(t => t.Coder_CoderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgrammingLanguageCoders", "Coder_CoderId", "dbo.Coders");
            DropForeignKey("dbo.ProgrammingLanguageCoders", "ProgrammingLanguage_LanguageId", "dbo.ProgrammingLanguages");
            DropIndex("dbo.ProgrammingLanguageCoders", new[] { "Coder_CoderId" });
            DropIndex("dbo.ProgrammingLanguageCoders", new[] { "ProgrammingLanguage_LanguageId" });
            DropTable("dbo.ProgrammingLanguageCoders");
        }
    }
}
