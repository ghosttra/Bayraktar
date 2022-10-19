namespace GameEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DestroyedImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Units", "ImageDestroyed", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Units", "ImageDestroyed");
        }
    }
}
