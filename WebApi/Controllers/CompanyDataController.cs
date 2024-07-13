using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using WebApi.Models;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    /// <summary>
    /// 公司營收Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CompanyRevenueController : ControllerBase
    {

        private readonly CompanyDataContext _companyDataContext;

        private readonly ILogger<CompanyRevenueController> _logger;

        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyDataContext">公司資料庫</param>
        public CompanyRevenueController(ILogger<CompanyRevenueController> logger, CompanyDataContext companyDataContext, IMapper mapper)
        {
            _logger = logger;
            _companyDataContext = companyDataContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 查詢公司營收
        /// </summary>
        /// <param name="reportDate">出表日期</param>
        /// <param name="dataMonth">資料年月</param>
        /// <param name="companyId">公司代號</param>
        /// <param name="companyName">公司名稱</param>
        /// <param name="companyType">產業別</param>
        /// <param name="pageNumber">第幾頁</param>
        /// <param name="pageSize">單頁筆數</param>
        /// <returns>公司營收查詢結果</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CompanyRevenue>), 200)]
        [HttpGet(Name = "GetCompanyRevenue")]
        public IEnumerable<CompanyRevenueViewModel> Get(int? reportDate = 0, int? dataMonth = 0, int? companyId = 0, string? companyName = "", string? companyType = "", int pageNumber = 1, int pageSize = 50)
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

            if (companyType != "")
            {
                companyRevenueList = companyRevenueList.Where(c => c.CompanyType.Contains(companyType));
            }

            var pageData = companyRevenueList
                            .OrderBy(c => c.Id)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            var companyRevenueViewModels = _mapper.Map<IEnumerable<CompanyRevenueViewModel>>(pageData);
            return companyRevenueViewModels;
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
            var result = _companyDataContext.CompanyRevenues.Where(c => c.DataMonth == companyRevenue.DataMonth && c.CompanyId == companyRevenue.CompanyId).FirstOrDefault();

            var companyRevenueViewModels = _mapper.Map<CompanyRevenueViewModel>(result);
            return result;
        }

        /// <summary>
        /// 更新公司營收資料
        /// </summary>
        /// <returns>更新後的資料清單</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CompanyRevenue>), 200)]
        [HttpPatch(Name = "PatchCompanyRevenue")]
        public async Task<IEnumerable<CompanyRevenueViewModel>> Patch()
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

            var companyRevenueViewModels = _mapper.Map<IEnumerable<CompanyRevenueViewModel>>(_companyDataContext.CompanyRevenues.ToList());
            return companyRevenueViewModels;
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

        private class CustomDecimalConverter : CsvHelper.TypeConversion.DecimalConverter
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
