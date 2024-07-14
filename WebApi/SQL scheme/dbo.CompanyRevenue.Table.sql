USE [CompanyData]
GO
/****** Object:  Table [dbo].[CompanyRevenue]    Script Date: 2024/7/15 上午 01:54:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyRevenue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReportDate] [int] NOT NULL,
	[DataMonth] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CompanyName] [nvarchar](32) NOT NULL,
	[CompanyType] [nvarchar](16) NOT NULL,
	[RevenueThisMonth] [decimal](18, 0) NOT NULL,
	[RevenueLastMonth] [decimal](18, 0) NOT NULL,
	[RevenueThisMonthLastYear] [decimal](18, 0) NOT NULL,
	[RevenueCompareLastMonthPercentage] [decimal](18, 10) NOT NULL,
	[RevenueCompareMonthLastYearPercentage] [decimal](18, 10) NOT NULL,
	[RevenueTotalThisMonth] [decimal](18, 0) NOT NULL,
	[RevenueTotalLastYear] [decimal](18, 0) NOT NULL,
	[RevenueCompareLastPeriodPercentage] [decimal](18, 10) NOT NULL,
 CONSTRAINT [PK_CompanyRevenue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CompanyIdMothUnique] UNIQUE NONCLUSTERED 
(
	[CompanyId] ASC,
	[DataMonth] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CompanyRevenue]  WITH CHECK ADD  CONSTRAINT [FK_CompanyRevenue_CompanyRevenue] FOREIGN KEY([Id])
REFERENCES [dbo].[CompanyRevenue] ([Id])
GO
ALTER TABLE [dbo].[CompanyRevenue] CHECK CONSTRAINT [FK_CompanyRevenue_CompanyRevenue]
GO
