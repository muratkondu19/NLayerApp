using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    /*
     Servis katmanı, business kodlarının olacağı yer, dbdendatayı aldıktan sonra o data ile ilgili ekstra işlem yapılacaksa ya da data farklı bir yere gönderilecekse bu işlemlerin yapılacağı yer bu katmandır. Repo katmanı sadece enttiy ile işlem yapar. 

    API ya da web uygulamaları bu katmanla haberleşir. 

    Servis katmanı core katmanında tanımlanacak IService interface’ine göre işlemlerini gerçekleştirecektir.

    Repo katmanından alınan data burada mapping yapılabilir farklı birşeylere dönüştürülebilir.

    ProductService isminde bir interface oluştuğunda dönüş tipleri farklı olacaktır. 
    IProductRepository içerisindeki metotların dönüş tipi ile IProductService interface içerisindeki metotların dönüş tipi farkılaşacaktır.
    Repository ve Service içerisindeki metotların dönüş tipi generic olmasından dolayı core üzerinde aynı metotlar kullanılmıştır.
     */
    public interface IService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        //Örnek olması açısından repositorydeki metot ile farklı tutulmuştur, tüm datayı çeker
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task<T> AddRangeAsync(IEnumerable<T> entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        //Repository void update işleminden farklı olarak serviste veritabanına bu değişikliklerin yansıtılacağı için 
        //SaveChangeAsync metodunun kullanılacağı için Task /async olarak kullanılır. 
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entity);
    }
}
