namespace BasketballStatistics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommandStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Points = c.Int(nullable: false),
                        Assists = c.Int(nullable: false),
                        Rebounds = c.Int(nullable: false),
                        Steals = c.Int(nullable: false),
                        BlockedShots = c.Int(nullable: false),
                        ShotsFromGame = c.Int(nullable: false),
                        ShotsFromGameSuccessfull = c.Int(nullable: false),
                        ShotsFromGameFar = c.Int(nullable: false),
                        ShotsFromGameFarSuccessfull = c.Int(nullable: false),
                        Match_Id = c.Int(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Matches", t => t.Match_Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Match_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Place = c.String(),
                        FinalScore = c.String(),
                        Team1_Id = c.Int(),
                        Team2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team1_Id)
                .ForeignKey("dbo.Teams", t => t.Team2_Id)
                .Index(t => t.Team1_Id)
                .Index(t => t.Team2_Id);
            
            CreateTable(
                "dbo.PersonalStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Points = c.Int(nullable: false),
                        Assists = c.Int(nullable: false),
                        Rebounds = c.Int(nullable: false),
                        Steals = c.Int(nullable: false),
                        BlockedShots = c.Int(nullable: false),
                        ShotsFromGame = c.Int(nullable: false),
                        ShotsFromGameSuccessfull = c.Int(nullable: false),
                        ShotsFromGameFar = c.Int(nullable: false),
                        ShotsFromGameFarSuccessfull = c.Int(nullable: false),
                        Match_Id = c.Int(),
                        Player_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Matches", t => t.Match_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .Index(t => t.Match_Id)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Height = c.Double(nullable: false),
                        Mass = c.Double(nullable: false),
                        Age = c.Int(nullable: false),
                        Position = c.String(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonalStatistics", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Players", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.PersonalStatistics", "Match_Id", "dbo.Matches");
            DropForeignKey("dbo.CommandStatistics", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.CommandStatistics", "Match_Id", "dbo.Matches");
            DropForeignKey("dbo.Matches", "Team2_Id", "dbo.Teams");
            DropForeignKey("dbo.Matches", "Team1_Id", "dbo.Teams");
            DropForeignKey("dbo.Coaches", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_Id" });
            DropIndex("dbo.PersonalStatistics", new[] { "Player_Id" });
            DropIndex("dbo.PersonalStatistics", new[] { "Match_Id" });
            DropIndex("dbo.Matches", new[] { "Team2_Id" });
            DropIndex("dbo.Matches", new[] { "Team1_Id" });
            DropIndex("dbo.CommandStatistics", new[] { "Team_Id" });
            DropIndex("dbo.CommandStatistics", new[] { "Match_Id" });
            DropIndex("dbo.Coaches", new[] { "Team_Id" });
            DropTable("dbo.Players");
            DropTable("dbo.PersonalStatistics");
            DropTable("dbo.Matches");
            DropTable("dbo.CommandStatistics");
            DropTable("dbo.Teams");
            DropTable("dbo.Coaches");
        }
    }
}
