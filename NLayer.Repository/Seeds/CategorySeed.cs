using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Veritabanına ilgili tablolar oluşurken default bazı dataların atılmasıdır.

Migration yapılırken ilgili tabloların oluşma esnasında tablolarla ilgili default dataların atılması ve ya migration yapıp tablolar oluştuğunda uygulama ayağa kalktığında da data atabiliriz. 

Migration yapılırken seed data oluşumu sağlanacak .
 */
namespace NLayer.Repository.Seeds
{
    internal class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Migration esnasında seed oluşturmak istenirse id değerlerini kendimiz vermemiz gerekiyor
            builder.HasData(
                new Category { Id = 1, Name = "Kalemler" },
                new Category { Id = 2, Name = "Kitaplar" },
                new Category { Id = 3, Name = "Defterler" }
                );
        }
    }
}
