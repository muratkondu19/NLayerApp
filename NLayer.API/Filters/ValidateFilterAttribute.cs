using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;

namespace NLayer.API.Filters
{
    //ActionFilterAttribute miras alınır
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        //OnActionExecuting override edilir
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //ModelState ile FluentValidation birbiriyle entegredir ve hataları ModelState ile de yakalayabiliriz
            if (context.ModelState.IsValid)
            {
                //hata var ise
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();//hata sınıfını alma ve ErrorMessage dönme
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentResult>.Fail(400, errors));

            }
        }
    }
}
