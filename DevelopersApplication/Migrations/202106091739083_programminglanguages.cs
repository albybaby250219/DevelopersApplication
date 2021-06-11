namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programminglanguages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgrammingLanguages",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        LanguageInfo = c.String(),
                        IDEUsed = c.String(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProgrammingLanguages");
        }
    }
}
