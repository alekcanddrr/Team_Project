namespace BasketballStatistics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScoreIsInt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommandStatistics", "FreeThrows", c => c.Int(nullable: false));
            AddColumn("dbo.CommandStatistics", "FreeThrowsSuccessfull", c => c.Int(nullable: false));
            AddColumn("dbo.Matches", "Team1Score", c => c.Int(nullable: false));
            AddColumn("dbo.Matches", "Team2Score", c => c.Int(nullable: false));
            AddColumn("dbo.PersonalStatistics", "FreeThrows", c => c.Int(nullable: false));
            AddColumn("dbo.PersonalStatistics", "FreeThrowsSuccessfull", c => c.Int(nullable: false));
            DropColumn("dbo.Matches", "FinalScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Matches", "FinalScore", c => c.String());
            DropColumn("dbo.PersonalStatistics", "FreeThrowsSuccessfull");
            DropColumn("dbo.PersonalStatistics", "FreeThrows");
            DropColumn("dbo.Matches", "Team2Score");
            DropColumn("dbo.Matches", "Team1Score");
            DropColumn("dbo.CommandStatistics", "FreeThrowsSuccessfull");
            DropColumn("dbo.CommandStatistics", "FreeThrows");
        }
    }
}
