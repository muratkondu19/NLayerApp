using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    //IProductService miras alınır, mevcut IProduct yapısı bozulmaması gerektiğinden
    public class ProductServiceWithCaching : IProductService
    {
        //Tüm product'ların tutulacağı bir cache key tutulur 
        //in memory cache key-value şeklinde çalışır
        private const string CacheProductKey = "productsCache";

        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductServiceWithCaching(IProductRepository productRepository,IMapper mapper,IMemoryCache memoryCache,IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;   
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;

            //ProductService ilk nesne örneği oluşturulduğu anda cacheleme yapmak gerekmektedir
            //TryGetValue ile değer alınmaya çalışılır geriye bool döner, belirtilen keye göre döner. geriye true döndüğünde out ile başlayan parametresinde cachede tuttuğu daayı döner.
            //cachede yer alan data alınmak istenmiyor ise out _ kullanılır
            if (!_memoryCache.TryGetValue(CacheProductKey,out _))
            {
                //Cache de veri yoksa oluşturacak
                _memoryCache.Set(CacheProductKey, _productRepository.GetAll().ToList());
            }
        }


        public Task<Product> AddAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<Product> entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
