IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessUploadDetail')
BEGIN
CREATE TABLE[dbo].[tblAccessUploadDetail](
    [Id][int] IDENTITY(1, 1) NOT NULL,
    [CardNumber][nvarchar](100) NULL,
    [UserIDofFinger][nvarchar](100) NULL,
    [Action][nvarchar](100) NOT NULL,
    [Status][nvarchar](100) NOT NULL,
    [ControllerID][nvarchar](100) NULL,
    [EventType][nvarchar](50) NULL,
    [UserID][nvarchar](100) NULL
) ON[PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblDiscountParking')
BEGIN
CREATE TABLE[dbo].[tblDiscountParking](
    [Id][nvarchar](150)  NOT NULL,
    [DCTypeName] [nvarchar](250) NOT  NULL,
    [DCTypeCode][nvarchar](250) NOT NULL,
    [DiscountMode] int not null default(0),
    [DiscountAmount][decimal] (18,2) NOT NULL default(0),
    [Note][nvarchar](250)  NULL ,
   [Priority][smallint] NOT NULL default(0) 
) ON[PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessUploadProcess')
BEGIN
CREATE TABLE [dbo].[tblAccessUploadProcess](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[CardNumber] [nvarchar](100) NULL,
	[UserIDofFinger] [nvarchar](100) NULL,
	[Actions] [nvarchar](100) NULL,
	[CardGroupID] [nvarchar](50) NULL,
	[UserID] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[AccessLevelID] [nvarchar](100) NULL,
	[CustomerID] [nvarchar](50) NULL,
	[CustomerGroupID] [nvarchar](50) NULL,
	[SuccessControllerIDs] [nvarchar](250) NULL,
	[TotalControllerIDs] [nvarchar](250) NULL,
	[EventType] [nvarchar](50) NULL,
	[AccessDateExpire] [datetime] NULL
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SelfHostConfig')
BEGIN
CREATE TABLE [dbo].[SelfHostConfig](
	[Id] [nvarchar](128) NOT NULL,
	[Hostname] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[PCID] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SelfHostConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'AccessExpireDate')
BEGIN
ALTER TABLE tblCard ADD AccessExpireDate datetime not null default('2099/12/31')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomer' AND COLUMN_NAME = 'AccessExpireDate')
BEGIN
ALTER TABLE tblCustomer ADD AccessExpireDate datetime not null default('2099/12/31')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomer' AND COLUMN_NAME = 'DevPass')
BEGIN
ALTER TABLE tblCustomer ADD DevPass nvarchar(max) not null default('')
END

IF NOT EXISTS (SELECT * FROM MenuFunction WHERE Id = '98703784')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName], [MenuType], [ActionName], [Url], [Icon], [ParentId], [Active], [Deleted], [OrderNumber], [Breadcrumb], [Dept], [isSystem], [MenuGroupListId]) VALUES (N'98703784', N'Thẻ hết hạn', N'Report', N'1', N'ReportCardExpire', N'/Report/ReportCardExpire', N'fa fa-caret-right', N'81478553', 1, 0, 11, N'/81478553/98703784/', 2, 0, N'98818976')
END

IF NOT EXISTS (SELECT * FROM MenuFunction WHERE Id = '48658042')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName], [MenuType], [ActionName], [Url], [Icon], [ParentId], [Active], [Deleted], [OrderNumber], [Breadcrumb], [Dept], [isSystem], [MenuGroupListId]) VALUES (N'48658042', N'Khách hàng hết hạn', N'Report', N'1', N'ReportCustomerExpire', N'/Report/ReportCustomerExpire', NULL, N'81478553', 1, 0, 12, N'/81478553/48658042/', 2, 0, N'98818976')
END

IF NOT EXISTS (SELECT * FROM MenuFunction WHERE Id = '71149345')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName], [MenuType], [ActionName], [Url], [Icon], [ParentId], [Active], [Deleted], [OrderNumber], [Breadcrumb], [Dept], [isSystem], [MenuGroupListId]) VALUES (N'71149345', N'Sự kiện vào ra', N'Report', N'1', N'ReportEvent', N'/Report/ReportEvent', N'fa fa-caret-right', N'81478553', 1, 0, 10, N'/81478553/71149345/', 2, 0, N'98818976')
END

IF NOT EXISTS (SELECT * FROM MenuFunctionConfig WHERE MenuFunctionId = '98703784')
BEGIN
INSERT [dbo].[MenuFunctionConfig] ([Id], [MenuFunctionId]) VALUES (N'99057886', N'98703784')
END

IF NOT EXISTS (SELECT * FROM MenuFunctionConfig WHERE MenuFunctionId = '48658042')
BEGIN
INSERT [dbo].[MenuFunctionConfig] ([Id], [MenuFunctionId]) VALUES (N'39927393', N'48658042')
END

IF NOT EXISTS (SELECT * FROM MenuFunctionConfig WHERE MenuFunctionId = '71149345')
BEGIN
INSERT [dbo].[MenuFunctionConfig] ([Id], [MenuFunctionId]) VALUES (N'66758050', N'71149345')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessDoor' AND COLUMN_NAME = 'Ordering')
BEGIN
ALTER TABLE tblAccessDoor ADD Ordering int not null default('0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessCamera')
BEGIN
CREATE TABLE [dbo].[tblAccessCamera](
	[CameraID] [uniqueidentifier] ROWGUIDCOL NOT NULL,
    [CameraCode] [nvarchar](5) NULL,
	[CameraName] [nvarchar](70) NULL,
	[HttpURL] [nvarchar](50) NULL,
	[HttpPort] [nvarchar](50) NULL,
	[RtspPort] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[FrameRate] [int] NULL,
	[Resolution] [nvarchar](50) NULL,
	[Channel] [int] NULL,
	[CameraType] [nvarchar](50) NULL,
	[StreamType] [nvarchar](50) NULL,
	[SDK] [nvarchar](50) NULL,
	[Cgi] [nvarchar](100) NULL,
	[EnableRecording] [bit] NOT NULL,
	[ControllerID] [varchar](50) NULL,
	[PositionIndex] [int] NULL,
	[Inactive] [bit] NOT NULL,
	[SortOrder] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_tblAccessCamera] PRIMARY KEY CLUSTERED 
(
	[CameraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessController' AND COLUMN_NAME = 'FunctionController')
BEGIN
	ALTER TABLE tblAccessController ADD FunctionController int not null default(0)
END
