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

        public ShoppingCartService(
            IShoppingCartRepository shoppingcartRepository,
            IValidator<ShoppingCartViewModel> viewModelValidator)
        {
            _repository = shoppingcartRepository;
            _viewModelValidator = viewModelValidator;
        }

        public async Task<bool> DeleteShoppingCartAsync(long cartId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteShoppingCartItemAsync(long cartItemId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShoppingCartViewModel>> GetAllShoppingCartsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCartViewModel> GetBySessionIdAsync(string sessionId)
        {
            return MapToViewModel(await _repository.GetBySessionIdAsync(sessionId));
        }

        public async Task<bool> InsertShoppingCartAsync(ShoppingCartViewModel shoppingCart)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartItemAsync(ShoppingCartItemViewModel shoppingCartItem)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuantityAsync(long cartItemId, int quantity)
        {
            throw new NotImplementedException();
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