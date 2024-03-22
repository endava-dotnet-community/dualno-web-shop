using FluentValidation;
using Models.ViewModels;

namespace Models.Validators
{
    public class ShoppingCartItemViewModelValidator
        : AbstractValidator<ShoppingCartItemViewModel>
    {
        public ShoppingCartItemViewModelValidator()
        {
            //Za validator kriterijumi shoppingCartItem == null || (shoppingCartItem.Id == null || shoppingCartItem.Id < 0) || (shoppingCartItem.CartId == null || shoppingCartItem.CartId < 0) || (shoppingCartItem.ProductId ==null || shoppingCartItem.ProductId <0) || shoppingCartItem.Quantity<0

            RuleFor(shoppingCart => shoppingCart)
                .NotNull();

            RuleFor(shoppingCart => shoppingCart.Id)
                .GreaterThanOrEqualTo<ShoppingCartItemViewModel, long>(shoppingCart => 0);


        }
    }
}