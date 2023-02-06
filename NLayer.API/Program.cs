using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

//Migration yapýlmasý için repo ve implementasyonlarýn eklenmesi
//generic olanlar typeopf ile kullanýlýr
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//sadece bir adet T entitiy generiz aldýðý için <> yeterli fakat çoklu olsaydý <,> her biri için , eklenecekti
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericeRepository<>));
//builder.Services.AddScoped(typeof(IService<>),typeof(Service<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
