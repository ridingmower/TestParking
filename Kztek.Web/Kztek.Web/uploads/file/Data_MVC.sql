IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MenuFunction')
BEGIN
	CREATE TABLE [dbo].[MenuFunction](
	[Id] [nvarchar](128) NOT NULL,
	[MenuName] [nvarchar](max) NOT NULL,
	[ControllerName] [nvarchar](150) NULL,
	[MenuType] [nvarchar](10) NULL,
	[ActionName] [nvarchar](150) NULL,
	[Url] [nvarchar](1000) NULL,
	[Icon] [nvarchar](100) NULL,
	[ParentId] [nvarchar](100) NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[OrderNumber] [int] NULL,
	[Breadcrumb] [nvarchar](max) NULL,
	[Dept] [int] NULL,
	[isSystem] [bit] NOT NULL,
	[MenuGroupListId] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.MenuFunction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MenuFunctionConfig')
BEGIN
	CREATE TABLE [dbo].[MenuFunctionConfig](
	[Id] [nvarchar](128) NOT NULL,
	[MenuFunctionId] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.MenuFunctionConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Role')
BEGIN
	CREATE TABLE [dbo].[Role](
	[Id] [nvarchar](128) NOT NULL,
	[RoleName] [nvarchar](150) NULL,
	[Description] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RoleMenu')
BEGIN
	CREATE TABLE [dbo].[RoleMenu](
	[Id] [nvarchar](128) NOT NULL,
	[MenuId] [varchar](150) NULL,
	[RoleId] [varchar](150) NULL,
 CONSTRAINT [PK_dbo.RoleMenu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User')
BEGIN
	CREATE TABLE [dbo].[User](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](250) NULL,
	[ImagePath] [varchar](500) NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [varchar](500) NULL,
	[PasswordSalat] [varchar](500) NULL,
	[Address] [nvarchar](500) NULL,
	[Phone] [varchar](150) NULL,
	[Admin] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserAvatar] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserRole')
BEGIN
	CREATE TABLE [dbo].[UserRole](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [varchar](150) NULL,
	[RoleId] [varchar](150) NULL,
 CONSTRAINT [PK_dbo.UserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SystemRecord')
BEGIN
CREATE TABLE [dbo].[SystemRecord](
	[Id] [nvarchar](128) NOT NULL,
	[Filename] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ComputerName] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SystemRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ExcelColumn')
BEGIN
	CREATE TABLE [dbo].[ExcelColumn](
	[Id] [nvarchar](128) NOT NULL,
	[MenuFunctionId] [nvarchar](max) NULL,
	[ColName] [nvarchar](max) NULL,
	[ColValue] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ExcelColumn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'API_Auth')
BEGIN
	CREATE TABLE [dbo].[API_Auth](
	[Id] [nvarchar](128) NOT NULL,
	[AccessToken] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.API_Auth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OrderActiveCard')
BEGIN
	CREATE TABLE [dbo].[OrderActiveCard](
	[Id] [nvarchar](128) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Price] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.OrderActiveCard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'isAutoCapture')
BEGIN
	ALTER TABLE tblCard ADD isAutoCapture bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblFtpAccount')
BEGIN
	CREATE TABLE [dbo].tblFtpAccount(
	[Id] [nvarchar](128) NOT NULL,
	[FtpHost] [nvarchar](MAX) NULL,
	[FtpUser] [nvarchar](MAX) NULL,
	[FtpPass] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_dbo.tblFtpAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_ApartmentRole')
BEGIN
	CREATE TABLE [dbo].BM_ApartmentRole(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_ApartmentRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Building_Service')
BEGIN
	CREATE TABLE [dbo].BM_Building_Service(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Price] decimal,
	[Day] INT NULL,
	[SchedulePay] [nvarchar](max) NULL,
	[ScheduleType] INT NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Building_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_ApartmentEWPrice')
BEGIN
	CREATE TABLE [dbo].BM_ApartmentEWPrice(
	[Id] [nvarchar](128) NOT NULL,
	[Electricity_Price] decimal,
	[Water_Price] decimal,
	[Date_Price] [datetime] NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_ApartmentEWPrice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_ResidentGroup')
BEGIN
	CREATE TABLE [dbo].BM_ResidentGroup(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ParentId] [nvarchar](max) NULL,
	[Ordering] INT NOT NULL DEFAULT(0),
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_ResidentGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Resident')
BEGIN
	CREATE TABLE [dbo].BM_Resident(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[CustomerId] [nvarchar](max) NULL,
	[ResidentGroupId] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Resident] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Building')
BEGIN
	CREATE TABLE [dbo].BM_Building(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[FloorNumber] INT NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Building] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Floor')
BEGIN
	CREATE TABLE [dbo].BM_Floor(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[BuildingId] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
    [Ordering] INT NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Floor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_ApartmentUse')
BEGIN
	CREATE TABLE [dbo].BM_ApartmentUse(
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[BuildingId] [nvarchar](max) NULL,
	[DateCreated] [datetime] NULL,
    [Ordering] INT NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_ApartmentUse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Apartment')
BEGIN
	CREATE TABLE [dbo].BM_Apartment(
	[Id] [nvarchar](128) NOT NULL,
	[Code] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[Acreage] decimal NULL,
	[ApartmentUseId] [nvarchar](128) NULL,
	[BuildingId] [nvarchar](128) NULL,
	[FloorId] [nvarchar](128) NULL,
	[ElecticityType] [int] NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Apartment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Apartment_Member')
BEGIN
	CREATE TABLE [dbo].BM_Apartment_Member(
	[Id] [nvarchar](128) NOT NULL,
	[ResidentId] [nvarchar](128) NOT NULL,
	[ApartmentId] [nvarchar](128) NULL,
	[RoleId] [nvarchar](128) NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Apartment_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Apartment_Service')
BEGIN
	CREATE TABLE [dbo].BM_Apartment_Service(
	[Id] [nvarchar](128) NOT NULL,
	[ApartmentId] [nvarchar](128) NULL,
	[ServiceId] [nvarchar](128) NULL,
	[Price] [decimal] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[SchedulePay] [nvarchar](max) NULL,
	[ScheduleType] [int] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Apartment_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BM_Apartment_Receipt')
BEGIN
	CREATE TABLE [dbo].BM_Apartment_Receipt(
	[Id] [nvarchar](128) NOT NULL,
	[Code] [nvarchar](128) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[ApartmentId] [nvarchar](128) NULL,
	[ResidentId] [nvarchar](128) NULL,
	[PayerName] [nvarchar](500) NULL,
	[PayerMobile] [nvarchar](50) NULL,
	[UserId] [nvarchar](128) NULL,
	[UserCreatedId] [nvarchar](128) NULL,
	[Money] [decimal] NULL,
	[DatePaid] [datetime] NULL,
	[PayType] [int] NULL,
	[Type] [int] NULL,
	[Status] [int] NULL,
	[DateCreated] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT(0),
 CONSTRAINT [PK_dbo.BM_Apartment_Receipt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'ViettelId')
BEGIN
	ALTER TABLE tblCard ADD ViettelId varchar(MAX) null default('')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'ViettelType')
BEGIN
	ALTER TABLE tblCard ADD ViettelType varchar(MAX) null default('')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MenuFunction' AND COLUMN_NAME = 'Dept')
BEGIN
	ALTER TABLE MenuFunction ADD Dept int null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MenuFunction' AND COLUMN_NAME = 'MenuGroupListId')
BEGIN
	ALTER TABLE MenuFunction ADD MenuGroupListId nvarchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MenuFunction' AND COLUMN_NAME = 'isSystem')
BEGIN
	ALTER TABLE MenuFunction ADD isSystem bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'IsDeleted')
BEGIN
	ALTER TABLE [User] ADD IsDeleted bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'CardType')
BEGIN
	ALTER TABLE [tblCard] ADD CardType int not null default(0)
END
--//orderinfo VNPAY
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'OrderInfo')
BEGIN
	CREATE TABLE [dbo].[OrderInfo](
	[OrderId] [nvarchar](128) NOT NULL,
	[Amount] [nvarchar](max)  NULL,
	[OrderDesc] [nvarchar](max) NULL,
	[Status] [nvarchar](128) NULL,
	[PaymentTranId] [nvarchar](max) NULL,
	[BankCode] [nvarchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[PayStatus] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.OrderInfo] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardProcess' AND COLUMN_NAME = 'OldInfoCP')
BEGIN
	ALTER TABLE tblCardProcess ADD OldInfoCP  nvarchar(Max) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardProcess' AND COLUMN_NAME = 'Plates')
BEGIN
	ALTER TABLE tblCardProcess ADD Plates  nvarchar(250) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardProcess' AND COLUMN_NAME = 'Actions')
BEGIN
	ALTER TABLE tblCardProcess ADD Actions  nvarchar(250) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardGroup' AND COLUMN_NAME = 'RestrictedNumber')
BEGIN
	ALTER TABLE tblCardGroup ADD RestrictedNumber    int not null default(-1)
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardGroup' AND COLUMN_NAME = 'IsSpecialGroup')
BEGIN
	ALTER TABLE tblCardGroup ADD IsSpecialGroup bit  not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLoopEvent' AND COLUMN_NAME = 'ReducedMoney')
BEGIN
	ALTER TABLE tblLoopEvent ADD ReducedMoney decimal(18, 0)  not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblDiscountParking' AND COLUMN_NAME = 'DiscountMode')
BEGIN
	ALTER TABLE tblDiscountParking ADD DiscountMode int not null default(0)
END
---




IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MultipleLanesMap')
BEGIN
	CREATE TABLE [dbo].[MultipleLanesMap](
	[Id] [varchar](250) NOT NULL,
     [Name] [nvarchar](250)  NULL,
	[PCid] [varchar](max) NOT NULL,
	[ViewOrder] int not NULL default (1),
	[SideIndex] int not NULL default (1),
	[CurrentDirection] int not NULL default (1),
 CONSTRAINT [PK_dbo.MultipleLanesMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,  ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
