using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// 公司營收Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CompanyDataController : ControllerBase
    {
        
        private readonly CompanyDataContext _companyDataContext;
        
        private readonly ILogger<CompanyDataController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyDataContext">公司營收資料庫</param>
        public CompanyDataController(ILogger<CompanyDataController> logger, CompanyDataContext companyDataContext)
        {
            _logger = logger;
            _companyDataContext = companyDataContext;
        }

        /// <summary>
        /// 查詢公司營收
        /// </summary>
        /// <param name="reportDate">出表日期</param>
        /// <param name="dataMonth">資料年月</param>
        /// <param name="companyId">公司代號</param>
        /// <param name="companyName">公司名稱</param>
        /// <param name="companyType">產業別</param>
        /// <returns>公司營收查詢結果</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CompanyRevenue>), 200)]
        [HttpGet(Name = "GetCompanyRevenue")]
        public IEnumerable<CompanyRevenue> Get(int? reportDate = 0, int? dataMonth = 0, int? companyId = 0, string? companyName = "", string? companyType = "")
        {
            var companyRevenueList = _companyDataContext.CompanyRevenues.Where(c => c.Id > 0);

            if (reportDate != 0)
            {
                companyRevenueList = companyRevenueList.Where(c => c.ReportDate == reportDate);
            }

            if (dataMonth != 0)
            {
                companyRevenueList = companyRevenueList.Where(c => c.DataMonth == dataMonth);
            }

            if (companyId != 0)
            {
                companyRevenueList = companyRevenueList.Where(c => c.CompanyId == companyId);
            }

            if (companyName != "")
            {
                companyRevenueList = companyRevenueList.Where(c => c.CompanyName.Contains(companyName));
            }

            if (companyName != "")
            {
                companyRevenueList = companyRevenueList.Where(c => c.CompanyType.Contains(companyType));
            }

            return companyRevenueList.OrderBy(c => c.Id).ToList();
        }

        /// <summary>
        /// 新增一筆公司營收資料
        /// </summary>
        /// <param name="companyRevenue">公司營收資料</param>
        /// <returns>新增後的完整資料</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompanyRevenue), 200)]
        [HttpPost(Name = "AddCompanyRevenue")]
        public CompanyRevenue? Post(CompanyRevenue companyRevenue)
        {
            _companyDataContext.Add(companyRevenue);
            _companyDataContext.SaveChanges();
            var result = _companyDataContext.CompanyRevenues.Where(c =>c.DataMonth == companyRevenue.DataMonth && c.CompanyId == companyRevenue.CompanyId).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 更新公司營收資料
        /// </summary>
        /// <returns>更新後的資料清單</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CompanyRevenue>), 200)]
        [HttpPatch(Name = "PatchCompanyRevenue")]
        public async Task<IEnumerable<CompanyRevenue>> Patch()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("BIG5");

            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                // 採用標準的 RFC 4180 解析與寫入 CSV 資料
                Mode = CsvMode.RFC4180,
                // 用來讓 CSV 欄位標頭不區分大小寫
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };

            var client = new HttpClient();
            var url = "https://mopsfin.twse.com.tw/opendata/t187ap05_L.csv";
            var stream = await client.GetStreamAsync(url);
            //string filePath = @"D:\t187ap05_L.csv";
            //using (var filerStream = new FileStream(filePath, FileMode.Open))
            using (var reader = new StreamReader(stream, encoding))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<CompanyRevenueMap>();
                    List<CompanyRevenue> companyRevenueList = new();
                    var isHeader = true;
                    while (csv.Read())
                    {
                        if (isHeader)
                        {
                            csv.ReadHeader();
                            isHeader = false;
                            continue;
                        }
                        else
                        {
                            var companyRevenue = csv.GetRecord<CompanyRevenue>();
                            var query = _companyDataContext.CompanyRevenues.Where(c => c.CompanyId == companyRevenue.CompanyId && c.DataMonth == companyRevenue.DataMonth).FirstOrDefault();

                            if (query != null)
                            {
                                if (query.ReportDate < companyRevenue.ReportDate)
                                {
                                    query.ReportDate = companyRevenue.ReportDate;
                                    query.DataMonth = companyRevenue.DataMonth;
                                    query.CompanyId = companyRevenue.CompanyId;
                                    query.CompanyName = companyRevenue.CompanyName;
                                    query.CompanyType = companyRevenue.CompanyType;
                                    query.RevenueThisMonth = companyRevenue.RevenueThisMonth;
                                    query.RevenueLastMonth = companyRevenue.RevenueLastMonth;
                                    query.RevenueThisMonthLastYear = companyRevenue.RevenueThisMonthLastYear;
                                    query.RevenueCompareLastMonthPercentage = companyRevenue.RevenueCompareLastMonthPercentage;
                                    query.RevenueCompareMonthLastYearPercentage = companyRevenue.RevenueCompareMonthLastYearPercentage;
                                    query.RevenueTotalThisMonth = companyRevenue.RevenueTotalThisMonth;
                                    query.RevenueTotalLastYear = companyRevenue.RevenueTotalLastYear;
                                    query.RevenueCompareLastPeriodPercentage = companyRevenue.RevenueCompareLastPeriodPercentage;
                                }
                            }
                            else
                            {
                                companyRevenueList.Add(companyRevenue);
                            }
                        }
                    }

                    _companyDataContext.AddRange(companyRevenueList);
                    _companyDataContext.SaveChanges();
                }
            }

            return _companyDataContext.CompanyRevenues.ToList();
        }

        sealed class CompanyRevenueMap : ClassMap<CompanyRevenue>
        {
            public CompanyRevenueMap()
            {
                Map(m => m.ReportDate).Name("出表日期");
                Map(m => m.DataMonth).Name("資料年月");
                Map(m => m.CompanyId).Name("公司代號");
                Map(m => m.CompanyName).Name("公司名稱");
                Map(m => m.CompanyType).Name("產業別");
                Map(m => m.RevenueThisMonth).Name("營業收入-當月營收");
                Map(m => m.RevenueLastMonth).Name("營業收入-上月營收");
                Map(m => m.RevenueThisMonthLastYear).Name("營業收入-去年當月營收");
                Map(m => m.RevenueCompareLastMonthPercentage).Name("營業收入-上月比較增減(%)").TypeConverter(new CustomDecimalConverter());
                Map(m => m.RevenueCompareMonthLastYearPercentage).Name("營業收入-去年同月增減(%)").TypeConverter(new CustomDecimalConverter());
                Map(m => m.RevenueTotalThisMonth).Name("累計營業收入-當月累計營收");
                Map(m => m.RevenueTotalLastYear).Name("累計營業收入-去年累計營收");
                Map(m => m.RevenueCompareLastPeriodPercentage).Name("累計營業收入-前期比較增減(%)").TypeConverter(new CustomDecimalConverter());
            }
        }

        public class CustomDecimalConverter : CsvHelper.TypeConversion.DecimalConverter
        {
            public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
            {
                if (decimal.TryParse(text, out var result))
                {
                    return result;
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }
    }
}
