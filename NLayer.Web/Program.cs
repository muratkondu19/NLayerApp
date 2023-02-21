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
    //invalid filter� bask�la = true
    //framework'un d�nd��� kendi model filtresini bask�layarak yaz�lan filtrenin kullan�lmas�n� ve custom reponse d�nmesini sa�lama
    options.SuppressModelStateInvalidFilter = true;
}); ;

//appsetting.jsonda yer alan connection string i�in
builder.Services.AddDbContext<AppDbContext>(x =>
{
    //Configuration.GetConnectionString("SqlConnection") -> appsettingste olan SqlConnection � al�r.
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),
        options =>
        {
            /*
             * Migration dosyalar� ve repositoryde olu�acak ve app db context�de repository katman�nda oldu�undan appdbcontextin bulundu�u assembly API Katman�nda uygulamaya haberdar edilmelidir. bunun i�in options parametresi ge�ilir.
             * Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name-> bize NLayer.Repository assembly ad�n� verir
             */
            options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        });
});

builder.Services.AddScoped(typeof(NotFoundFilter<>));

//Auto mapper�n eklenmesi
builder.Services.AddAutoMapper(typeof(MapProfile)); //MapProfile'�n bulundu�u assembly'i typeof ile bulabilir

//AutoFac'in projeye dahil edilmesi
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//Mod�l eklenerek dinamik olarak ekleme i�lemlerinin yap�lmas�
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
