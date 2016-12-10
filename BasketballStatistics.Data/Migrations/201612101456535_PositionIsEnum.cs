namespace BasketballStatistics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionIsEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Weight", c => c.Double(nullable: false));
            AlterColumn("dbo.Players", "Position", c => c.Int(nullable: false));
            DropColumn("dbo.Players", "Mass");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "Mass", c => c.Double(nullable: false));
            AlterColumn("dbo.Players", "Position", c => c.String());
            DropColumn("dbo.Players", "Weight");
        }
    }
}
