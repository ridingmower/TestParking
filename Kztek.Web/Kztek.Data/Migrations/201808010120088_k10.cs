namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSystemConfig",
                c => new
                    {
                        SystemConfigID = c.Guid(nullable: false),
                        Company = c.String(),
                        Address = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        FeeName = c.String(),
                        EnableDeleteCardFailed = c.Boolean(nullable: false),
                        SystemCode = c.String(),
                        KeyA = c.String(),
                        KeyB = c.String(),
                        SortOrder = c.Int(nullable: false),
                        EnableSoundAlarm = c.Boolean(nullable: false),
                        Logo = c.String(),
                        EnableAlarmMessageBox = c.Boolean(nullable: false),
                        EnableAlarmMessageBoxIn = c.Boolean(nullable: false),
                        Tax = c.String(),
                        DelayTime = c.Int(nullable: false),
                        Para1 = c.String(),
                        Para2 = c.String(),
                    })
                .PrimaryKey(t => t.SystemConfigID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblSystemConfig");
        }
    }
}
