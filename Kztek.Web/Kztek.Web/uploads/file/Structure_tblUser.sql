IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'IsHideEventOut')
BEGIN
	ALTER TABLE tblSystemConfig ADD IsHideEventOut bit not null default(0)
END


IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '94291706')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('94291706', N'Tổng hợp thu tiền theo nhóm thẻ','Report','1','ReportTotalByCardGroup_LOTTE','/Report/ReportTotalByCardGroup_LOTTE','','81478553','1','0','10','/84530775/81478553/94291706/','3','12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '58510587')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('58510587', N'Excel','ReportTotalByCardGroup_LOTTE','2','Export','/ReportTotalByCardGroup_LOTTE/Export','','94291706','1','0','1','/84530775/81478553/94291706/58510587/','4','12878956','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'CardGroupId')
BEGIN
	ALTER TABLE tblLane ADD CardGroupId varchar(150) null
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '50034867')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('50034867', N'Ghi vé vào','ActionsApp','2','WriteIn','/ActionsApp/WriteIn','','94065935','1','0','9','/94065935/50034867/','2','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '113854902')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('113854902', N'Ghi vé ra','ActionsApp','2','WriteOut','/ActionsApp/WriteOut','','94065935','1','0','10','/94065935/113854902/','2','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '53117040')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('53117040', N'Sửa biển số','ActionsApp','2','FixPlate','/ActionsApp/FixPlate','','94065935','1','0','11','/94065935/53117040/','2','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '84530775')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('84530775', N'Web','controller_84530775','1','action_84530775','/controller_84530775/action_84530775','','0','1','0','0','/84530775/','1','67810176,98818976,12878956,61119719','0')
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '88049945')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '88049945'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '81478553')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '81478553'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '84330865')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '84330865'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '67797447')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '67797447'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '55844880')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '55844880'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '63779207')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '63779207'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '71080602')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '71080602'
END


IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '81507875')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '81507875'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '86760592')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '86760592'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '70081965')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '70081965'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '35537716')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '35537716'
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '96015492')
BEGIN
Update [dbo].[MenuFunction]
set ParentId = '84530775'
WHERE Id = '96015492'
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunctionConfig] WHERE Id = '84954591')
BEGIN
INSERT [dbo].[MenuFunctionConfig] ([Id], [MenuFunctionId]) VALUES ('84954591', '84530775')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessController' AND COLUMN_NAME = 'IsAddOutput')
BEGIN
	ALTER TABLE tblAccessController ADD IsAddOutput bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessController' AND COLUMN_NAME = 'NumberOutput')
BEGIN
	ALTER TABLE tblAccessController ADD NumberOutput int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'CheckPlateOutOption2')
BEGIN
	ALTER TABLE tblLane ADD CheckPlateOutOption2 int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'OpenBarrieIn')
BEGIN
	ALTER TABLE tblLane ADD OpenBarrieIn int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'OpenBarrieOption1')
BEGIN
	ALTER TABLE tblLane ADD OpenBarrieOption1 int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'OpenBarrieOption2')
BEGIN
	ALTER TABLE tblLane ADD OpenBarrieOption2 int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'CardGroupIds')
BEGIN
	ALTER TABLE tblUser ADD CardGroupIds varchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'CustomerGroupIds')
BEGIN
	ALTER TABLE tblUser ADD CustomerGroupIds varchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblUser' AND COLUMN_NAME = 'AccessControllerSelected')
BEGIN
	ALTER TABLE tblUser ADD AccessControllerSelected varchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'DateActive')
BEGIN
	ALTER TABLE tblCard ADD DateActive datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLED' AND COLUMN_NAME = 'RowNumber')
BEGIN
	ALTER TABLE tblLED ADD RowNumber int not null default(0)
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'DateUpload')
BEGIN
	ALTER TABLE tblCard ADD DateUpload datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'DateDelete')
BEGIN
	ALTER TABLE tblCard ADD DateDelete datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'DateSign')
BEGIN
	ALTER TABLE tblCard ADD DateSign datetime null
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'UserIDUpload')
BEGIN
	ALTER TABLE tblCard ADD UserIDUpload varchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'UserIDDelete')
