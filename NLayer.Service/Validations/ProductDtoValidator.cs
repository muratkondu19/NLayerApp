using FluentValidation;
using NLayer.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Validations
{
    //AbstractValidator ->FluentValidation kütüphanesine aittir
    //AbstractValidator<ProductDto> -> ProductDto validate edilecek
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            //PropertyName->Name ismini vermektedir. 
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is not empty");
            //Price decimal olduğundan dolayı değer girilmese bile 0 olarak gelir bu sebeple not null işe yaramaz
            //value type'ların default değerleri olduğundan not null empty kullanmak uygun olmaz
            RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be grater 0");
            RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be grater 0");
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be grater 0");
        }
    }
}
