namespace BulkInsertDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "part.Prospects", newName: "Users");
            RenameTable(name: "part.ProspectAddresses", newName: "UserAddresses");
            AlterColumn("part.Users", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("part.Users", "Surname", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("part.Users", "Surname", c => c.String());
            AlterColumn("part.Users", "Name", c => c.String());
            RenameTable(name: "part.UserAddresses", newName: "ProspectAddresses");
            RenameTable(name: "part.Users", newName: "Prospects");
        }
    }
}
