namespace SaphirCongesCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoOfDaysFix3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Conges", "NoOfDays", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Conges", "NoOfDays", c => c.Double(nullable: false));
        }
    }
}
