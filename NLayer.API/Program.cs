using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>  options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //invalid filter� bask�la = true
    //framework'un d�nd��� kendi model filtresini bask�layarak yaz�lan filtrenin kullan�lmas�n� ve custom reponse d�nmesini sa�lama
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
/*
 * E�er bir filter��n ctor�unda herhangi bir servisi ya da herhangi bir class�� kullan�yorsa bunun startup taraf�na da eklenmesi gerekmektedir. 
 */
builder.Services.AddScoped(typeof(NotFoundFilter<>));
//Migration yap�lmas� i�in repo ve implementasyonlar�n eklenmesi
//generic olanlar typeopf ile kullan�l�r
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//sadece bir adet T entitiy generiz ald��� i�in <> yeterli fakat �oklu olsayd� <,> her biri i�in , eklenecekti
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericeRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

//Auto mapper�n eklenmesi
builder.Services.AddAutoMapper(typeof(MapProfile)); //MapProfile'�n bulundu�u assembly'i typeof ile bulabilir

//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Yaz�lan middleware eklenmesi 
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
