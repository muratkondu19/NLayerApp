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
    public class CategoryRepository : GenericeRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetSingeleCategoryByIdWithProductAsync(int categoryId)
        {
            //FirstOrDeafult->id değerinden dbde 4-5 tane var ise ilkini bulup döndürür, ex dönmez
            //Singe ile x=>x.Id==categoryId koşulu karşılayan birden çok kayıt bulursa geriye hata (ex) döner
            //id PK olduğundan first yerine single kullanmak daha uygun olmaktadır.
            return await _context.Categories.Include(x=>x.Products).Where(x=>x.Id==categoryId).SingleOrDefaultAsync();
        }
    }
}
