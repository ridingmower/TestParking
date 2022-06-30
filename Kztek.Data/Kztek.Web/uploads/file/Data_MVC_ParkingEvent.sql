IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblEventPayment')
BEGIN
CREATE TABLE [dbo].tblEventPayment(
	[EventId] [varchar](128) NOT NULL,
	[DateCreated] [datetime] NULL,
	[TimeIn] [datetime] NULL,
	[TimeOut] [datetime] NULL,
	[Plate] [nvarchar](500) NULL,
	[Money] [int] NULL,
	[OrderId] [varchar](128) NULL,
	[PaymentStatus] [int] NULL,
	[isSuccessQRCode] [bit] NOT NULL DEFAULT(0),
	[isSuccessPay] [bit] NOT NULL DEFAULT(0),
	[ResponseContentQRCode] [nvarchar](1000) NULL,
	[ResponseContentPay] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tblEventPayment] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardEvent' AND COLUMN_NAME = 'DCTypeCode')
BEGIN
	ALTER TABLE tblCardEvent ADD DCTypeCode  varchar(250) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardEvent' AND COLUMN_NAME = 'DiscountPercent')
BEGIN
	ALTER TABLE tblCardEvent ADD DiscountPercent  varchar(10) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardEvent' AND COLUMN_NAME = 'ReducedMoney')
BEGIN
	ALTER TABLE tblCardEvent ADD ReducedMoney  decimal(18,0) not null default (0)
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardEvent' AND COLUMN_NAME = 'DCTypeName')
BEGIN
	ALTER TABLE tblCardEvent ADD DCTypeName  nvarchar(250) null
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLog' AND COLUMN_NAME = 'OldInfo')
BEGIN
	ALTER TABLE tblLog ADD OldInfo  nvarchar(250) null
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardEvent' AND COLUMN_NAME = 'IsPlateInValid')
BEGIN
	ALTER TABLE [tblCardEvent] ADD IsPlateInValid bit not null default(0)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCardEvent' AND COLUMN_NAME = 'IsPlateOutValid ')
BEGIN
	ALTER TABLE [tblCardEvent] ADD IsPlateOutValid  bit not null default(0)
END


