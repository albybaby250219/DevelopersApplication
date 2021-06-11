namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coders",
                c => new
                    {
                        CoderId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Bio = c.String(),
                        Company = c.String(),
                    })
                .PrimaryKey(t => t.CoderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Coders");
        }
    }
}