BEGIN
	ALTER TABLE tblCard ADD UserIDDelete varchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'UserIDSign')
BEGIN
	ALTER TABLE tblCard ADD UserIDSign varchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomerGroup' AND COLUMN_NAME = 'Ordering')
BEGIN
	ALTER TABLE tblCustomerGroup ADD Ordering int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomerGroup' AND COLUMN_NAME = 'IsCompany')
BEGIN
	ALTER TABLE tblCustomerGroup ADD IsCompany bit not null default(0)
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomerGroup' AND COLUMN_NAME = 'Tax')
BEGIN
	ALTER TABLE tblCustomerGroup ADD Tax nvarchar(250) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomer' AND COLUMN_NAME = 'ContractStartDate')
BEGIN
	ALTER TABLE tblCustomer ADD ContractStartDate datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomer' AND COLUMN_NAME = 'ContractEndDate')
BEGIN
	ALTER TABLE tblCustomer ADD ContractEndDate datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'ContractStartDate')
BEGIN
	ALTER TABLE tblActiveCard ADD ContractStartDate datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'ContractEndDate')
BEGIN
	ALTER TABLE tblActiveCard ADD ContractEndDate datetime null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblFee' AND COLUMN_NAME = 'IsUseExtend')
BEGIN
	ALTER TABLE tblFee ADD IsUseExtend bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'IsDisplay')
BEGIN
	ALTER TABLE tblSystemConfig ADD IsDisplay bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCamera' AND COLUMN_NAME = 'ResizeImage')
BEGIN
	ALTER TABLE tblCamera ADD ResizeImage nvarchar(max) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'IsSaveButton')
BEGIN
	ALTER TABLE tblSystemConfig ADD IsSaveButton bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '54896855')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('54896855', N'Báo cáo chi tiết xe phát sinh phí phụ trội','Report','1','ReportVehicleMoneyByCardMonth','/Report/ReportVehicleMoneyByCardMonth','fa fa-caret-right','69053990','1','0','5','/81478553/69053990/54896855/','3','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '104451142')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('104451142', N'Báo cáo tổng hợp theo mã thẻ cho lượt vào ra phát sinh phụ trội','Report','1','ReportTotalSubventionByCardNumber','/Report/ReportTotalSubventionByCardNumber','fa fa-caret-right','69053990','1','0','6','/81478553/69053990/104451142/','3','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCustomer' AND COLUMN_NAME = 'AddressUnsign')
BEGIN
	ALTER TABLE tblCustomer ADD AddressUnsign nvarchar(MAX) null
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '84330865')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('84330865', N'Biểu đồ','controller_84330865','1','action_84330865','/controller_84330865/action_84330865','fa fa-bar-chart','0','1','0','96','/84330865/','1','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '84330865')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('84330865', N'Biểu đồ','controller_84330865','1','action_84330865','/controller_84330865/action_84330865','fa fa-bar-chart','0','1','0','96','/84330865/','1','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '85612479')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('85612479', N'Biểu đồ vào ra theo nhóm thẻ','Report','1','ReportChartInOutByCardGroup','/Report/ReportChartInOutByCardGroup','fa fa-caret-right','84330865','1','0','1','/84330865/85612479/','2','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '87343838')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('87343838', N'Biểu đồ xe vào ra theo làn','Report','1','ReportChartInOutByLane','/Report/ReportChartInOutByLane','fa fa-caret-right','84330865','1','0','2','/84330865/87343838/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '88072654')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('88072654', N'Biểu đồ xe vào ra theo thời gian','Report','1','ReportChartInOutByTime','/Report/ReportChartInOutByTime','fa fa-caret-right','84330865','1','0','3','/84330865/88072654/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '98425321')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('98425321', N'Biểu đồ doanh thu theo làn','Report','1','ReportChartMoneyByLane','/Report/ReportChartMoneyByLane','fa fa-caret-right','84330865','1','0','4','/84330865/98425321/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '80836351')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('80836351', N'Biểu đồ doanh thu theo mức thu','Report','1','ReportChartMoneyByLevel','/Report/ReportChartMoneyByLevel','fa fa-caret-right','84330865','1','0','5','/84330865/80836351/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '40465418')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('40465418', N'Biểu đồ doanh thu theo thời gian','Report','1','ReportChartMoneyByTime','/Report/ReportChartMoneyByTime','fa fa-caret-right','84330865','1','0','6','/84330865/40465418/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '74914283')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('74914283', N'Biểu đồ lượt xe ra theo làn','Report','1','ReportChartOutByLane','/Report/ReportChartOutByLane','fa fa-caret-right','84330865','1','0','7','/84330865/74914283/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '84125437')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('84125437', N'Biểu đồ lượt xe ra theo mức thu','Report','1','ReportChartOutByLevel','/Report/ReportChartOutByLevel','fa fa-caret-right','84330865','1','0','8','/84330865/84125437/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '41356495')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('41356495', N'Biểu đồ lượt xe ra theo thời gian','Report','1','ReportChartOutByTime','/Report/ReportChartOutByTime','fa fa-caret-right','84330865','1','0','9','/84330865/41356495/','2','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '117330057')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('117330057', N'Danh sách hóa đơn','OrderActiveCard','1','Index','/OrderActiveCard/Index','fa fa-caret-right','43820046','1','0','3','/67797447/43820046/117330057/','3','67810176,98818976,12878956','0')
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'OrderId')
BEGIN
	ALTER TABLE tblActiveCard ADD OrderId nvarchar(max) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'ContractCode')
