using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        /*
         * repositoryler entityler ile ilgili olduğundan orada product dönülürken burada dönüş tipi özelleştirilir
         * product ile kategori dönülmelidir bu sebeple bir DTO oluşturulur ->ProductWithCategoryDto
         * Repository'ler geriye entity döner iken servisler direkt olarak API'nin isteyeceği DTO'yu otomatik olarak döner. 
         */
        //Task<List<ProductWithCategoryDto>> GetProductListCategory();
        //Best practices için kullanım
         Task<List<ProductWithCategoryDto>> GetProductsWithCategory();
        //tam olarak apinin istediği datayı dönmektedir
    }
}
