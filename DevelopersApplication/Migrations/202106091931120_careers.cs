namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class careers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        CareerId = c.Int(nullable: false, identity: true),
                        CareerName = c.String(),
                        CareerDesc = c.String(),
                    })
                .PrimaryKey(t => t.CareerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Careers");
        }
    }
}
