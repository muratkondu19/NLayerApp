using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //Route bilgileri CustomBaseController da yer aldığı için kaldırılıur
    //[Route("api/[controller]")]
    //[ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //api/categories/GetSingeleCategoryByIdWithProductAsync/3
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingeleCategoryByIdWithProductAsync(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingeleCategoryByIdWithProductAsync(categoryId));
        }
    }
}
