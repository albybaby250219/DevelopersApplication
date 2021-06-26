namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coderxpl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoderxProgrammingLanguages",
                c => new
                    {
                        CoderxPLId = c.Int(nullable: false, identity: true),
                        CoderId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        FavLang = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CoderxPLId)
                .ForeignKey("dbo.Coders", t => t.CoderId, cascadeDelete: true)
                .ForeignKey("dbo.ProgrammingLanguages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.CoderId)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoderxProgrammingLanguages", "LanguageId", "dbo.ProgrammingLanguages");
            DropForeignKey("dbo.CoderxProgrammingLanguages", "CoderId", "dbo.Coders");
            DropIndex("dbo.CoderxProgrammingLanguages", new[] { "LanguageId" });
            DropIndex("dbo.CoderxProgrammingLanguages", new[] { "CoderId" });
            DropTable("dbo.CoderxProgrammingLanguages");
        }
    }
}
