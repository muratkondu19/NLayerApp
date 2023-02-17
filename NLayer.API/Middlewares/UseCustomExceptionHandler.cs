using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        //yazılan middleware program.cs üzerinde aktif edilmelidir. 
        //Extension metod yazabilmek için class ve metod static olmalıdır 
        public static void UseCustomException(this IApplicationBuilder app)
        {
            //IApplicationBuilder interface için bir extension metot yazılırsa bunu implemente etmiş tüm sınıflarda kullanılabilir. 
            //UseExceptionHandler bu midleware bir ex fırlatıldığında çalışır ve geriye bir model döner
            //Knedi modelimizi dönmek için
            app.UseExceptionHandler(config =>
            {
                //Run sonlandırıcı bir mddleware'dir. 
                //Request buraya girdikten sonra daha ileriye gitmez controller ve metotlara gitmez buradan geri döner.
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    //IExceptionHandlerFeature bu interface üzerinden uygulamada fırlatılan hata yakalanır. 
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    //Kendi fırlattığımız hatalar ve uygulama hatalarını ayırt edebilmek için kendi exception sınıfımızı tanımlayarak 
                    //gelen hatanın tipinden ayırt edebiliriz. 
                    var statusCode = exceptionFeature.Error switch
                    {
                        //Hatanın tipi ClientSide ise 400 değilse 500 
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                    //response bir tip olmasından dolayı geriye dönmek için json serialize edilmesi gerekir
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
