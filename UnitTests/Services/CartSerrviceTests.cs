using Application.DTOs.Cart;
using Application.Interfaces.Repositories;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class CartSerrviceTests
    {
        private readonly Mock<IRepository<UserProduct>> _mockUserProductRepo;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<ILogger<ICartService>> _mockLogger;
        private readonly CartService _cartService;
        private readonly Mock<ICartService> _mockCartService;
        public CartSerrviceTests()
        {
            _mockUserProductRepo = new Mock<IRepository<UserProduct>>();
            _mockTokenService = new Mock<ITokenService>();
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockLogger = new Mock<ILogger<ICartService>>();
            _mockCartService = new Mock<ICartService>();

            _cartService = new CartService(
                cartItems: _mockUserProductRepo.Object,
                tokenService: _mockTokenService.Object,
                productRespository: _mockProductRepository.Object,
                 logger: _mockLogger.Object
                );



        }

        [Fact]
        public async Task AddCartItem_returnsSucessResponse_WhenItsUpdateSucess()
        {

            string userId = "s123";
            UserProduct cartItem = new UserProduct
            {
                userId = userId,
                ProductId = 1,
                Quantity = 1,
            };

            CartItemRequset newItem = new CartItemRequset
            {
                ProductId = 1,
                Quantity = 1,
            };

            _mockTokenService
            .Setup(t => t.GetUserId()).Returns(userId);
            //_mockLogger
            //    .Setup(logger => logger.LogError("error"));


            _mockUserProductRepo
                .Setup(c => c.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(cartItem)),
                It.IsAny<string>(),
                It.IsAny<bool>())).ReturnsAsync(cartItem);

            _mockUserProductRepo
                .Setup(c => c.Update(It.IsAny<UserProduct>()));
            
            var result = await _cartService.AddCartItem(newItem);

            Assert.Equal("Updated", result.Message);


        }
        [Fact]
        public async Task AddCartItem_returnsSucessResponse_WhenItsAdditionSucess()
        {

            string userId = "s123";
            UserProduct cartItem = new UserProduct
            {
                userId = userId,
                ProductId = 1,
                Quantity = 1,
            };

            CartItemRequset newItem = new CartItemRequset
            {
                ProductId = 1,
                Quantity = 1,
            };

            _mockTokenService
            .Setup(t => t.GetUserId()).Returns(userId);
            //_mockLogger
            //    .Setup(logger => logger.LogError("error"));


            _mockUserProductRepo
                .Setup(c => c.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(cartItem)),
                It.IsAny<string>(),
                It.IsAny<bool>()));

            _mockUserProductRepo
                .Setup(c => c.Add(It.IsAny<UserProduct>()));

            var result = await _cartService.AddCartItem(newItem);

            Assert.Equal("Added successfully", result.Message);


        }
    }
}

