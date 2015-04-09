namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostTitleIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Title", title => title.String(maxLength: 50, nullable: false));
            AlterColumn("dbo.Posts", "Content", title => title.String(maxLength: 50, nullable: false));
            CreateIndex("dbo.Posts", "Title", unique: true);
            CreateIndex("dbo.Posts", "Content", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Posts", new[] { "Title" });
            DropIndex("dbo.Posts", new[] { "Content" });
        }
    }
}
