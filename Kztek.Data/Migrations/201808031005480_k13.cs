namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFee",
                c => new
                    {
                        FeeID = c.Int(nullable: false, identity: true),
                        FeeName = c.String(),
                        CardGroupID = c.String(),
                        FeeLevel = c.Int(nullable: false),
                        Units = c.String(),
                    })
                .PrimaryKey(t => t.FeeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblFee");
        }
    }
}
