namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.CommentLikers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CommentId = c.Int(nullable: false),
                        IsLiked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CommentId })
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CommentId);

            this.CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ParentCommentId = c.Int(),
                        Content = c.String(),
                        Likes = c.Int(nullable: false),
                        ReplyCommentsCount = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        IsReadByAuthor = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.ParentCommentId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ParentCommentId)
                .Index(t => t.PostId);

            this.CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Content = c.String(),
                        Category = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Likes = c.Int(nullable: false),
                        CommentsCount = c.Int(nullable: false),
                        TimesRead = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.Title, unique: true, name: "UniqueTitle")
                .Index(t => t.UserId);

            this.CreateTable(
                "dbo.PostLikers",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IsLiked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.UserId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);

            this.CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ImageUrl = c.String(),
                        Points = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            this.CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            this.CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            this.CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Url = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);

            this.CreateTable(
                "dbo.PostReaders",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PostId, t.UserId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);

            this.CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            this.CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            this.CreateTable(
                "dbo.TagPosts",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Post_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Post_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Post_Id);            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            this.DropForeignKey("dbo.TagPosts", "Post_Id", "dbo.Posts");
            this.DropForeignKey("dbo.TagPosts", "Tag_Id", "dbo.Tags");
            this.DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.PostReaders", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.PostReaders", "PostId", "dbo.Posts");
            this.DropForeignKey("dbo.PostLikers", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Logs", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.CommentLikers", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.PostLikers", "PostId", "dbo.Posts");
            this.DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            this.DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            this.DropForeignKey("dbo.CommentLikers", "CommentId", "dbo.Comments");
            this.DropIndex("dbo.TagPosts", new[] { "Post_Id" });
            this.DropIndex("dbo.TagPosts", new[] { "Tag_Id" });
            this.DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            this.DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            this.DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            this.DropIndex("dbo.PostReaders", new[] { "UserId" });
            this.DropIndex("dbo.PostReaders", new[] { "PostId" });
            this.DropIndex("dbo.Logs", new[] { "UserId" });
            this.DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            this.DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            this.DropIndex("dbo.AspNetUsers", "UserNameIndex");
            this.DropIndex("dbo.PostLikers", new[] { "UserId" });
            this.DropIndex("dbo.PostLikers", new[] { "PostId" });
            this.DropIndex("dbo.Posts", new[] { "UserId" });
            this.DropIndex("dbo.Posts", "UniqueTitle");
            this.DropIndex("dbo.Comments", new[] { "PostId" });
            this.DropIndex("dbo.Comments", new[] { "ParentCommentId" });
            this.DropIndex("dbo.Comments", new[] { "UserId" });
            this.DropIndex("dbo.CommentLikers", new[] { "CommentId" });
            this.DropIndex("dbo.CommentLikers", new[] { "UserId" });
            this.DropTable("dbo.TagPosts");
            this.DropTable("dbo.AspNetRoles");
            this.DropTable("dbo.Tags");
            this.DropTable("dbo.AspNetUserRoles");
            this.DropTable("dbo.PostReaders");
            this.DropTable("dbo.Logs");
            this.DropTable("dbo.AspNetUserLogins");
            this.DropTable("dbo.AspNetUserClaims");
            this.DropTable("dbo.AspNetUsers");
            this.DropTable("dbo.PostLikers");
            this.DropTable("dbo.Posts");
            this.DropTable("dbo.Comments");
            this.DropTable("dbo.CommentLikers");
        }
    }
}
