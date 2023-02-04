using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    public class Product:BaseEntity
    {
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
}
