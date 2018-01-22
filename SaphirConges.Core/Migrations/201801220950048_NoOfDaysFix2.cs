namespace SaphirCongesCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoOfDaysFix2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Conges", "Weekend");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conges", "Weekend", c => c.String());
        }
    }
}
