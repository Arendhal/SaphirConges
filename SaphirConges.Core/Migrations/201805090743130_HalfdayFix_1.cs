namespace SaphirCongesCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HalfdayFix_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conges", "HalfDayEnd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Conges", "HalfDayEnd");
        }
    }
}
