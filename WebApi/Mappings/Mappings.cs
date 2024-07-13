using AutoMapper;
using WebApi.Models;
using WebApi.ViewModels;

namespace WebApi.Mappings
{
    /// <summary>
    /// AutoMapper設定檔
    /// </summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CompanyRevenue, CompanyRevenueViewModel>();
        }
    }
}
