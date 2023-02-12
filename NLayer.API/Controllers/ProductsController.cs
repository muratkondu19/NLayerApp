using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //Route bilgileri CustomBaseController da yer aldığı için kaldırılıur
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;
        private readonly IProductService productService;

        public ProductsController(IMapper mapper, IService<Product> service, IProductService productService)
        {
            _mapper = mapper;
            _service = service;
            this.productService = productService;
        }


        //[HttpGet("GetProductsWithCategory")] //api/product/GetProductsWithCategory olarak çağırılacak / çakışmayı önlemek için
        [HttpGet("[action]")] //ile isim vermeden de kullanabiliriz action metodun ismini alır
        public async Task<IActionResult> GetProductsWithCategory()
        {
            //özelleştirilmiş bir servis olduğunda dto döndüğünden generic olmadığından apinin istediği data direkt olarak dönüyor
            //diğer metodlarda generic olmasından doalyı map işlemi gerekmektedir
            return CreateActionResult(await productService.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDto)); //bu işlemi tekrar terkar yapmamak için bir basecontroller oluşturulur
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));
        }

        [HttpGet("{id}")] //apiproduct/5 ->id 5 olan data gelir
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto)); //productDto producta map edilir
            var productsDto = _mapper.Map<ProductDto>(product); //alınan product tekrar dtoya dönüştürülür
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            ////Geriye birşey dönmediği için NoContentDto kullanılır
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")] //apiproduct/5 ->id 5 olan data silinir
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
