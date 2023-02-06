using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    //Yazılan crud operasyonlarının tüm entity'ler için geçerli olması için generic ön eki verilmiştir.
    //IGenericRepository' Core üzerinden implemente edilmiştir
    public class GenericeRepository<T> : IGenericRepository<T> where T : class
    {
        //Db ile işlem yapabilmek için DbContext nesine ihtiyaç vardır 
        /*protexted yapma sebebi-> product sınıfına ait bu temel crud operasyonları dışında extra ayrı metotlara ihtiyaç olursa 
         * örnek olarak product ile ilgili kategoriler/feature alınmak istenirse burada böyle bir sorgu yer almamaktadır
         * bu sebeple özel olarak IProductRespository,ProductRepository, IProductService,ProductService gibi sadece product entity'si için 
         * özelleştirilmiş sınıflar oluşturulması gerekmektedir. 
         * Burada yer alan temel crud operasyonları işlemlere yetersiz kalırsa bu işleme başvurulur.
         * Bu sebeple oluşturulacak classlarda dbcontexte erişmek gerektiğinden protected olarak işaretlenir. 
         * Protected erişim belirgecine miras alan yer erişebilir.
         * Bu contextin sadece miraz alınan sınıflardan erişilmesi gerekmektedir. O sebeple protexted
         */
        protected readonly AppDbContext _context;

        /*
         * readonly kullanıldığında, bu değişkenleri ya tanımlama esnasında değer atanır ya da ctorda değer ataması yapılır
         * bu iki yer dışında değer atanmak istenirse compile zamanında hata alınır
         * bunlar ctorda değer atanacağı ve daha sonra da set edilmemesi gerektiğinden readonly olarak belirlenir. 
         */
        private readonly DbSet<T> _dbSet;

        //readonly olanların ctorda değer atanması
        public GenericeRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); //_context.Set<T>()->burası bir db set döner ve generic olarak DbSet<T> beklendiği için isteği karşılar 
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            //AsNoTracking-> EF Core çektiği dataları memory'e almaması için yazılır daha performanslı çalışma için 
            //bu kullanılmaz ise 1000 data çekildiğinde bunu hafızaya alır ve anlık olarak durumlarını izler, bu da uygulamanın perofmrmasını düşürür
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            //bu metodda async yoktur-> dbden silmez sadece EFCore'un hafızada entityi tutar ve remove dendiğinde sadece o entity'in state
            //durumu delete olarak işaretlenir. SaveChange çağırında delete flagi koyduklarını dbden siler. 
            _dbSet.Remove(entity);
            //_context.Entry(entity).State = EntityState.Deleted; //state güncellemesi, remove yerine kullanılabilir 
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            //foreach ile entitylerde dönüp her birinin stat durumunu deleted olarak günceller. 
            //save change olduğunda deleted olan flage sahip entityleri siler.
            _dbSet.RemoveRange(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
