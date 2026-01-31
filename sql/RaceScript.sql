USE [MeridianHealthAlliance]
GO

/****** Object:  Table [dbo].[RaceType]    Script Date: 1/30/2026 4:39:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RaceType](
	[RaceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[RaceCode] [nvarchar](20) NOT NULL,
	[RaceName] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDtm] [datetime2](3) NOT NULL,
	[UpdatedDtm] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_RaceType] PRIMARY KEY CLUSTERED 
(
	[RaceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_RaceType_RaceCode] UNIQUE NONCLUSTERED 
(
	[RaceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RaceType] ADD  CONSTRAINT [DF_RaceType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[RaceType] ADD  CONSTRAINT [DF_RaceType_CreatedDtm]  DEFAULT (sysutcdatetime()) FOR [CreatedDtm]
GO

ALTER TABLE [dbo].[RaceType] ADD  CONSTRAINT [DF_RaceType_UpdatedDtm]  DEFAULT (sysutcdatetime()) FOR [UpdatedDtm]
GO


