using AutoMapper;
using Microservices.Product.ViewModels;

namespace Product.WebApi.Helpers
{
    public class AutomappperProfile : Profile
    {
        public AutomappperProfile()
        {
            CreateMap<Microservices.Product.Data.Domains.Product, ProductViewModel>().ReverseMap();
            CreateMap<Microservices.Product.Data.Domains.ProductCategory, ProductCategoryViewModel>().ReverseMap();
        }
    }
}
