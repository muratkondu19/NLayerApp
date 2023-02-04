using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    /*
     * Core katmanında repository design pattern uygulanması için interface yazılması
        Repository Desing Pattern Nedir?

        Kod ile veritabanı arasına bir katman yerleştirir ve bu katman sayesinde veritabanına yapılan temel CRUD operasyonlarını kolay bir şekilde her bir entity için uygulayabilmemize imkan verir. 

        Generic ifadesinin yer alması bu repository katmanı üzerinden her bir entity için veritabanına temel CRUD operasyonları yapılabiliyor olacak.
     */

    //Veritabanına yapılabilecek tüm genel sorgular burada eklenecek 
    public interface IGenericRepository<T> where T : class //T bir class olacak
    {
        Task<T> GetByIdAsync(int id);

        //productRepository.GetAll(x=>x.id>5).orderBu.tolist -> ne zaman to list çağırılırsa veritabanına o zaman sorgu atılır
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        /*IQueryable dönüldüğünde yazılan sorgular direkt olarak veritabanına gitmez mutlaka ToList gibi metotlar çağırılınca veritabanına gider
        productRepository.where(x=>x.id>5) buraya kadar olan kısımdan IQueryable döner veritabanına sorgu yapılmaz 
        bunun üzerinden orderby çağırılabilir farklı işlemler yapılabilir fakat ne zaman tolistasync denirse dbye sorgu yapılabilir. 
        productRepository.where(x=>x.id>5).OrderBy.ToListAsync()-> dbden direkt olarak idsi 5 den büyük olan order by ile sıralanmış datayı alır

        Bu ifadeyi yazabilmek için Expression func delegesi yazılmalıdır
        EF Core'daki sorguların hepsi birer expression alır. 
        Tipi Func delegesi olacak.  Actioni func birer delegedir. Delegeler metotları işaret eden yapılardır.
        Expression<Func<T,bool>> bir Entity alacak ve geriye bool dönecek,x.id>5 her bir satırda true ya da false döner 
        */
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);

        //IEnumerable classını impelemente etmiş her class'a cast edilebilir
        Task<T> AddRangeAsync(IEnumerable<T> entity);

        //Bir metot async ise sonuna async eklenmesi belirtilmesi açısından önemlidir. 
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        /*
         Update ve remove işlemlerinin EF Core tarafında async metotları yoktur. 
         EF Core memory'e alıp takip ettiği bir product'ın sadece state'ini (modified) değiştir. Uzun süren bir işlem olamdığı için async yoktur. 
         */
        void Update(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