BEGIN
	ALTER TABLE tblActiveCard ADD ContractCode nvarchar(50) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'IsTransferPayment')
BEGIN
	ALTER TABLE tblActiveCard ADD IsTransferPayment bit not null default(0)
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'isAutoCapture')
BEGIN
	ALTER TABLE tblCard ADD isAutoCapture bit not null default(0)
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessControllerGroup')
BEGIN
	CREATE TABLE [dbo].tblAccessControllerGroup(
	[Id] [nvarchar](128) NOT NULL,
	Name [nvarchar](500) NULL,
	Description [nvarchar](max) NULL,
	SortOrder [int],

 CONSTRAINT [PK_dbo.tblAccessControllerGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '68241310')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('68241310', N'Danh mục bộ điều khiển','tblAccessControllerGroup','1','Index','/tblAccessControllerGroup/Index','fa fa-caret-right','81507875','1','0','4','/81507875/68241310/','2','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '44970488')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('44970488', N'Thêm mới','tblAccessControllerGroup','2','Create','/tblAccessControllerGroup/Create','fa fa-caret-right','68241310','1','0','1','/81507875/68241310/44970488/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '43123164')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('43123164', N'Cập nhật','tblAccessControllerGroup','2','Update','/tblAccessControllerGroup/Update','fa fa-caret-right','68241310','1','0','2','/81507875/68241310/43123164/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '60486573')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('60486573', N'Xóa','tblAccessControllerGroup','2','Delete','/tblAccessControllerGroup/Delete','fa fa-caret-right','68241310','1','0','3','/81507875/68241310/60486573/','3','98818976','0')
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'isAuthInView')
BEGIN
	ALTER TABLE tblSystemConfig ADD isAuthInView bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'DelayTime')
BEGIN
	ALTER TABLE tblSystemConfig ADD DelayTime int not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User_AuthGroup')
