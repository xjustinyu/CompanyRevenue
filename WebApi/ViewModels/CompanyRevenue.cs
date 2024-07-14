using System;
using System.Collections.Generic;

namespace WebApi.ViewModels
{
    /// <summary>
    /// 查詢結果
    /// </summary>
    public class GetCompanyRevenueResponse
    {
        /// <summary>
        /// 查詢資料總數
        /// </summary>
        public int TotalCount {  get; set; }

        /// <summary>
        /// CompanyRevenueViewModel List
        /// </summary>
        public List<CompanyRevenueViewModel>? CompanyRevenueList { get; set; }
    }

    /// <summary>
    /// 公司營收資料
    /// </summary>
    public class CompanyRevenueViewModel
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 報表日期
        /// </summary>
        public int ReportDate { get; set; }

        /// <summary>
        /// 資料年月
        /// </summary>
        public int DataMonth { get; set; }

        /// <summary>
        /// 公司代號
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompanyName { get; set; } = null!;

        /// <summary>
        /// 產業別
        /// </summary>
        public string CompanyType { get; set; } = null!;

        /// <summary>
        /// 營業收入-當月營收
        /// </summary>
        public decimal RevenueThisMonth { get; set; }

        /// <summary>
        /// 營業收入-上月營收
        /// </summary>
        public decimal RevenueLastMonth { get; set; }

        /// <summary>
        /// 營業收入-去年當月營收
        /// </summary>
        public decimal RevenueThisMonthLastYear { get; set; }

        /// <summary>
        /// 營業收入-上月比較增減(%)
        /// </summary>
        public decimal RevenueCompareLastMonthPercentage { get; set; }

        /// <summary>
        /// 營業收入-去年同月增減(%)
        /// </summary>
        public decimal RevenueCompareMonthLastYearPercentage { get; set; }

        /// <summary>
        /// 累計營業收入-當月累計營收
        /// </summary>
        public decimal RevenueTotalThisMonth { get; set; }

        /// <summary>
        /// 累計營業收入-去年累計營收
        /// </summary>
        public decimal RevenueTotalLastYear { get; set; }

        /// <summary>
        /// 累計營業收入-前期比較增減(%)
        /// </summary>
        public decimal RevenueCompareLastPeriodPercentage { get; set; }
    }
}
