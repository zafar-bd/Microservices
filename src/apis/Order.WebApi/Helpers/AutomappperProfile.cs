using AutoMapper;
using Microservices.Order.Data.Domains;
using Microservices.Order.ViewModels;

namespace Order.WebApi.Helpers
{
    public class AutomappperProfile : Profile
    {
        public AutomappperProfile()
        {
            CreateMap<Microservices.Order.Data.Domains.Order, OrderViewModel>().ReverseMap();
            CreateMap<Microservices.Order.Data.Domains.Order, MyOrderViewModel>().ReverseMap();
            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryViewModel>().ReverseMap();
        }
    }
}