BEGIN
	CREATE TABLE [dbo].[User_AuthGroup](
	[Id] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[CardGroupIds] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.User_AuthGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '77289307')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('77289307', N'Báo cáo thu tiền theo nhóm thẻ lượt','Report','1','BVDK_ReportTotalMoneyByCardGroup','/Report/BVDK_ReportTotalMoneyByCardGroup','fa fa-caret-right','108250286','1','0','2','/81478553/108250286/77289307/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '123157876')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('123157876', N'Xuất Excel','BVDK_ReportTotalMoneyByCardGroup','2','Export','/BVDK_ReportTotalMoneyByCardGroup/Export','fa fa-caret-right','77289307','1','0','1','/81478553/108250286/77289307/123157876/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '56485493')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('56485493', N'Xuất Excel','ActiveCardList','2','Export','/ActiveCardList/Export','fa fa-caret-right','71266266','1','0','1','/67797447/43820046/71266266/56485493/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '35388121')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('35388121', N'Xuất Excel','OrderActiveCard','2','Export','/OrderActiveCard/Export','fa fa-caret-right','117330057','1','0','2','/67797447/43820046/117330057/35388121/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '86760592')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('86760592', N'Xóa sự kiện','tblCardEventDelete','1','DeleteEvent','/tblCardEventDelete/DeleteEvent','fa fa-table','0','1','0','99','/86760592/','1','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '95549735')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('95549735', N'Lịch sử xóa sự kiện','tblCardEventDelete','2','HistoryDeleteEvent','/tblCardEventDelete/HistoryDeleteEvent','fa fa-caret-right','86760592','1','0','1','/86760592/95549735/','2','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '77341330')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('77341330', N'Xuất Excel','ReportCardExpire','2','Export','/ReportCardExpire/Export','fa fa-caret-right','91666525','1','0','1','/81478553/91666525/77341330/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '39485490')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('39485490', N'Xóa không điều kiện','tblCard','2','Remove','/tblCard/Remove','','76951396','1','0','8','/67797447/76951396/39485490/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'CardNo')
BEGIN
	ALTER TABLE tblActiveCard ADD CardNo nvarchar(50) not null default('')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardGroup' AND COLUMN_NAME = 'IsCheckPlate')
BEGIN
	ALTER TABLE tblCardGroup ADD IsCheckPlate bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardGroup' AND COLUMN_NAME = 'IsHaveMoneyExpiredDate')
BEGIN
	ALTER TABLE tblCardGroup ADD IsHaveMoneyExpiredDate bit not null default(0)
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'AccessLevelID')
BEGIN
	ALTER TABLE tblCard ADD AccessLevelID nvarchar(250) not null default('')
END


IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '50623372')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('50623372', N'Báo cáo FPT','controller_50623372','1','action_50623372','/controller_50623372/action_50623372','fa fa-caret-right','81478553','1','0','14','/81478553/50623372/','2','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '65923602')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('65923602', N'Cảnh báo quá lượt sử dụng','Report','1','FPT_AlarmExceededTurn','/Report/FPT_AlarmExceededTurn','fa fa-caret-right','50623372','1','0','1','/81478553/50623372/65923602/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '93666284')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('93666284', N'Cảnh báo không sử dụng thẻ tháng','Report','1','FPT_AlarmNotUse','/Report/FPT_AlarmNotUse','fa fa-caret-right','50623372','1','0','2','/81478553/50623372/93666284/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '87159331')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('87159331', N'Biểu đồ xe vào ra theo thời gian','Report','1','FPT_ChartInOutByTime','/Report/FPT_ChartInOutByTime','fa fa-caret-right','50623372','1','0','3','/81478553/50623372/87159331/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'CustomInfo')
BEGIN
	ALTER TABLE tblSystemConfig ADD CustomInfo nvarchar(max) null
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '90686603')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('90686603', N'Bạch Mai','controller_90686603','1','action_90686603','/controller_90686603/action_90686603','fa fa-caret-right','81478553','0','0','15','/81478553/90686603/','2','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '86765176')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('86765176', N'Dịch vụ trông xe','Report','1','BachMai_ReportS2','/Report/BachMai_ReportS2','fa fa-caret-right','90686603','0','0','1','/81478553/90686603/86765176/','3','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '28935584')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('28935584', N'Báo cáo xe đạp','Report','1','BachMai_ReportS3','/Report/BachMai_ReportS3','fa fa-caret-right','90686603','0','0','2','/81478553/90686603/28935584/','3','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '58407592')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('58407592', N'Xuất Excel','BachMai_ReportS2','2','Export','/BachMai_ReportS2/Export','','86765176','0','0','1','/81478553/90686603/86765176/58407592/','4','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '95052087')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('95052087', N'Xuất Excel','BachMai_ReportS3','2','Export','/BachMai_ReportS3/Export','','28935584','0','0','1','/81478553/90686603/28935584/95052087/','4','67810176,98818976,12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '100738988')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('100738988', N'Camera','tblAccessCamera','1','Index','/tblAccessCamera/Index','fa fa-caret-right','81507875','1','0','12','/81507875/100738988/','2','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '124825392')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('124825392', N'Thêm mới','tblAccessCamera','2','Create','/tblAccessCamera/Create','fa fa-caret-right','100738988','1','0','1','/81507875/100738988/124825392/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '37130812')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('37130812', N'Cập nhật','tblAccessCamera','2','Update','/tblAccessCamera/Update','fa fa-caret-right','100738988','1','0','2','/81507875/100738988/37130812/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '37671037')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('37671037', N'Xuất Excel','FPT_AlarmExceededTurn','2','Export','/FPT_AlarmExceededTurn/Export','fa fa-caret-right','65923602','1','0','1','/81478553/50623372/65923602/37671037/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '98958750')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('98958750', N'Xuất Excel','FPT_AlarmNotUse','2','Export','/FPT_AlarmNotUse/Export','fa fa-caret-right','93666284','1','0','1','/81478553/50623372/93666284/98958750/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'IsAutoCapture')
BEGIN
	ALTER TABLE tblSystemConfig ADD IsAutoCapture bit not null default(1)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ExtendCard')
