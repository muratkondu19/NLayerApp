using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.Web.Modules;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using System.Reflection;
using NLayer.Service.Mapping;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NLayer.Service.Validations;
using NLayer.Web;
using NLayer.Web.Serivces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //invalid filterý baskýla = true
    //framework'un döndüðü kendi model filtresini baskýlayarak yazýlan filtrenin kullanýlmasýný ve custom reponse dönmesini saðlama
    options.SuppressModelStateInvalidFilter = true;
}); ;

//appsetting.jsonda yer alan connection string için
builder.Services.AddDbContext<AppDbContext>(x =>
{
    //Configuration.GetConnectionString("SqlConnection") -> appsettingste olan SqlConnection ý alýr.
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),
        options =>
        {
            /*
             * Migration dosyalarý ve repositoryde oluþacak ve app db context’de repository katmanýnda olduðundan appdbcontextin bulunduðu assembly API Katmanýnda uygulamaya haberdar edilmelidir. bunun için options parametresi geçilir.
             * Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name-> bize NLayer.Repository assembly adýný verir
             */
            options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        });
});

builder.Services.AddScoped(typeof(NotFoundFilter<>));

//Auto mapperýn eklenmesi
builder.Services.AddAutoMapper(typeof(MapProfile)); //MapProfile'ýn bulunduðu assembly'i typeof ile bulabilir

//AutoFac'in projeye dahil edilmesi
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//Modül eklenerek dinamik olarak ekleme iþlemlerinin yapýlmasý
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new RepoServiceModule());
});

builder.Services.AddHttpClient<ProductApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["baseUrl"]);
});
builder.Services.AddHttpClient<CategoryApiService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["baseUrl"]);
});
var app = builder.Build();
app.UseExceptionHandler("/Home/Error");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
