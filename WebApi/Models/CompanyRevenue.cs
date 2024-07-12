using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class CompanyRevenue
    {
        public long Id { get; set; }
        public int ReportDate { get; set; }
        public int DataMonth { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string CompanyType { get; set; } = null!;
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueLastMonth { get; set; }
        public decimal RevenueThisMonthLastYear { get; set; }
        public decimal RevenueCompareLastMonthPercentage { get; set; }
        public decimal RevenueCompareMonthLastYearPercentage { get; set; }
        public decimal RevenueTotalThisMonth { get; set; }
        public decimal RevenueTotalLastYear { get; set; }
        public decimal RevenueCompareLastPeriodPercentage { get; set; }
    }
}
