using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
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

        //Entity ile ayar yapabilmek için ovveride edilemsi gereken metod / Model oluşurken çalışacak olan metod
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //entity ayarlarının burada yapılamsı dosyayı kalınlaştıracağı için her birinin ayrı dosyada yapılması önerilir. 
            //ApplyConfigurationsFromAssembly ilgili assembly'den (class library) ilgili tüm dosyaları oku, bu interface tüm configruationlarda (IEntityTypeConfiguration) implemente olduğu için okuyabilmektedir. 
            //Assembly.GetExecutingAssembly() -> çalıştığım assemblyde bunları ara
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //tek bir tanesi uygulanmak istenirse
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            //Seed işleminin OnModelCreating içerisinde tanımlanması örneği, best practices olarak seed klasöründen yapılmalıdır
            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature { Id = 1, Color = "Kırmızı", Height = 100, Width = 200, ProductId = 4 },
                new ProductFeature { Id = 2, Color = "Mavi", Height = 120, Width = 10, ProductId = 2 });

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            //Veritabanına verilerin yansımadan önce update/insert işlemi anlayarak date'lerin eklenmesi 
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entityReference.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                            entityReference.UpdatedDate = DateTime.Now;
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            //Veritabanına verilerin yansımadan önce update/insert işlemi anlayarak date'lerin eklenmesi 
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entityReference.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                            entityReference.UpdatedDate = DateTime.Now;
                            break;
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}

