namespace CuponDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class all3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PurchasedCupons", name: "BussinessCupon_BussinessCuponId", newName: "Bussiness_Cupon_BussinessCuponId");
            RenameColumn(table: "dbo.Bussinesses", name: "BussinessOwner_BussinessOwnerId", newName: "Owner_BussinessOwnerId");
            RenameIndex(table: "dbo.PurchasedCupons", name: "IX_BussinessCupon_BussinessCuponId", newName: "IX_Bussiness_Cupon_BussinessCuponId");
            RenameIndex(table: "dbo.Bussinesses", name: "IX_BussinessOwner_BussinessOwnerId", newName: "IX_Owner_BussinessOwnerId");
            AddColumn("dbo.BussinessCupons", "Bussiness_BussinessId", c => c.Int());
            AddColumn("dbo.Preferences", "Basic_User_BasicUserId", c => c.Int());
            AddColumn("dbo.SocialNetworkCupons", "User_BasicUserId", c => c.Int());
            CreateIndex("dbo.BussinessCupons", "Bussiness_BussinessId");
            CreateIndex("dbo.Preferences", "Basic_User_BasicUserId");
            CreateIndex("dbo.SocialNetworkCupons", "User_BasicUserId");
            AddForeignKey("dbo.BussinessCupons", "Bussiness_BussinessId", "dbo.Bussinesses", "BussinessId");
            AddForeignKey("dbo.Preferences", "Basic_User_BasicUserId", "dbo.BasicUsers", "BasicUserId");
            AddForeignKey("dbo.SocialNetworkCupons", "User_BasicUserId", "dbo.BasicUsers", "BasicUserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialNetworkCupons", "User_BasicUserId", "dbo.BasicUsers");
            DropForeignKey("dbo.Preferences", "Basic_User_BasicUserId", "dbo.BasicUsers");
            DropForeignKey("dbo.BussinessCupons", "Bussiness_BussinessId", "dbo.Bussinesses");
            DropIndex("dbo.SocialNetworkCupons", new[] { "User_BasicUserId" });
            DropIndex("dbo.Preferences", new[] { "Basic_User_BasicUserId" });
            DropIndex("dbo.BussinessCupons", new[] { "Bussiness_BussinessId" });
            DropColumn("dbo.SocialNetworkCupons", "User_BasicUserId");
            DropColumn("dbo.Preferences", "Basic_User_BasicUserId");
            DropColumn("dbo.BussinessCupons", "Bussiness_BussinessId");
            RenameIndex(table: "dbo.Bussinesses", name: "IX_Owner_BussinessOwnerId", newName: "IX_BussinessOwner_BussinessOwnerId");
            RenameIndex(table: "dbo.PurchasedCupons", name: "IX_Bussiness_Cupon_BussinessCuponId", newName: "IX_BussinessCupon_BussinessCuponId");
            RenameColumn(table: "dbo.Bussinesses", name: "Owner_BussinessOwnerId", newName: "BussinessOwner_BussinessOwnerId");
            RenameColumn(table: "dbo.PurchasedCupons", name: "Bussiness_Cupon_BussinessCuponId", newName: "BussinessCupon_BussinessCuponId");
        }
    }
}
