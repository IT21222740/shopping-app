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

namespace Application.Services.Implementation
{
    public class CartService:ICartService
    {
        private readonly IRepository<UserProduct> _cartItems;
        private readonly ITokenService _tokenService;
        public CartService(IRepository<UserProduct> cartItems,ITokenService tokenService) {
            _cartItems = cartItems;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse> AddCartItem(CartItemRequset item)
        {
            var userId = _tokenService.GetUserId();
            
            if(userId == null)
            {
                return new ServiceResponse(false, "Login session expired. Please Login");
            }

            UserProduct? cartItem = await GetCartItem(item.ProductId);

            if(cartItem != null)
            {
                cartItem.Quantity = item.Quantity;
                await _cartItems.Update(cartItem);
                return new ServiceResponse(true, "Updated");
            }

            UserProduct newCartItem = new UserProduct()
            {
                userId = userId,
                Quantity = item.Quantity,
                ProductId = item.ProductId,
            };
           await _cartItems.Add(newCartItem);
            return new ServiceResponse(true, "Added successfully");
           
        }

        public async Task<ServiceResponse> ModifyCartItem(CartItemRequset item)
        {

            UserProduct? cartItem = await GetCartItem(item.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity = item.Quantity;
                await _cartItems.Update(cartItem);
                return new ServiceResponse(true, "Updated");
            }
            else
            {
                return new ServiceResponse(false, "Cart Item not found");
            }
        }

        public async Task<ServiceResponse> RemoveCartItem(CartItemRequset item)
        {
            UserProduct? cartItem = await GetCartItem(item.ProductId);
            if (cartItem != null)
            {
                await _cartItems.Remove(cartItem);
                return new ServiceResponse(true, "Removed Sucessfully");
            }
            else
            {
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

        public async Task<UserProduct?> GetCartItem(int itemId)
        {
            var userId = _tokenService.GetUserId();

            UserProduct? cartItem = await _cartItems.Get(filter: up => up.userId == userId && up.ProductId == itemId);

            return cartItem;


        }
    }
}
