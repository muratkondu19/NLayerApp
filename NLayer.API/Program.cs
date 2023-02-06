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

//Migration yap�lmas� i�in repo ve implementasyonlar�n eklenmesi
//generic olanlar typeopf ile kullan�l�r
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//sadece bir adet T entitiy generiz ald��� i�in <> yeterli fakat �oklu olsayd� <,> her biri i�in , eklenecekti
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
