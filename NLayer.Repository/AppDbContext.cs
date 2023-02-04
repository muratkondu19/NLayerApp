using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    /*
        Repository katmanında veri tabanı ile ilgili işlemler gerçekleştirilir. 

        İlk olarak SQL Server’da veritabanına karşılık gelen bir class oluşturmaktır. Bu class ismi AppDbContex’tir.

        DbContext eki kullanma sebebi EF Core’da DbContext sınıfı Sql Server’da ya da ilişkisel bir veritabanında, veritabanına karşılık gelmektedir.
     */
    public class AppDbContext : DbContext
    {
        //Options ile beraber veritabanı yolunu startup dosyasından verilecek
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //Her bir entitye karşılık bir dbset oluşturulur
        public DbSet<Product> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        /*
         ProductFeatures, product ile ilgildir,  public DbSet<ProductFeature> ProductFeatures { get; set; } buraya eklenirse
        bağımsız olarak product feature satırları dbye eklenir, güncellenebilir, 
        buradan bu prop kapatıldığında bir product feat eklenmek istendiğinde product nesnesi üzerinden eklenmesi gerekecektir. 
        Örnek  var p = new Product { ProductFeature=new ProductFeature() { } }
        Best practices olarka product üzerinden işlem görmesi uygundur
         */

        //Entity ile ayar yapabilmek için ovveride edilemsi gereken metod / Model oluşurken çalışacak olan metod
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //entity ayarlarının burada yapılamsı dosyayı kalınlaştıracağı için her birinin ayrı dosyada yapılması önerilir. 
            //ApplyConfigurationsFromAssembly ilgili assembly'den (class library) ilgili tüm dosyaları oku, bu interface tüm configruationlarda (IEntityTypeConfiguration) implemente olduğu için okuyabilmektedir. 
            //Assembly.GetExecutingAssembly() -> çalıştığım assemblyde bunları ara
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //tek bir tanesi uygulanmak istenirse
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}
