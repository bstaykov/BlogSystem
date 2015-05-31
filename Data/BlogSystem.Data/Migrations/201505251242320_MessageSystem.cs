namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DialogId = c.Int(),
                        SenderId = c.String(maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        Content = c.String(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        SendOn = c.DateTime(nullable: false),
                        ReadOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.DialogId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.DialogId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Messages", "ReceiverId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Messages", "DialogId", "dbo.Messages");
            this.DropIndex("dbo.Messages", new[] { "ReceiverId" });
            this.DropIndex("dbo.Messages", new[] { "SenderId" });
            this.DropIndex("dbo.Messages", new[] { "DialogId" });
            this.DropTable("dbo.Messages");
        }
    }
}