BEGIN
CREATE TABLE [dbo].[ExtendCard](
	[Id] [nvarchar](128) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Date] [datetime] NULL,
	[CardNumber] [nvarchar](50) NOT NULL,
	[Plate] [nvarchar](50) NOT NULL,
	[OldExpireDate] [datetime] NULL,
	[Days] [int] NOT NULL,
	[NewExpireDate] [datetime] NULL,
	[CardGroupID] [nvarchar](50) NOT NULL,
	[CustomerGroupID] [nvarchar](50) NOT NULL,
	[CustomerID] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[FeeLevel] [int] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CardNo] [nvarchar](50) NOT NULL,
	[IsTransferPayment] [bit] NOT NULL,
	[SubId] [nvarchar](128) NULL,
    [DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.ExtendCard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblActiveCard' AND COLUMN_NAME = 'SubId')
BEGIN
	ALTER TABLE tblActiveCard ADD SubId nvarchar(128) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'IsLost')
BEGIN
	ALTER TABLE tblCard ADD IsLost bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '62359104')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('62359104', N'Gia hạn mới (chi tiết)','ActiveCard','1','Extend','/ActiveCard/Extend','fa fa-caret-right','43820046','1','0','1','/67797447/43820046/62359104/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '79884819')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('79884819', N'Danh sách gia hạn chi tiết','ActiveCardList','1','ExtendList','/ActiveCardList/ExtendList','fa fa-caret-right','43820046','1','0','4','/67797447/43820046/79884819/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '82057158')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('82057158', N'Xuất Excel','ActiveCardList','2','Export','/ActiveCardList/Export','fa fa-caret-right','79884819','1','0','1','/67797447/43820046/79884819/82057158/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '23291877')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('23291877', N'Thẻ theo căn hộ','controller_23291877','1','action_23291877','/controller_23291877/action_23291877','fa fa-caret-right','81478553','1','0','10','/81478553/23291877/','2','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '73834288')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('73834288', N'Báo cáo chi tiết','Report','1','ReportDetailCardCompartment','/Report/ReportDetailCardCompartment','fa fa-caret-right','23291877','1','0','1','/81478553/23291877/73834288/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '53552246')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('53552246', N'Xuất Excel','ReportDetailCardCompartment','2','Export','/ReportDetailCardCompartment/Export','fa fa-caret-right','73834288','1','0','1','/81478553/23291877/73834288/53552246/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '11228420')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('11228420', N'Báo cáo tổng hợp','Report','1','ReportTotalCardCompartment','/Report/ReportTotalCardCompartment','fa fa-caret-right','23291877','1','0','2','/81478553/23291877/11228420/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '84147312')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('84147312', N'Xuất Excel','ReportTotalCardCompartment','2','Export','/ReportTotalCardCompartment/Export','fa fa-caret-right','11228420','1','0','1','/81478553/23291877/11228420/84147312/','4','12878956,98818976,67810176','0')
END

IF EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '87340366')
BEGIN
Update [dbo].[MenuFunction]
set ActionName = 'Export'
WHERE Id = '87340366'
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '26187697')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('26187697', N'Chi tiết thu tiền thẻ lượt khi vào','Report','1','LAOCAI_ReportDetailMoneyCardDay','/Report/LAOCAI_ReportDetailMoneyCardDay','fa fa-caret-right','108250286','1','0','1','/81478553/108250286/26187697/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '69257811')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('69257811', N'Xuất Excel','LAOCAI_ReportDetailMoneyCardDay','2','Export','/LAOCAI_ReportDetailMoneyCardDay/Export','fa fa-caret-right','26187697','1','0','1','/81478553/108250286/26187697/69257811/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '145543937')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('145543937', N'In','LAOCAI_ReportDetailMoneyCardDay','2','Print','/LAOCAI_ReportDetailMoneyCardDay/Print','fa fa-caret-right','26187697','1','0','2','/81478553/108250286/26187697/145543937/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '46573729')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('46573729', N'In','ReportDetailMoneyCardDay','2','Print','/ReportDetailMoneyCardDay/Print','fa fa-caret-right','97335801','1','0','2','/81478553/108250286/97335801/46573729/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'isCompartment')
BEGIN
	ALTER TABLE tblSystemConfig ADD isCompartment bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'IsFaceRecognize')
                    BEGIN
                        ALTER TABLE tblLane ADD IsFaceRecognize bit not null default(0)
                   END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
                      WHERE TABLE_NAME = 'tblCamera' AND COLUMN_NAME = 'IsFaceRecognize')
                  BEGIN
                      ALTER TABLE tblCamera ADD IsFaceRecognize bit not null default(0)
                  END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCard' AND COLUMN_NAME = 'DVT')
