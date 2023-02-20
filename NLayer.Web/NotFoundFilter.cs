using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault(); //id değerini alır

            if (idValue == null)
            {
                //id yok ise yoluna devam et
                await next.Invoke();
                return;
            }
            var id = (int)idValue; //değer cast ediliyor

            //entiy var mı yok mu onu kontrol ediyoruz
            //T Base entity olacağından base entity'de yer alan Id değerini eşitleyebiliyoruz 
            var anyEntity = await _service.AnyAsync(x => x.Id == id); //servis üzerinden id değerinin olup olmadığını kontrol eder 

            if (anyEntity)
            {
                await next.Invoke();
                return;
            }
            var errorViewModel = new ErrorViewModel();
            errorViewModel.Errors.Add($"{typeof(T).Name}({id}) not found");

            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel);
        }
    }
}
