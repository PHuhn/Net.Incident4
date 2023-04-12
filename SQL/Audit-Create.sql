USE [NetIncidentIdentity04]
GO

/****** Object:  Table [dbo].[Audit]    Script Date: 3/3/2023 3:00:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.[Audit] (
	Id			bigint IDENTITY(1,1) NOT NULL,
	ChangeDate	datetime2(7) NOT NULL,
	UserName	nvarchar(256) NOT NULL,
	Program		nvarchar(256) NOT NULL,
	TableName	nvarchar(256) NOT NULL,
	UpdateType	CHAR(1) NOT NULL,
	Keys		nvarchar(512) NOT NULL,
	[Before]	nvarchar(4000) NOT NULL,
	[After]		nvarchar(4000) NOT NULL,
 CONSTRAINT PK_Audit PRIMARY KEY CLUSTERED 
(
	Id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


