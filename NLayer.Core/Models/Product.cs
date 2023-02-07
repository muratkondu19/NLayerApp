using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        /*
        //.net6 nullable için kullanılabilir 
        Nullable özelliğinin run time’da uygulama çalışırken bir önemi yoktur. 
        Sadece kodlama esnasında uyarı verir.
        public Product(string name)
        {
            //Name product üretilirken null olursa hata fırlatmasını sağlar.
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        */
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        //Ürünün bir adet kategorisi vardır / prodcut ile ilişkinin kurulması / Navigation prop 
        public Category Category { get; set; }
        //Navigation prop
        public ProductFeature ProductFeature { get; set; }
    }
    //EF, Id proprery adını PK olarak algılarken CategoryId özelliğini de FK olarak algılar.
    //Farklı bir isimlendirme yapılırsa attribute olarak fk olarak belirtilmelidir.
    //public ProductFeature ProductFeature { get; set; } null olabilecekse public ProductFeature? ProductFeature { get; set; } olarak kullanılabilir, uygulama çalışırken null hata alınmasını engeller
}
