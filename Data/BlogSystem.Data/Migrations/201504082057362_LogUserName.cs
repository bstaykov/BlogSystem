namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogUserName : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Logs", new[] { "UserId" });
            AddColumn("dbo.Logs", "UserName", c => c.String());
            DropColumn("dbo.Logs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Logs", "UserName");
            CreateIndex("dbo.Logs", "UserId");
            AddForeignKey("dbo.Logs", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
