using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    //IEntityTypeConfiguration iplemente edilmelidir
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Entity configurasyonları burada yapılır
            builder.HasKey(x => x.Id); //Id değerinin belirtilmesi
            builder.Property(x => x.Id).UseIdentityColumn(); //id değerinin 1+ artmasını sağlar
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50); //Name zorunlu ve 50 karakter olsun

            builder.ToTable("Categories"); //Tablo ismi belirtilir / verilmez ise default olarak db sette verilen prop adını alır

        }
    }
}
