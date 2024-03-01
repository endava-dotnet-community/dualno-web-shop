using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain;
using FluentValidation;
using Models.Validators;
using Models.ViewModels;
using Services.Exceptions;

namespace Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _repository;
        private readonly IValidator<ShoppingCartViewModel> _viewModelValidator;
        private readonly IValidator<ShoppingCartItemViewModel> _viewModelItemValidator;

        public ShoppingCartService(
            IShoppingCartRepository shoppingcartRepository,
            IValidator<ShoppingCartViewModel> viewModelValidator,
            IValidator<ShoppingCartItemViewModel> viewModelItemValidator)
        {
            _repository = shoppingcartRepository;
            _viewModelValidator = viewModelValidator;
            _viewModelItemValidator = viewModelItemValidator;
        }

        //public async Task<bool> DeleteShoppingCartAsync(long cartId)
        //{
        //    return await _repository.DeleteShoppingCartAsync(cartId);
        //}

        public async Task<bool> DeleteShoppingCartItemAsync(long cartItemId)
        {
            return await _repository.DeleteShoppingCartItemAsync(cartItemId);
        }

        public async Task<List<ShoppingCartViewModel>> GetAllShoppingCartsAsync()
        {
            return (await _repository
                .GetAllShoppingCartsAsync())
                .Select<ShoppingCart, ShoppingCartViewModel>(p => MapToViewModel(p))
                .Where(p => p != null)
                .ToList();
        }

        public async Task<ShoppingCartViewModel> GetBySessionIdAsync(string sessionId)
        {
            return MapToViewModel(await _repository.GetBySessionIdAsync(sessionId));
        }

        //public async Task<bool> InsertShoppingCartAsync(ShoppingCartViewModel shoppingCartViewModel)
        //{
        //    _viewModelValidator.ValidateAndThrow(shoppingCartViewModel);
        //    ShoppingCart shoppingCart = MapFromViewModel(shoppingCartViewModel);
        //    if (shoppingCart == null)
        //        throw new ArgumentNullException(nameof(shoppingCartViewModel));
        //    return await _repository.InsertShoppingCartAsync(shoppingCart);
        //}

        public async Task<bool> InsertShoppingCartItemAsync(string sessionId ,ShoppingCartItemViewModel shoppingCartItemViewModel)
        {
            var shoppingCart = await _repository.GetBySessionIdAsync(sessionId);
            if(shoppingCart == null)
            {
                await _repository.InsertShoppingCartAsync(new ShoppingCart
                {
                    SessionId = sessionId,
                    AccessedAt = DateTime.UtcNow,
                });
                shoppingCart = await _repository.GetBySessionIdAsync(sessionId);
            }
            shoppingCartItemViewModel.CartId = shoppingCart.Id;
            shoppingCartItemViewModel.Id = 0;
            _viewModelItemValidator.ValidateAndThrow(shoppingCartItemViewModel);
            ShoppingCartItem shoppingCartItem = MapItemFromViewModel(shoppingCartItemViewModel);
            //Za validator kriterijumi shoppingCartItem == null || (shoppingCartItem.Id == null || shoppingCartItem.Id < 0) || (shoppingCartItem.CartId == null || shoppingCartItem.CartId < 0) || (shoppingCartItem.ProductId ==null || shoppingCartItem.ProductId <0) || shoppingCartItem.Quantity<0
            if (shoppingCartItem == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartItemViewModel));
            }
            await _repository.InsertShoppingCartItemAsync(shoppingCartItem);
            await _repository.UpdateAccessedAtAsync(shoppingCart.Id,DateTime.UtcNow);
            return true;
        }

        public async Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt)
        {
            return await _repository.UpdateAccessedAtAsync(cartId, accessedAt);
        }

        public async Task<bool> UpdateQuantityAsync(long cartItemId, int quantity)
        {
            return await _repository.UpdateQuantityAsync(cartItemId, quantity);
        }

        private static ShoppingCartViewModel MapToViewModel(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                return null;

            return new ShoppingCartViewModel
            {
                Id = shoppingCart.Id,
                AccessedAt = shoppingCart.AccessedAt,
                SessionId = shoppingCart.SessionId,
                Items = shoppingCart.Items.Select(s => MapItemToViewModel(s)).ToList()
            };
        }

        private static ShoppingCart MapFromViewModel(ShoppingCartViewModel shoppingCartViewModel)
        {
            if (shoppingCartViewModel == null)
                return null;

            return new ShoppingCart
            {
                Id = shoppingCartViewModel.Id,
                AccessedAt = shoppingCartViewModel.AccessedAt,
                SessionId = shoppingCartViewModel.SessionId,
                Items = shoppingCartViewModel.Items.Select(s => MapItemFromViewModel(s)).ToList()
            };
        }

        private static ShoppingCartItem MapItemFromViewModel(ShoppingCartItemViewModel itemViewModel)
        {
            if (itemViewModel == null)
                return null;

            return new ShoppingCartItem
            {
                Id = itemViewModel.Id,
                CartId = itemViewModel.CartId,
                ProductId = itemViewModel.ProductId,
                Quantity = itemViewModel.Quantity
            };
        }

        private static ShoppingCartItemViewModel MapItemToViewModel(ShoppingCartItem item)
        {
            if (item == null)
                return null;

            return new ShoppingCartItemViewModel
            {
                Id = item.Id,
                CartId = item.CartId,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }
    }
}