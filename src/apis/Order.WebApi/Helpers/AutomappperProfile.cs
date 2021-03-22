using AutoMapper;
using Microservices.Order.ViewModels;

namespace Order.WebApi.Helpers
{
    public class AutomappperProfile : Profile
    {
        public AutomappperProfile()
        {
            CreateMap<Microservices.Order.Data.Domains.Order, OrderViewModel>().ReverseMap();
        }
    }
}
