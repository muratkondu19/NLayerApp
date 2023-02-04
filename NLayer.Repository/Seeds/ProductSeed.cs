using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //UpdatedDate dbye ilk kayıt eklendiğinde null olamsı gerekir bu sebeple nullable olmalıdır
            //UpdatedDate ve CreatedDate merkezi olarak yönetilecektir. Ekleme ve güncelleme işlemlerinde otomatikleştirilecektir. 
            builder.HasData(
                new Product { Id = 1, Name = "Kalem 1", CategoryId = 1, Price = 100, Stock = 20, CreatedDate = DateTime.Now },
                new Product { Id = 2, Name = "Kalem 2", CategoryId = 1, Price = 200, Stock = 20, CreatedDate = DateTime.Now },
                new Product { Id = 3, Name = "Kalem 3", CategoryId = 1, Price = 300, Stock = 20, CreatedDate = DateTime.Now },
                new Product { Id = 4, Name = "Kitap 1", CategoryId = 2, Price = 150, Stock = 50, CreatedDate = DateTime.Now },
                new Product { Id = 4, Name = "Kitap 2", CategoryId = 2, Price = 160, Stock = 70, CreatedDate = DateTime.Now });
        }
    }
}
