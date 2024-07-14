USE [CompanyData]
GO
/****** Object:  StoredProcedure [dbo].[GetCompanyRevenueByCompanyId]    Script Date: 2024/7/15 上午 01:54:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCompanyRevenueByCompanyId]
    @CompanyId int
AS
BEGIN
    SELECT
        [Id],
        [ReportDate],
        [DataMonth],
        [CompanyId],
        [CompanyName],
        [CompanyType],
        [RevenueThisMonth],
        [RevenueLastMonth],
        [RevenueThisMonthLastYear],
        [RevenueCompareLastMonthPercentage],
        [RevenueCompareMonthLastYearPercentage],
        [RevenueTotalThisMonth],
        [RevenueTotalLastYear],
        [RevenueCompareLastPeriodPercentage]
    FROM [dbo].[CompanyRevenue]
    WHERE [CompanyId] = @CompanyId;
END;
GO
