﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService,IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
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

        public async Task<IActionResult> Save()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto product)
        {
          
            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(product));
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            //İşlem başarısız ise category tekrar yüklenerek aynı sayfaya yeniden döner. 
            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek,seçilen değer 
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name",product.CategoryId);
            return View(_mapper.Map<ProductDto>(product));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //Kullanılacı Name görerek onu seçecek Id değeri gönderilecek
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name",productDto.CategoryId);
            //İşlem başarısız ise category tekrar yüklenerek aynı sayfaya yeniden döner. 
            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            await _productService.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
