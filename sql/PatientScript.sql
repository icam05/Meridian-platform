USE [MeridianHealthAlliance]
GO

/****** Object:  Table [dbo].[Patient]    Script Date: 1/30/2026 4:36:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Patient](
	[PatientId] [bigint] IDENTITY(1,1) NOT NULL,
	[ExternalMemberId] [nvarchar](64) NOT NULL,
	[RaceTypeId] [int] NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[DateOfBirth] [date] NULL,
	[Sex] [char](1) NULL,
	[CreatedDtm] [datetime2](3) NOT NULL,
	[UpdatedDtm] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Patient_ExternalMemberId] UNIQUE NONCLUSTERED 
(
	[ExternalMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Patient] ADD  CONSTRAINT [DF_Patient_CreatedDtm]  DEFAULT (sysutcdatetime()) FOR [CreatedDtm]
GO

ALTER TABLE [dbo].[Patient] ADD  CONSTRAINT [DF_Patient_UpdatedDtm]  DEFAULT (sysutcdatetime()) FOR [UpdatedDtm]
GO

ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_RaceType] FOREIGN KEY([RaceTypeId])
REFERENCES [dbo].[RaceType] ([RaceTypeId])
GO

ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_RaceType]
GO


