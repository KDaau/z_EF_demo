namespace BulkInsertDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "part.Prospects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "part.ProspectAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("part.Prospects", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("part.ProspectAddresses", "Id", "part.Prospects");
            DropIndex("part.ProspectAddresses", new[] { "Id" });
            DropTable("part.ProspectAddresses");
            DropTable("part.Prospects");
        }
    }
}
