IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerEvent')
BEGIN

CREATE TABLE [dbo].[tblLockerEvent](
	[Id] [VARCHAR](128) NOT NULL,

	[EventCode] INT NOT NULL DEFAULT(1),

	[CardNo] [nvarchar](500) NULL,
	[CardNumber] [nvarchar](500) NULL,
	[CardGroupID] [nvarchar](500) NULL,

	[LockerIndex] [nvarchar](500) NULL,
	[ControllerID] [nvarchar](500) NULL,
	 
	[FaceID] [nvarchar](500) NULL,

	[PicIn] [nvarchar](MAX) NULL,
	[PicOut] [nvarchar](MAX) NULL,

	[EventType] [nvarchar](500) NULL,
	[EventStatus] [nvarchar](50) NULL,

	[DateCreated] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0),
	[RegisterId] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblLockerEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerAlarm')
BEGIN

CREATE TABLE [dbo].[tblLockerAlarm](
	[Id] [VARCHAR](128) NOT NULL,

	[EventCode] INT NOT NULL DEFAULT(1),

	[CardNo] [nvarchar](500) NULL,
	[CardNumber] [nvarchar](500) NULL,
	[CardGroupID] [nvarchar](500) NULL,

	[LockerIndex] [nvarchar](500) NULL,
	[ControllerID] [nvarchar](500) NULL,

	[FaceID] [nvarchar](500) NULL,

	[PicDir] [nvarchar](MAX) NULL,

	[EventType] [nvarchar](500) NULL,

	[DateCreated] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0),
	[AlarmCode] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblLockerAlarm] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END