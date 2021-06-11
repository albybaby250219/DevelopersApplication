namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class codercareer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coders", "CareerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Coders", "CareerId");
            AddForeignKey("dbo.Coders", "CareerId", "dbo.Careers", "CareerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coders", "CareerId", "dbo.Careers");
            DropIndex("dbo.Coders", new[] { "CareerId" });
            DropColumn("dbo.Coders", "CareerId");
        }
    }
}
