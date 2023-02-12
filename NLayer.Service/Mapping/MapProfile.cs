using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Mapping
{
    //Birden fazla map profile olabilir, ayrı ayrı isim vererek kullanılabilir. MapProductProfile gibi..
    //Profile sınıfından miras alır 
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //Maplemek istenen nesneler belirtilir.
            //CreateMap<Product,ProductDto>(); --> Product ProductDto'ya çevirilebilir
            CreateMap<Product, ProductDto>().ReverseMap(); //--> Product ProductDto'ya çevirilebilir,ProductDto Product'a çevirilebilir
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>(); //prodcut update işleminde producta dönüştür,tersine ihtiyaç olmadığı için reverse yok
            CreateMap<Product, ProductWithCategoryDto>();
        }
    }
}
