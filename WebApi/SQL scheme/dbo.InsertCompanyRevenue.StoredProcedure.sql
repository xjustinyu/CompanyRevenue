USE [CompanyData]
GO
/****** Object:  StoredProcedure [dbo].[InsertCompanyRevenue]    Script Date: 2024/7/15 上午 01:54:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCompanyRevenue]
    @ReportDate int,
    @DataMonth int,
    @CompanyId int,
    @CompanyName NVARCHAR(32),
    @CompanyType NVARCHAR(16),
    @RevenueThisMonth DECIMAL(18, 0),
    @RevenueLastMonth DECIMAL(18, 0),
    @RevenueThisMonthLastYear DECIMAL(18, 0),
    @RevenueCompareLastMonthPercentage DECIMAL(18, 0),
    @RevenueCompareMonthLastYearPercentage DECIMAL(18, 0),
    @RevenueTotalThisMonth DECIMAL(18, 0),
    @RevenueTotalLastYear DECIMAL(18, 0),
    @RevenueCompareLastPeriodPercentage DECIMAL(18, 0)
AS
BEGIN
    INSERT INTO [dbo].[CompanyRevenue]
        ([ReportDate], [DataMonth], [CompanyId], [CompanyName], [CompanyType], [RevenueThisMonth], [RevenueLastMonth], [RevenueThisMonthLastYear], [RevenueCompareLastMonthPercentage], [RevenueCompareMonthLastYearPercentage], [RevenueTotalThisMonth], [RevenueTotalLastYear], [RevenueCompareLastPeriodPercentage])
    VALUES
        (@ReportDate, @DataMonth, @CompanyId, @CompanyName, @CompanyType, @RevenueThisMonth, @RevenueLastMonth, @RevenueThisMonthLastYear, @RevenueCompareLastMonthPercentage, @RevenueCompareMonthLastYearPercentage, @RevenueTotalThisMonth, @RevenueTotalLastYear, @RevenueCompareLastPeriodPercentage);
END;
GO
