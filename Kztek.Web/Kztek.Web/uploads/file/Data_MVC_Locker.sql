IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerPC')
BEGIN

CREATE TABLE [dbo].[tblLockerPC](
	[Id] [VARCHAR](128) NOT NULL,
	[PCName] [nvarchar](50) NULL,
	[IPAddress] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblLockerPC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerLine')
BEGIN

CREATE TABLE [dbo].[tblLockerLine](
	[Id] [VARCHAR](128) NOT NULL,
	[LineName] [nvarchar](50) NULL,
	[PCID] [nvarchar](50) NULL,
	[CommunicationType] [int] NULL,
	[Comport] [nvarchar](50) NULL,
	[Baudrate] [nvarchar](50) NULL,
	[LineTypeID] [int] NULL,
	[DownloadTime] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tblLockerLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerController')
BEGIN

CREATE TABLE [dbo].[tblLockerController](
	[Id] [VARCHAR](128) NOT NULL,
	[ControllerName] [nvarchar](50) NULL,
	[LineID] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL,
	[Address] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblLockerController] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLocker')
BEGIN

CREATE TABLE [dbo].[tblLocker](
	[Id] [VARCHAR](128) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[ReaderIndex] [INT] NOT NULL DEFAULT(0),
	[CardNo] [nvarchar](500) NULL,
	[CardNumber] [nvarchar](500) NULL,
	[ControllerID] [nvarchar](500) NULL,
	[DateCreated] [datetime] NOT NULL,
	[LockerType] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblLocker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerSelfHost')
BEGIN

CREATE TABLE [dbo].[tblLockerSelfHost](
	[Id] [nvarchar](128) NOT NULL,
	[Hostname] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[PCID] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.tblLockerSelfHost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerProcess')
BEGIN

CREATE TABLE [dbo].[tblLockerProcess](
	[Id] [nvarchar](128) NOT NULL,
	[LockerName] [nvarchar](max) NULL,
	[LockerReaderIndex] [INT] NOT NULL DEFAULT(0),
	[ControllerID] [nvarchar](max) NULL,
	[CardNumber] [nvarchar](max) NULL,
	[CardNo] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[ActionLocker] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.tblLockerProcess] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblLockerRegister')
BEGIN

CREATE TABLE [dbo].[tblLockerRegister](
	[Id] [nvarchar](128) NOT NULL,

	[Name] [nvarchar](max) NULL,
	[Mobile] [INT] NOT NULL DEFAULT(0),

	[CardNo] [nvarchar](max) NULL,
	[CardNumber] [nvarchar](max) NULL,

	[LockerID] [nvarchar](max) NULL,
	[LockerIndex] [nvarchar](max) NULL,
	[ControllerID] [nvarchar](max) NULL,

	[Description] [nvarchar](max) NULL,

	[Status] [INT] NOT NULL DEFAULT(0),
	[ImagePath] [nvarchar](max) NULL,

	[DateCreated] [datetime] NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0),
	
 CONSTRAINT [PK_dbo.tblLockerRegister] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END