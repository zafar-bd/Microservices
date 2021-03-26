using AutoMapper;
using Microservices.Sales.Data.Domains;
using Microservices.Sales.ViewModels;

namespace Sales.WebApi.Helpers
{
    public class AutomappperProfile : Profile
    {
        public AutomappperProfile()
        {
            CreateMap<SalesDetails, SalesDetailsViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryViewModel>().ReverseMap();
            CreateMap<Microservices.Sales.Data.Domains.Sales, SalesViewModel>().ReverseMap();
        }
    }
}
