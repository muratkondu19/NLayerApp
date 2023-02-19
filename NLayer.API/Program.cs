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
    //invalid filterý baskýla = true
    //framework'un döndüðü kendi model filtresini baskýlayarak yazýlan filtrenin kullanýlmasýný ve custom reponse dönmesini saðlama
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
/*
 * Eðer bir filter’ýn ctor’unda herhangi bir servisi ya da herhangi bir class’ý kullanýyorsa bunun startup tarafýna da eklenmesi gerekmektedir. 
 */
builder.Services.AddScoped(typeof(NotFoundFilter<>));
//Migration yapýlmasý için repo ve implementasyonlarýn eklenmesi
//generic olanlar typeopf ile kullanýlýr
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//sadece bir adet T entitiy generiz aldýðý için <> yeterli fakat çoklu olsaydý <,> her biri için , eklenecekti
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericeRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

//Auto mapperýn eklenmesi
builder.Services.AddAutoMapper(typeof(MapProfile)); //MapProfile'ýn bulunduðu assembly'i typeof ile bulabilir

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

//Yazýlan middleware eklenmesi 
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
