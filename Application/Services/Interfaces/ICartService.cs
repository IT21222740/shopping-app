using Application.DTOs;
using Application.DTOs.Cart;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICartService
    {
        public Task<ServiceResponse> AddCartItem(CartItemRequset item);
        public Task<ServiceResponse> ModifyCartItem(CartItemRequset item);
        public Task<ServiceResponse> RemoveCartItem(CartItemRequset item);
        public Task<UserProduct?> GetCartItem(int itemId);

        public Task<ServiceResponse> ViewCart();
        Task<ServiceResponse> ClearCart(string UserId);

        Task<IEnumerable<CartItemResponse>> GetItems(string id);
        Task<bool> checkAvailabilty();
    }
}
