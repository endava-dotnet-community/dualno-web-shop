using FluentValidation;
using Models.ViewModels;

namespace Models.Validators
{
    public class ShoppingCartItemViewModelValidator
        : AbstractValidator<ShoppingCartItemViewModel>
    {
        public ShoppingCartItemViewModelValidator()
        {
            //RuleFor(category => category.Name)
            //    .NotEmpty();

            //RuleFor(category => category.Name)
            //    .Matches("^(\\w|\\s)*$")
            //    .MaximumLength(1024);
        }
    }
}