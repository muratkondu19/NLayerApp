using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            //MVC projesi olmasından dolayı CustomResponseDTO dönmeden kullanılabilir
            //Bu işlem için servis katmanında değişiklik yapılır. 
            //Durum kodları response üzerinden kaldırıldı
            //API katmanında hata alınmaması için Unload edildi
            //Cache aktif olmadığı için dosya adı değiştirildi. 
            return View(await _productService.GetProductsWithCategory());
        }
    }
}
