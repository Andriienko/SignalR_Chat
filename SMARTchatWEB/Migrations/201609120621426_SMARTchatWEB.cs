namespace SMARTchatWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SMARTchatWEB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        ChannelId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ChannelId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false),
                        SendTime = c.DateTime(nullable: false),
                        ChannelId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Channels", t => t.ChannelId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ChannelId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApplicationUserChannels",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Channel_ChannelId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Channel_ChannelId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Channels", t => t.Channel_ChannelId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Channel_ChannelId);
            
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.AspNetUsers", "HomeTown");
            DropColumn("dbo.AspNetUsers", "BirthDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "HomeTown", c => c.String());
            DropForeignKey("dbo.Messages", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserChannels", "Channel_ChannelId", "dbo.Channels");
            DropForeignKey("dbo.ApplicationUserChannels", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ChannelId", "dbo.Channels");
            DropIndex("dbo.ApplicationUserChannels", new[] { "Channel_ChannelId" });
            DropIndex("dbo.ApplicationUserChannels", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "ChannelId" });
            DropColumn("dbo.AspNetUsers", "LastName");
            DropTable("dbo.ApplicationUserChannels");
            DropTable("dbo.Messages");
            DropTable("dbo.Channels");
        }
    }
}