BEGIN
	ALTER TABLE tblCard ADD DVT int not null default(0)
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '126756391')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('126756391', N'Hoành Bồ','controller_126756391','1','action_126756391','/controller_126756391/action_126756391','','81478553','1','0','17','/81478553/126756391/','2','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '140248500')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('140248500', N'Tổng hợp khối lượng xe chở vật liệu','Report','1','HOANHBO_ReportInOut','/Report/HOANHBO_ReportInOut','fa fa-caret-right','126756391','1','0','1','/81478553/126756391/140248500/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '41729663')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('41729663', N'Xuất Excel','HOANHBO_ReportInOut','2','Export','/HOANHBO_ReportInOut/Export','','140248500','1','0','1','/81478553/126756391/140248500/41729663/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '60299666')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('60299666', N'Sửa biển số','HOANHBO_ReportInOut','2','EditPlate','/HOANHBO_ReportInOut/EditPlate','','140248500','1','0','2','/81478553/126756391/140248500/60299666/','4','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '71838864')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('71838864', N'Thẻ phụ','tblSubCard','1','Index','/tblSubCard/Index','fa fa-caret-right','67797447','1','0','4','/67797447/71838864/','2','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '92360530')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('92360530', N'Thêm mới','tblSubCard','2','Create','/tblSubCard/Create','','71838864','1','0','1','/67797447/71838864/92360530/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '66911186')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('66911186', N'Cập nhật','tblSubCard','2','Update','/tblSubCard/Update','','71838864','1','0','2','/67797447/71838864/66911186/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '87916452')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('87916452', N'Xóa','tblSubCard','2','Delete','/tblSubCard/Delete','','71838864','1','0','3','/67797447/71838864/87916452/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '79540029')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('79540029', N'Xuất Excel','tblSubCard','2','Export','/tblSubCard/Export','','71838864','1','0','4','/67797447/71838864/79540029/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '70258740')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('70258740', N'Import file','tblSubCard','2','Import','/tblSubCard/Import','','71838864','1','0','5','/67797447/71838864/70258740/','3','12878956,98818976,67810176','0')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblSystemConfig' AND COLUMN_NAME = 'Background')
BEGIN
	ALTER TABLE tblSystemConfig ADD Background nvarchar(500) null
END



update MenuFunction
set 
ActionName = 'ReportParkingCardExpire'
where Id = '91666525'

