using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductServiceWithNoCaching : Service<Product>, IProductService
    {
        //burda yer alan repository yerine ProductRepository kullanılması gerekmektedir bu sebeple burada geçilmesi gerekmektedir.
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductServiceWithNoCaching(GenericeRepository<Product> repository, UnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;

        }
        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
             /*
             * Tam olarak apinin istediği data dönmektedir 
             * try catch ve business kodları burada yazılmaktadır 
             */
            var products = await _productRepository.GetProductsWithCategoryAsync();
            var prodcustDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, prodcustDto);
        }
    }
}
