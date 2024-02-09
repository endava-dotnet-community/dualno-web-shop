using FluentValidation;
using Models.ViewModels;

namespace Models.Validators
{
    public class ShoppingCartViewModelValidator
        : AbstractValidator<ShoppingCartViewModel>
    {
        public ShoppingCartViewModelValidator()
        {
            //RuleFor(category => category.Name)
            //    .NotEmpty();

            //RuleFor(category => category.Name)
            //    .Matches("^(\\w|\\s)*$")
            //    .MaximumLength(1024);
        }
    }
}