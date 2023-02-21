using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    /*
     * Temel crud operasyonları tekrar yazmamak için GenericeRepository implemente edilir
     */
    public class ProductRepository : IGenericeRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }
        //GenericeRepository da yer alan hazır metodlar miras alındı
        //IProductRepository'den de GetProductListCategory implemente edildi
        //ProductRepository üzerinden hem GenericeRepository metodlarına hem de IProductRepository metodlarına erişim sağlanır
        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            //Include ile  Eager loading yapıldı ->data çekilirlen kategorilerinde alınması istendi
            //Product'a bağlı kategoriyi ihtiyaç olduğunda daha sonra şekilirse bu da Lazzy Loading işlemidir.
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
