using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    //Bu entity product'a bağlı olduğu için BaseEntity tanımlanmamıştır.
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        //FK olarak ProductId kullanılacak
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
