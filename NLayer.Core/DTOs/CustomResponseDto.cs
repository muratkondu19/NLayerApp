using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    /*
     * API’de client tarafından kullanılan endpointler sonucunda geriye dönen model tek olmalıdır. 
    Bu class içerisinde başarılı ise geriye data döner hatalı ise hata dönmesi gerekir 
    Endpoint başarılı olduğunda feriye bir success dto isminde bir sınıf, başarısız olduğunda fail dto isminde bir sınıf dönülebilir. 
    Geriye 2 farklı model döndüğümüzde api kullanan clientlerde kendi uygulamalarında geriye 2 farklı model implemente etmeleri gerekmektedir. 
    Tek model kullanarak da işlem başarılı ise datayı değil ise aynı modelde hatayı dönebiliriz bu da bir best practices’dir fakat duruma göre de ayrı olabilir.
    Apilere özgü bir durumdur 
     */
    
    public class CustomResponseDto<T>
    {
        //bazı metotlarda t data null olabilmektedir ve bunu açıkça belirtmek gerekebilir. 
        //data response'da gönderilmeycekse boş bir class oluşturulabilir, boş bir dto oluşturulur ->NoContentDto
        //dönülecek prop isimlerinin aynı olması önemlidir.

        //Tek bir model dönmesi ele alınıyor
        public T Data { get; set; }
        //endpointe istek atıldığında geriye bir durum kodu döner bunu response ile dönmeye gerek yoktur. Kod içinde kullanmak için ekleniyor
        [JsonIgnore] //jsona dönüştürüken ignore eder / dış dünyaya kapatır / endpoint cliente kendisi status code döndüğü için
        public int StatusCode { get; set; }
        public List<String> Errors { get; set; }

        //new anahtar sözüğüyle oluşturmak yerine static olarak metot oluşturulur ve geriye nesne döner
        public static CustomResponseDto<T> Success(int statusCode,T data)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Data = data ,Errors=null};
        }

        //data dönmeden success işlemleri için durum kodu dönmek yeterliyse 
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode};
        }

        public static CustomResponseDto<T> Fail(int statusCode,List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode,Errors=errors };
        }

        //birden fazla değil tek error mevcut ise 
        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors=new List<string> { error } };
        }

        //burada yapılan işlem static factory metod (design pattern) işlemidir araştırılabilir 
        //nesne oluşturma işlemi kontrol altına alınmıştır.
    }
}
