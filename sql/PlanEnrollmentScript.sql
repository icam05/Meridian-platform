USE [MeridianHealthAlliance]
GO

/****** Object:  Table [dbo].[PlanEnrollment]    Script Date: 1/30/2026 4:38:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlanEnrollment](
	[PlanEnrollmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[PatientId] [bigint] NOT NULL,
	[PlanId] [int] NOT NULL,
	[EffectiveStartDtm] [datetime2](3) NOT NULL,
	[EffectiveEndDtm] [datetime2](3) NULL,
	[CreatedDtm] [datetime2](3) NOT NULL,
	[UpdatedDtm] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_PlanEnrollment] PRIMARY KEY CLUSTERED 
(
	[PlanEnrollmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PlanEnrollment] ADD  CONSTRAINT [DF_PlanEnrollment_CreatedDtm]  DEFAULT (sysutcdatetime()) FOR [CreatedDtm]
GO

ALTER TABLE [dbo].[PlanEnrollment] ADD  CONSTRAINT [DF_PlanEnrollment_UpdatedDtm]  DEFAULT (sysutcdatetime()) FOR [UpdatedDtm]
GO

ALTER TABLE [dbo].[PlanEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_PlanEnrollment_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patient] ([PatientId])
GO

ALTER TABLE [dbo].[PlanEnrollment] CHECK CONSTRAINT [FK_PlanEnrollment_Patient]
GO

ALTER TABLE [dbo].[PlanEnrollment]  WITH CHECK ADD  CONSTRAINT [FK_PlanEnrollment_Plan] FOREIGN KEY([PlanId])
REFERENCES [dbo].[HealthPlan] ([PlanId])
GO

ALTER TABLE [dbo].[PlanEnrollment] CHECK CONSTRAINT [FK_PlanEnrollment_Plan]
GO

ALTER TABLE [dbo].[PlanEnrollment]  WITH CHECK ADD  CONSTRAINT [CK_PlanEnrollment_DateOrder] CHECK  (([EffectiveEndDtm] IS NULL OR [EffectiveEndDtm]>[EffectiveStartDtm]))
GO

ALTER TABLE [dbo].[PlanEnrollment] CHECK CONSTRAINT [CK_PlanEnrollment_DateOrder]
GO

ALTER TABLE [dbo].[PlanEnrollment]  WITH CHECK ADD  CONSTRAINT [CK_PlanEnrollment_NoZeroLength] CHECK  (([EffectiveEndDtm] IS NULL OR [EffectiveEndDtm]<>[EffectiveStartDtm]))
GO

ALTER TABLE [dbo].[PlanEnrollment] CHECK CONSTRAINT [CK_PlanEnrollment_NoZeroLength]
GO


