namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCardProcess",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        CardNumber = c.String(),
                        Actions = c.String(),
                        CardGroupID = c.String(),
                        CustomerID = c.String(),
                        UserID = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCardProcess");
        }
    }
}
