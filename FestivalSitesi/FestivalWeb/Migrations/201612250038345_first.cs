namespace FestivalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fests",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                        ShortDesc = c.String(),
                        Descrition = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        FestPicture = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserComments",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        FestID = c.Guid(nullable: false),
                        Comment = c.String(),
                        LikeCount = c.Int(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Fests", t => t.FestID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.FestID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                        Name = c.String(maxLength: 100),
                        Surname = c.String(maxLength: 100),
                        Email = c.String(maxLength: 250),
                        Password = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserGoings",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        FestID = c.Guid(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Fests", t => t.FestID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.FestID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGoings", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserGoings", "FestID", "dbo.Fests");
            DropForeignKey("dbo.UserComments", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserComments", "FestID", "dbo.Fests");
            DropIndex("dbo.UserGoings", new[] { "FestID" });
            DropIndex("dbo.UserGoings", new[] { "UserID" });
            DropIndex("dbo.UserComments", new[] { "FestID" });
            DropIndex("dbo.UserComments", new[] { "UserID" });
            DropTable("dbo.UserGoings");
            DropTable("dbo.Users");
            DropTable("dbo.UserComments");
            DropTable("dbo.Fests");
        }
    }
}
