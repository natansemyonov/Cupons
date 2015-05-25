
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/23/2015 12:25:31
-- Generated from EDMX file: C:\Users\Ido\Documents\Cupons\CuponWebSite\CuponWebSite\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BussinessOwnerBussiness]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bussinesses] DROP CONSTRAINT [FK_BussinessOwnerBussiness];
GO
IF OBJECT_ID(N'[dbo].[FK_BussinessBussinessCupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cupons_BussinessCupon] DROP CONSTRAINT [FK_BussinessBussinessCupon];
GO
IF OBJECT_ID(N'[dbo].[FK_BasicUserPreference]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Preferences] DROP CONSTRAINT [FK_BasicUserPreference];
GO
IF OBJECT_ID(N'[dbo].[FK_BasicUserPurchasedCupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchasedCupons] DROP CONSTRAINT [FK_BasicUserPurchasedCupon];
GO
IF OBJECT_ID(N'[dbo].[FK_BussinessCuponPurchasedCupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchasedCupons] DROP CONSTRAINT [FK_BussinessCuponPurchasedCupon];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSocialNetworkCupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cupons_SocialNetworkCupon] DROP CONSTRAINT [FK_UserSocialNetworkCupon];
GO
IF OBJECT_ID(N'[dbo].[FK_BussinessOwner_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_BussinessOwner] DROP CONSTRAINT [FK_BussinessOwner_inherits_User];
GO
IF OBJECT_ID(N'[dbo].[FK_BussinessCupon_inherits_Cupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cupons_BussinessCupon] DROP CONSTRAINT [FK_BussinessCupon_inherits_Cupon];
GO
IF OBJECT_ID(N'[dbo].[FK_BasicUser_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_BasicUser] DROP CONSTRAINT [FK_BasicUser_inherits_User];
GO
IF OBJECT_ID(N'[dbo].[FK_SocialNetworkCupon_inherits_Cupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cupons_SocialNetworkCupon] DROP CONSTRAINT [FK_SocialNetworkCupon_inherits_Cupon];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemAdmin_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_SystemAdmin] DROP CONSTRAINT [FK_SystemAdmin_inherits_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Cupons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cupons];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Bussinesses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bussinesses];
GO
IF OBJECT_ID(N'[dbo].[Preferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferences];
GO
IF OBJECT_ID(N'[dbo].[PurchasedCupons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchasedCupons];
GO
IF OBJECT_ID(N'[dbo].[Users_BussinessOwner]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_BussinessOwner];
GO
IF OBJECT_ID(N'[dbo].[Cupons_BussinessCupon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cupons_BussinessCupon];
GO
IF OBJECT_ID(N'[dbo].[Users_BasicUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_BasicUser];
GO
IF OBJECT_ID(N'[dbo].[Cupons_SocialNetworkCupon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cupons_SocialNetworkCupon];
GO
IF OBJECT_ID(N'[dbo].[Users_SystemAdmin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_SystemAdmin];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cupons'
CREATE TABLE [dbo].[Cupons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Photo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Bussinesses'
CREATE TABLE [dbo].[Bussinesses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Category] int  NOT NULL,
    [Location_Longtitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Photo] nvarchar(max)  NOT NULL,
    [BussinessOwner_Id] int  NOT NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Category] int  NOT NULL,
    [BasicUser_Id] int  NOT NULL
);
GO

-- Creating table 'PurchasedCupons'
CREATE TABLE [dbo].[PurchasedCupons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SerialKey] nvarchar(max)  NOT NULL,
    [State] int  NOT NULL,
    [Rate] int  NOT NULL,
    [BasicUser_Id] int  NOT NULL,
    [BussinessCupon_Id] int  NOT NULL
);
GO

-- Creating table 'Users_BussinessOwner'
CREATE TABLE [dbo].[Users_BussinessOwner] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Cupons_BussinessCupon'
CREATE TABLE [dbo].[Cupons_BussinessCupon] (
    [Description] nvarchar(max)  NOT NULL,
    [OriginalPrice] float  NOT NULL,
    [Price] float  NOT NULL,
    [Rate] int  NOT NULL,
    [ExpirationDate] datetime  NOT NULL,
    [Category] int  NOT NULL,
    [Approved] bit  NOT NULL,
    [Location_Longtitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Photo] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL,
    [Bussiness_Id] int  NOT NULL
);
GO

-- Creating table 'Users_BasicUser'
CREATE TABLE [dbo].[Users_BasicUser] (
    [Gender] int  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [Location_Longtitude] float  NOT NULL,
    [Location_Latitude] float  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Cupons_SocialNetworkCupon'
CREATE TABLE [dbo].[Cupons_SocialNetworkCupon] (
    [URL] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'Users_SystemAdmin'
CREATE TABLE [dbo].[Users_SystemAdmin] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Cupons'
ALTER TABLE [dbo].[Cupons]
ADD CONSTRAINT [PK_Cupons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Bussinesses'
ALTER TABLE [dbo].[Bussinesses]
ADD CONSTRAINT [PK_Bussinesses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchasedCupons'
ALTER TABLE [dbo].[PurchasedCupons]
ADD CONSTRAINT [PK_PurchasedCupons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users_BussinessOwner'
ALTER TABLE [dbo].[Users_BussinessOwner]
ADD CONSTRAINT [PK_Users_BussinessOwner]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cupons_BussinessCupon'
ALTER TABLE [dbo].[Cupons_BussinessCupon]
ADD CONSTRAINT [PK_Cupons_BussinessCupon]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users_BasicUser'
ALTER TABLE [dbo].[Users_BasicUser]
ADD CONSTRAINT [PK_Users_BasicUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cupons_SocialNetworkCupon'
ALTER TABLE [dbo].[Cupons_SocialNetworkCupon]
ADD CONSTRAINT [PK_Cupons_SocialNetworkCupon]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users_SystemAdmin'
ALTER TABLE [dbo].[Users_SystemAdmin]
ADD CONSTRAINT [PK_Users_SystemAdmin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BussinessOwner_Id] in table 'Bussinesses'
ALTER TABLE [dbo].[Bussinesses]
ADD CONSTRAINT [FK_BussinessOwnerBussiness]
    FOREIGN KEY ([BussinessOwner_Id])
    REFERENCES [dbo].[Users_BussinessOwner]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BussinessOwnerBussiness'
CREATE INDEX [IX_FK_BussinessOwnerBussiness]
ON [dbo].[Bussinesses]
    ([BussinessOwner_Id]);
GO

-- Creating foreign key on [Bussiness_Id] in table 'Cupons_BussinessCupon'
ALTER TABLE [dbo].[Cupons_BussinessCupon]
ADD CONSTRAINT [FK_BussinessBussinessCupon]
    FOREIGN KEY ([Bussiness_Id])
    REFERENCES [dbo].[Bussinesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BussinessBussinessCupon'
CREATE INDEX [IX_FK_BussinessBussinessCupon]
ON [dbo].[Cupons_BussinessCupon]
    ([Bussiness_Id]);
GO

-- Creating foreign key on [BasicUser_Id] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [FK_BasicUserPreference]
    FOREIGN KEY ([BasicUser_Id])
    REFERENCES [dbo].[Users_BasicUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BasicUserPreference'
CREATE INDEX [IX_FK_BasicUserPreference]
ON [dbo].[Preferences]
    ([BasicUser_Id]);
GO

-- Creating foreign key on [BasicUser_Id] in table 'PurchasedCupons'
ALTER TABLE [dbo].[PurchasedCupons]
ADD CONSTRAINT [FK_BasicUserPurchasedCupon]
    FOREIGN KEY ([BasicUser_Id])
    REFERENCES [dbo].[Users_BasicUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BasicUserPurchasedCupon'
CREATE INDEX [IX_FK_BasicUserPurchasedCupon]
ON [dbo].[PurchasedCupons]
    ([BasicUser_Id]);
GO

-- Creating foreign key on [BussinessCupon_Id] in table 'PurchasedCupons'
ALTER TABLE [dbo].[PurchasedCupons]
ADD CONSTRAINT [FK_BussinessCuponPurchasedCupon]
    FOREIGN KEY ([BussinessCupon_Id])
    REFERENCES [dbo].[Cupons_BussinessCupon]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BussinessCuponPurchasedCupon'
CREATE INDEX [IX_FK_BussinessCuponPurchasedCupon]
ON [dbo].[PurchasedCupons]
    ([BussinessCupon_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Cupons_SocialNetworkCupon'
ALTER TABLE [dbo].[Cupons_SocialNetworkCupon]
ADD CONSTRAINT [FK_UserSocialNetworkCupon]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSocialNetworkCupon'
CREATE INDEX [IX_FK_UserSocialNetworkCupon]
ON [dbo].[Cupons_SocialNetworkCupon]
    ([User_Id]);
GO

-- Creating foreign key on [Id] in table 'Users_BussinessOwner'
ALTER TABLE [dbo].[Users_BussinessOwner]
ADD CONSTRAINT [FK_BussinessOwner_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Cupons_BussinessCupon'
ALTER TABLE [dbo].[Cupons_BussinessCupon]
ADD CONSTRAINT [FK_BussinessCupon_inherits_Cupon]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Cupons]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Users_BasicUser'
ALTER TABLE [dbo].[Users_BasicUser]
ADD CONSTRAINT [FK_BasicUser_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Cupons_SocialNetworkCupon'
ALTER TABLE [dbo].[Cupons_SocialNetworkCupon]
ADD CONSTRAINT [FK_SocialNetworkCupon_inherits_Cupon]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Cupons]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Users_SystemAdmin'
ALTER TABLE [dbo].[Users_SystemAdmin]
ADD CONSTRAINT [FK_SystemAdmin_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------