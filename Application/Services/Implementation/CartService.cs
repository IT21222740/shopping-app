using Application.DTOs;
using Application.DTOs.Cart;
using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementation
{
    public class CartService:ICartService
    {
        private readonly IRepository<UserProduct> _cartItems;
        private readonly ITokenService _tokenService;
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<ICartService> _logger;

        
        public CartService(IRepository<UserProduct> cartItems,ITokenService tokenService, IRepository<Product> productRespository, ILogger<ICartService> logger) {
            _cartItems = cartItems;
            _tokenService = tokenService;
            _productRepository = productRespository;
            _logger = logger;
        }

        public async Task<ServiceResponse> AddCartItem(CartItemRequset item)
        {
            var userId = _tokenService.GetUserId();
            
            if(userId == null)
            {
                _logger.LogError("User Authorization failde");
                return new ServiceResponse(false, "Login session expired. Please Login");
            }

            UserProduct? cartItem = await GetCartItem(item.ProductId);

            if(cartItem != null)
            {
                cartItem.Quantity = item.Quantity;
                await _cartItems.Update(cartItem);

                _logger.LogInformation("Cart Item updated Sucessfully");

                return new ServiceResponse(true, "Updated");
            }

            UserProduct newCartItem = new UserProduct()
            {
                userId = userId,
                Quantity = item.Quantity,
                ProductId = item.ProductId,
            };
           await _cartItems.Add(newCartItem);

            _logger.LogInformation("Cart Items sucessfully added");

            return new ServiceResponse(true, "Added successfully");
           
        }

        public async Task<ServiceResponse> ModifyCartItem(CartItemRequset item)
        {

            UserProduct? cartItem = await GetCartItem(item.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity = item.Quantity;
                await _cartItems.Update(cartItem);

                _logger.LogInformation("Cart Items updated Sucessfully");
                return new ServiceResponse(true, "Updated");
            }
            else
            {
                _logger.LogError("Cart Item not found");
                return new ServiceResponse(false, "Cart Item not found");
            }
        }

        public async Task<ServiceResponse> RemoveCartItem(CartItemRequset item)
        {
            UserProduct? cartItem = await GetCartItem(item.ProductId);
            if (cartItem != null)
            {
                await _cartItems.Remove(cartItem);
                _logger.LogInformation("Cart Item remove successfully");
                return new ServiceResponse(true, "Removed Sucessfully");
            }
            else
            {
                _logger.LogError("Cart Item deletion unsucessful");
                return new ServiceResponse(false,"wrong Product ID");
            }
           
            
        }

        public async Task<ServiceResponse> ViewCart()
        {
            var userId = _tokenService.GetUserId();

            if (userId == null)
            {
                return new ServiceResponse(false, "Login session expired. Please Login");
            }
            var userCartitems = await _cartItems.GetAll(filter: up => up.userId == userId, includePropeties: "Product");

            var cart = userCartitems.Select(up => new CartItemResponse
            {

                ProductId = up.ProductId,
                Quantity = up.Quantity,
                Product = new ProductCartResponse()
                {
                    ProductId = up.ProductId,
                    ImageUrl = up.Product.ImageUrl,
                    Description = up.Product.Description,
                    Name = up.Product.Name,
                    Price= up.Product.Price,
                    Quantity= up.Product.Quantity,
                    Discount= up.Product.Discount,
                    ProductCategoryId=up.Product.ProductCategoryId

                }

            }).ToList();
            return new ServiceResponse(true, "Cart Items", cart);
           
        }
        public async Task<IEnumerable<CartItemResponse>> GetItems(string id)
        {
            

                   
            var userCartitems = await _cartItems.GetAll(filter: up => up.userId == id, includePropeties: "Product");

            var cart = userCartitems.Select(up => new CartItemResponse
            {

                ProductId = up.ProductId,
                Quantity = up.Quantity

            }).ToList();

            return cart;
        }

        public async Task<UserProduct?> GetCartItem(int itemId)
        {
            var userId = _tokenService.GetUserId();

            UserProduct? cartItem = await _cartItems.Get(filter: up => up.userId == userId && up.ProductId == itemId);

            return cartItem;


        }

        public async Task<ServiceResponse> ClearCart(string UserId)
        {
            var list = await _cartItems.GetAll(filter: up => up.userId == UserId);
            await _cartItems.RemoveMany(list.ToList());
            return new ServiceResponse(true, "deleted Cart Items");

        }

        public async Task<bool> checkAvailabilty()
        {
            var userId = _tokenService.GetUserId();
            var cart = await _cartItems.GetAll(c=>c.userId == userId);
            foreach (var item in cart) {
                var product = await _productRepository.Get(p => p.ProductId == item.ProductId);
                if(product != null)
                {
                    if(product.Quantity < item.Quantity)
                    {
                        return false;
                    }
                }
                
                
            }
            return true;
        }
    }
}
