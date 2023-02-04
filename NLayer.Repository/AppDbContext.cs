using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    /*
        Repository katmanında veri tabanı ile ilgili işlemler gerçekleştirilir. 

        İlk olarak SQL Server’da veritabanına karşılık gelen bir class oluşturmaktır. Bu class ismi AppDbContex’tir.

        DbContext eki kullanma sebebi EF Core’da DbContext sınıfı Sql Server’da ya da ilişkisel bir veritabanında, veritabanına karşılık gelmektedir.
     */
    public class AppDbContext:DbContext
    {
        //Options ile beraber veritabanı yolunu startup dosyasından verilecek
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
           
        }

        //Her bir entitye karşılık bir dbset oluşturulur
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        /*
         ProductFeatures, product ile ilgildir,  public DbSet<ProductFeature> ProductFeatures { get; set; } buraya eklenirse
        bağımsız olarak product feature satırları dbye eklenir, güncellenebilir, 
        buradan bu prop kapatıldığında bir product feat eklenmek istendiğinde product nesnesi üzerinden eklenmesi gerekecektir. 
        Örnek  var p = new Product { ProductFeature=new ProductFeature() { } }
        Best practices olarka product üzerinden işlem görmesi uygundur
         */
    }
}
