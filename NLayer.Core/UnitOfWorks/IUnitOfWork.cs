using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.UnitOfWorks
{
    /*
     IUnitOfWork adında bir interface oluşturulacak ve bu interface ile birlikte ilgili patternın implementasyonu gerçekleştirilecek.

    UnitOfWork Desing Pattern Nedir?

    Veritabanına yapılacak olan işlemlerin toplu bir şekilde bir transaction üzerinden yönetilmesine imkan verir. 

    EF Core’da bir DbContext nesnesi var Generic Repository aracılığıyla IProductRepositroy ICategoryRepository gibi farklı repositoryler var, bunlar üzerinden remove update gibi metotlar çağırarak veritabanına güncelleme yapılmak istenir. 

    ProductRepository’de add metoduyla datalar eklendiğinde, update yapıldığında vs gibi işlemlerde EF Core’un SaveChanges metodunu çağırana kadar EF Core yapılan bu işlemleri memory’de tutar. 

    SaveChanges çağırıldığında veritabanıan yapılmış olan repositroydeki değişiklikleri yansıtır. Bu save changes metodu kontrol altına alınmalıdır. Her repository işlemi yapıldıktan sonra save changes metodu çağırmamalıdır. Ne zaman talep edilirse o zaman çağırılmalıdır. 

    UnitOfWork deseni farklı farklı repositorylerde yapılmış olan işlemleri tek bir seferde tek bir transaction bloğunda veritabanına yansıtmamıza izin verir. Repository’lerin herhangi birinde yapılan işlem hatalı ise rollback ile işlemler geriye alınır ve veritabanında tutarsız bir işlemi engeller.
     */
    public interface IUnitOfWork
    {
        Task CommitAsync(); //SaveChaneAsync 
        void Commit(); //SaveChange
    }
}
