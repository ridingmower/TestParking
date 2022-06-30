namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuFunctionConfig",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MenuFunctionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MenuFunctionConfig");
        }
    }
}
