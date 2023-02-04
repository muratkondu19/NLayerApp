using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    //Tüm tablolarda olması gereken sütunlar için kullanılacaktır.
    public abstract class BaseEntity
    {
        //abstract yaparak base entity'den nesne örneği alınmasını engelleriz
        //abstract'lar ve interace'ler soyut nesnelerdir new ile yeni bir nesne oluşturulamaz
        //abstract class'lar genelde projede ortak olan prop ve metotları tanımladığımız yerlerdir
        //interface'lerde de genellikle kontrat ve sözleşmeler tanımlanır 
        public int Id { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
