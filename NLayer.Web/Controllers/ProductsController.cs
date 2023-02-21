using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Serivces;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;
        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            //MVC projesi olmasından dolayı CustomResponseDTO dönmeden kullanılabilir
            //Bu işlem için servis katmanında değişiklik yapılır. 
            //Durum kodları response üzerinden kaldırıldı
            //API katmanında hata alınmaması için Unload edildi
            //Cache aktif olmadığı için dosya adı değiştirildi. 
            return View(await _productApiService.GetProductWithCategoryAsync());
        }

        public async Task<IActionResult> Save()
        {
            var categoriesDto = await _categoryApiService.GetAllAsync();

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto product)
        {

            if (ModelState.IsValid)
            {
                await _productApiService.Save(product);
                return RedirectToAction(nameof(Index));
            }
            var categoriesDto = await _categoryApiService.GetAllAsync();

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            //İşlem başarısız ise category tekrar yüklenerek aynı sayfaya yeniden döner. 
            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiService.GetByIdAsync(id);

            var categoriesDto = await _categoryApiService.GetAllAsync();

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek,seçilen değer 
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.Update(productDto);
                return RedirectToAction(nameof(Index));
            }
            var categoriesDto = await _categoryApiService.GetAllAsync();


            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);
            //İşlem başarısız ise category tekrar yüklenerek aynı sayfaya yeniden döner. 
            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _productApiService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
