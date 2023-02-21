using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //Route bilgileri CustomBaseController da yer aldığı için kaldırılıur
    //[Route("api/[controller]")]
    //[ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService,IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        //api/categories/GetSingeleCategoryByIdWithProductAsync/3
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingeleCategoryByIdWithProductAsync(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingeleCategoryByIdWithProductAsync(categoryId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200,categoriesDto));
        }
    }
}
