using AutoMapper;
using Microservices.Product.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
