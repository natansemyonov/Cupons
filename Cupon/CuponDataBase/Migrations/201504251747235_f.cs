namespace CuponDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Preferences", "Category", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Preferences", "Category");
        }
    }
}
