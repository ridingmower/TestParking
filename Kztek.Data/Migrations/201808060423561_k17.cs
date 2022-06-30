namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblActiveCard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Date = c.DateTime(),
                        CardNumber = c.String(),
                        Plate = c.String(),
                        OldExpireDate = c.DateTime(),
                        Days = c.Int(nullable: false),
                        NewExpireDate = c.DateTime(),
                        CardGroupID = c.String(),
                        CustomerGroupID = c.String(),
                        CustomerID = c.String(),
                        UserID = c.String(),
                        FeeLevel = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblActiveCard");
        }
    }
}
