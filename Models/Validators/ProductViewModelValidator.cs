using FluentValidation;
using Models.ViewModels;

namespace Models.Validators
{
    public class ProductViewModelValidator 
        : AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty();

            RuleFor(product => product.Category)
                .NotEmpty();

            RuleFor(product => product.Price)
                .NotEmpty()
                .InclusiveBetween(0, decimal.MaxValue);

            //RuleFor(product => product.Description)
            //    .Matches("^(\\w|\\s)*$")
            //    .MaximumLength(1024);
        }
    }
}