namespace Cupon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class natan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        User_name = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Gender = c.String(),
                        Phone_Number = c.String(),
                        LastKnownLocation = c.String(),
                        Birth_Date = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        PreferenceId = c.Int(nullable: false, identity: true),
                        BasicUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PreferenceId)
                .ForeignKey("dbo.Users", t => t.BasicUser_UserId)
                .Index(t => t.BasicUser_UserId);
            
            CreateTable(
                "dbo.Cupons",
                c => new
                    {
                        CuponId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Original_Price = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Rate = c.Double(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        Category = c.String(),
                        Location = c.String(),
                        URL = c.String(),
                        PhoneNumber = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Bussiness_BussinessId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CuponId)
                .ForeignKey("dbo.Bussinesses", t => t.Bussiness_BussinessId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Bussiness_BussinessId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Bussinesses",
                c => new
                    {
                        BussinessId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        Description = c.String(),
                        Category = c.String(),
                        BussinessOwner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.BussinessId)
                .ForeignKey("dbo.Users", t => t.BussinessOwner_UserId)
                .Index(t => t.BussinessOwner_UserId);
            
            CreateTable(
                "dbo.PurchasedCupons",
                c => new
                    {
                        PurchasedCuponId = c.Int(nullable: false, identity: true),
                        Serial_Key = c.Int(nullable: false),
                        Cupon_State = c.String(),
                        Rate = c.Double(nullable: false),
                        BussinessCupon_CuponId = c.Int(),
                        CuponHistory_CuponHistoryId = c.Int(),
                    })
                .PrimaryKey(t => t.PurchasedCuponId)
                .ForeignKey("dbo.Cupons", t => t.BussinessCupon_CuponId)
                .ForeignKey("dbo.CuponHistories", t => t.CuponHistory_CuponHistoryId)
                .Index(t => t.BussinessCupon_CuponId)
                .Index(t => t.CuponHistory_CuponHistoryId);
            
            CreateTable(
                "dbo.CuponHistories",
                c => new
                    {
                        CuponHistoryId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.CuponHistoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cupons", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.PurchasedCupons", "CuponHistory_CuponHistoryId", "dbo.CuponHistories");
            DropForeignKey("dbo.Bussinesses", "BussinessOwner_UserId", "dbo.Users");
            DropForeignKey("dbo.Cupons", "Bussiness_BussinessId", "dbo.Bussinesses");
            DropForeignKey("dbo.PurchasedCupons", "BussinessCupon_CuponId", "dbo.Cupons");
            DropForeignKey("dbo.Preferences", "BasicUser_UserId", "dbo.Users");
            DropIndex("dbo.PurchasedCupons", new[] { "CuponHistory_CuponHistoryId" });
            DropIndex("dbo.PurchasedCupons", new[] { "BussinessCupon_CuponId" });
            DropIndex("dbo.Bussinesses", new[] { "BussinessOwner_UserId" });
            DropIndex("dbo.Cupons", new[] { "User_UserId" });
            DropIndex("dbo.Cupons", new[] { "Bussiness_BussinessId" });
            DropIndex("dbo.Preferences", new[] { "BasicUser_UserId" });
            DropTable("dbo.CuponHistories");
            DropTable("dbo.PurchasedCupons");
            DropTable("dbo.Bussinesses");
            DropTable("dbo.Cupons");
            DropTable("dbo.Preferences");
            DropTable("dbo.Users");
        }
    }
}
