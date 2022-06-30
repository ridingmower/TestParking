namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuFunction", "isSystem", c => c.Boolean(nullable: false));
            AddColumn("dbo.MenuFunction", "MenuGroupListId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuFunction", "MenuGroupListId");
            DropColumn("dbo.MenuFunction", "isSystem");
        }
    }
}
