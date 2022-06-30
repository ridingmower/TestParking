IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblAccessCardEvent')
BEGIN
CREATE TABLE [dbo].[tblAccessCardEvent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[CardNumber] [nvarchar](20) NOT NULL,
	[ControllerID] [nvarchar](50) NOT NULL,
	[CardGroupID] [nvarchar](50) NOT NULL,
	[ReaderIndex] [int] NOT NULL,
	[EventStatus] [nvarchar](100) NOT NULL,
	[CardNo] [nvarchar](50) NOT NULL,
	[EventType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tblAccessCardEvent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END