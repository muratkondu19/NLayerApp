using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.Web.Modules
{
    public class RepoServiceModule:Module
    {
        //Load ovveride edilir 
        protected override void Load(ContainerBuilder builder)
        {
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericeRepository<>)); -mevcutta kullanılan
            //builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));  -mevcutta kullanılan
            builder.RegisterGeneric(typeof(IGenericeRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  -mevcutta kullanılan
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            //Önce assembly'ler alınır, AutoFac assembly'leri tarayacak ve içerisinden istenilen tüm interface ve class'ları dinamik olarak ekleyecek
            var apiAssembly = Assembly.GetExecutingAssembly(); //üzerinde çalıştığı API assembly'sini alır.
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //Repository katmanında herhangi birisi verilebilir ->AppDbContext
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            //sınıflardan Repository ile bitenleri al ve o sınıflara karşılık gelen interface'leri al 
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, apiAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //InstancePerLifetimeScope bu metod ASP.NET Core'daki Scoped'a karşılık gelmektedir. 
            //InstancePerDependency ise transient'e karşılık gelmektedir. 

            //sınıflardan Service ile bitenleri al ve o sınıflara karşılık gelen interface'leri al 
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, apiAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //IProductService interface gördüğün zaman Product Service değil ProductServiceWithCaching'in nesne örneğini al
           // builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();
        }
    }
}
