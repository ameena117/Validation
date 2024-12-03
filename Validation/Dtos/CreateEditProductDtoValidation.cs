using FluentValidation;
using System.Data;
using System.Xml;
using Validation.Data;
using Validation.Models;

namespace Validation.Dtos
{
    public class CreateEditProductDtoValidation : AbstractValidator<CreateEditProductDto>
    {
        private readonly ApplicationDbContext _db;

        public CreateEditProductDtoValidation(ApplicationDbContext db)
        {
            _db = db;
            // name required => with custom message
            RuleFor(p => p.Name).NotEmpty().WithMessage("The Name is Required")
             // min length => 5 , max length => 30 , unique
            .MinimumLength(5).WithMessage("The Name Minimum 5")
            .MaximumLength(30).WithMessage("The Name Maximum 30")
            .Must(IsUnique).WithMessage("The Name must be unique");


            // price => required with custom message => range 20 => 3000
            RuleFor(p => p.Price).NotEmpty().WithMessage("The Price is Required")
            .InclusiveBetween(20,3000).WithMessage("The Price Rane 20-3000");

            //description => required with custom message => min length 10 
            RuleFor(p => p.Description).MinimumLength(10).WithMessage("The Description Minimum 10");
        }

        private bool IsUnique(string name)
        {
            var dbProduct = _db.Products.SingleOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (dbProduct == null)
                return true;

            return false;
        }

    }
}
