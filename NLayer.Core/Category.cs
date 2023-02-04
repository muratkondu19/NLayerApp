using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        //Kategorinin birden çok product'ı olabilir. 
        //Entity'ler içerisindeki farklı class ve entity'lere referans verilen property'lere navigation property nedir. 
        public ICollection<Product> Products { get; set; }
    }
}