update MenuFunction
set 
ControllerName = 'ReportParkingCardExpire'
where Id = '77341330'

update MenuFunction
set 
MenuName = 'Output'
where MenuName = 'Door'


IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '11131266')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('11131266', N'Xuất Excel','ReportEvent','2','Export','/ReportEvent/Export','','71149345','1','0','1','/81478553/71149345/11131266/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '92841742')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('92841742', N'Xuất Excel Bảo Việt','BAOVIET_ReportEvent','2','Export','/BAOVIET_ReportEvent/Export','','71149345','1','0','2','/81478553/71149345/92841742/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '85339467')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('85339467', N'Xuất Excel','ReportCardExpire','2','Export','/ReportCardExpire/Export','','98703784','1','0','1','/81478553/98703784/85339467/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '101249497')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('101249497', N'Xuất Excel','ReportCustomerExpire','2','Export','/ReportCustomerExpire/Export','','48658042','1','0','1','/81478553/48658042/101249497/','3','98818976','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '57881661')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('57881661', N'Phân quyền thẻ','tblCard','2','Authorize','/tblCard/Authorize','','76951396','1','0','15','/67797447/76951396/57881661/','3','98818976','0')
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DynamicCard')
BEGIN
CREATE TABLE [dbo].[DynamicCard](
	[Id] [varchar](128) NOT NULL,
	[PCID] [varchar](150) NULL,
	[ControllerID] [varchar](150) NULL,
	[Button] [int] NOT NULL default(0),
	[CardGroupID] [varchar](150) NULL,	
    [DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.DynamicCard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '123526012')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('123526012', N'Máy phát thẻ','DynamicCard','1','Index','/DynamicCard/Index','','81507875','1','0','10','/81507875/123526012/','2','12878956','0')
END


IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '79201848')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('79201848', N'Thêm mới','DynamicCard','2','Create','/DynamicCard/Create','','123526012','1','0','1','/81507875/123526012/79201848/','3','12878956','0')
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '78174881')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('78174881', N'Cập nhật','DynamicCard','2','Update','/DynamicCard/Update','','123526012','1','0','2','/81507875/123526012/78174881/','3','12878956','0')
END


IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '110046032')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('110046032', N'Xóa','DynamicCard','2','Delete','/DynamicCard/Delete','','123526012','1','0','3','/81507875/123526012/110046032/','3','12878956','0')
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'VehicleTypeLeft')
BEGIN
	ALTER TABLE tblLane ADD VehicleTypeLeft  varchar(128) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'CardTypeLeft')
BEGIN
	ALTER TABLE tblLane ADD CardTypeLeft  varchar(128) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'VehicleTypeRight')
BEGIN
	ALTER TABLE tblLane ADD VehicleTypeRight  varchar(128) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLane' AND COLUMN_NAME = 'CardTypeRight')
BEGIN
	ALTER TABLE tblLane ADD CardTypeRight  varchar(128) null
END

--ALTER TABLE [dbo].[tblLog]
--ALTER COLUMN Description nvarchar(1000); 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessController' AND COLUMN_NAME = 'ControllerGroupId')
BEGIN
	ALTER TABLE tblAccessController ADD ControllerGroupId  nvarchar(128) null
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLog' AND COLUMN_NAME = 'OldInfo')
BEGIN
	ALTER TABLE tblLog ADD OldInfo  nvarchar(250) null
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLog' AND COLUMN_NAME = 'NewInfo')
BEGIN
	ALTER TABLE tblLog ADD NewInfo  nvarchar(250) null
END

IF NOT EXISTS (SELECT * FROM [dbo].[MenuFunction] WHERE Id = '72549678')
BEGIN
INSERT [dbo].[MenuFunction] ([Id], [MenuName], [ControllerName],[MenuType], [ActionName],[Url],[Icon],[ParentId],[Active],[Deleted],[OrderNumber],[Breadcrumb],[Dept],[MenuGroupListId],[isSystem]) VALUES ('72549678', N'Bàn làm việc','Home','1','Index','/Home/Index','fa fa-dashboard','84530775','1','0','1','/84530775/72549678/','2','67810176,98818976,12878956,61119719','0')
END
