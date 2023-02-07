using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        [NonAction] //Bu metodun endpoint olmadığnı belirtir
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            /*
             * Controllerde her seferinde ->  return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));
             * dönmemek için bu metod tanımlanmıştır 
             * CreateActionResult metodu çağırıldığında response status code ne ise onu dönecek 
             */
            if (response.StatusCode == 204) //204 no content durum kodu, bir cevap dönmez 
            {
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            }
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
