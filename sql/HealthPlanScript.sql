USE [MeridianHealthAlliance]
GO

/****** Object:  Table [dbo].[HealthPlan]    Script Date: 1/30/2026 4:38:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HealthPlan](
	[PlanId] [int] IDENTITY(1,1) NOT NULL,
	[PlanCode] [nvarchar](50) NOT NULL,
	[PlanName] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDtm] [datetime2](3) NOT NULL,
	[UpdatedDtm] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_Plan] PRIMARY KEY CLUSTERED 
(
	[PlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Plan_PlanCode] UNIQUE NONCLUSTERED 
(
	[PlanCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[HealthPlan] ADD  CONSTRAINT [DF_Plan_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[HealthPlan] ADD  CONSTRAINT [DF_Plan_CreatedDtm]  DEFAULT (sysutcdatetime()) FOR [CreatedDtm]
GO

ALTER TABLE [dbo].[HealthPlan] ADD  CONSTRAINT [DF_Plan_UpdatedDtm]  DEFAULT (sysutcdatetime()) FOR [UpdatedDtm]
GO


