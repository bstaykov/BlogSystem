namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DialogsIsReadTable : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.ReadDialogs",
                c => new
                    {
                        DialogId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        DateRead = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.DialogId, t.UserId })
                .ForeignKey("dbo.Dialogs", t => t.DialogId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.DialogId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.ReadDialogs", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.ReadDialogs", "DialogId", "dbo.Dialogs");
            this.DropIndex("dbo.ReadDialogs", new[] { "UserId" });
            this.DropIndex("dbo.ReadDialogs", new[] { "DialogId" });
            this.DropTable("dbo.ReadDialogs");
        }
    }
}
