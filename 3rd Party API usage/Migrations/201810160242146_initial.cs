namespace _3rd_Party_API_usage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Value = c.String(),
                        Suit = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Decks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Deck_ID = c.String(),
                        Remaining = c.Int(nullable: false),
                        LastCardID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cards", t => t.LastCardID)
                .Index(t => t.LastCardID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Decks", "LastCardID", "dbo.Cards");
            DropIndex("dbo.Decks", new[] { "LastCardID" });
            DropTable("dbo.Decks");
            DropTable("dbo.Cards");
        }
    }
}
