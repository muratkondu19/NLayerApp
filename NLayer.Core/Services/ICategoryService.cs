using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        //BUradaki servisten dto döneceği için bir dto oluşturulur->CategoryWithProductsDto
        public Task<CustomResponseDto<CategoryWithProductsDto>> GetSingeleCategoryByIdWithProductAsync(int categoryId);
    }
}
