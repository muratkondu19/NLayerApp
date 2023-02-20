using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.Web.Modules;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using System.Reflection;
using NLayer.Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

//Auto mapper�n eklenmesi
builder.Services.AddAutoMapper(typeof(MapProfile)); //MapProfile'�n bulundu�u assembly'i typeof ile bulabilir

//AutoFac'in projeye dahil edilmesi
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//Mod�l eklenerek dinamik olarak ekleme i�lemlerinin yap�lmas�
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new RepoServiceModule());
});
var app = builder.Build();

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
