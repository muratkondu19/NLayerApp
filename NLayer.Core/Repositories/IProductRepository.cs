using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    /*
     * Product’larla beraber product’ın bağlı olduğu kategorileri getirecektir. 

        Bu custom istek için mevcuttaki generic repository tek bir entity ile işlem yaptığından ihtiyacı karşılamamaktadır. Generic repository ortak sorguların olduğu bir yer olduğundan custom bir sorgu yazmak gerekmektedir. 

        Bu custom sorgu için Porduct’a özel repository ve service oluşturulacak. 

        IProductRepository → implemente eden →ProductRepository

        IProductService →implemente eden → ProductService

        Bu interface’leri implemnte ederken bu product service aynı zamanda IGenericRepositoryProdcut interface’ini de implemente edecek ki o interface’den gelen temel crud işlemleri de bu repoda olacak.
     * Product ile ilgili işlemler olmasından dolayı generic kullanılmayacak, özelleştirilmiş bir repositorydir
     */
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsWithCategoryAsync();
    }
}
