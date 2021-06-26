namespace DevelopersApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeddetailscoder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coders", "CoderHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Coders", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coders", "PicExtension");
            DropColumn("dbo.Coders", "CoderHasPic");
        }
    }
}
