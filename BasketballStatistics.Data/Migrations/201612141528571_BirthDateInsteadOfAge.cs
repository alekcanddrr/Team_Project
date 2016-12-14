namespace BasketballStatistics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BirthDateInsteadOfAge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "BirthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Players", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "Age", c => c.Int(nullable: false));
            DropColumn("dbo.Players", "BirthDate");
        }
    }
}
