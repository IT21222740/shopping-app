using Application.DTOs;
using Application.DTOs.Cart;
using Application.Interfaces.Repositories;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CartSerrviceTests
    {
        private readonly Mock<IRepository<UserProduct>> _mockUserProductRepo;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<ILogger<ICartService>> _mockLogger;
        private readonly CartService _cartService;
        
        public CartSerrviceTests()
        {
            _mockUserProductRepo = new Mock<IRepository<UserProduct>>();
            _mockTokenService = new Mock<ITokenService>();
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockLogger = new Mock<ILogger<ICartService>>();
            
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
           



            _mockUserProductRepo
                .Setup(c => c.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(cartItem)),
                It.IsAny<string>(),
                It.IsAny<bool>())).ReturnsAsync(cartItem);

            _mockUserProductRepo
                .Setup(c => c.Update(It.IsAny<UserProduct>()));
            
            var result = await _cartService.AddCartItem(newItem);

            Assert.Equal("Cart Item Updated Sucessfully", result.Message);


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
           
            _mockUserProductRepo
                .Setup(c => c.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(cartItem)),
                It.IsAny<string>(),
                It.IsAny<bool>()));

            _mockUserProductRepo
                .Setup(c => c.Add(It.IsAny<UserProduct>()));

            var result = await _cartService.AddCartItem(newItem);

            Assert.Equal("Added successfully", result.Message);


        }

        [Fact]
        public async Task ClearCart_throwsAnException_whenNoitemsfound()
        {
            var userId = "123";
            _mockTokenService
                .Setup(t => t.GetUserId()).Returns(userId);
            _mockUserProductRepo
            .Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserProduct, bool>>>(),It.IsAny<string>(),It.IsAny<bool>()));

            var exception = await Assert.ThrowsAsync<Exception>(()=>_cartService.ClearCart(userId));
            Console.WriteLine(exception.Message);
            Assert.Equal("Wrong operation: No cart items found for the specified user.", exception.Message);



        }
        
        [Fact]
        public async Task ClearCart_retrunsSucessResponse_whenItemsDeletedFromTheCart()
        {
            var userId = "123";
            var userProducts = new List<UserProduct> { new UserProduct { userId = userId, ProductId = 1, Quantity = 1 }, new UserProduct { userId = userId, ProductId = 1, Quantity = 1 } };
           

            _mockTokenService
                .Setup(t => t.GetUserId()).Returns(userId);
            _mockUserProductRepo
            .Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserProduct, bool>>>(),It.IsAny<string>(),It.IsAny<bool>())).ReturnsAsync(userProducts);

           
            _mockUserProductRepo
                .Setup(repo => repo.RemoveMany(It.IsAny<List<UserProduct>>()));

            ServiceResponse response = await _cartService.ClearCart(userId);

            Assert.True(response.Flag);
            Assert.Equal("deleted Cart Items", response.Message);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task checkAvailabilty_retrunsTrue_whenStockAvailable()
        {
            var userId = "123";

            var product = new Product
            {
                ProductId = 1,
                ImageUrl = "image1.png",
                Description = "Description1",
                Name = "Product1",
                Price = 10,
                Quantity = 5,
                Discount = 2,
                ProductCategoryId = 1,
            };


            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 1 };
            var userProducts = new List<UserProduct> { userProduct, userProduct };


            _mockTokenService
              .Setup(t => t.GetUserId()).Returns(userId);
            _mockUserProductRepo
            .Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserProduct, bool>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProducts);

            _mockProductRepository.Setup(repo => repo.Get(
              It.Is<Expression<Func<Product, bool>>>(filter => filter.Compile()(product)),
              It.IsAny<string>(),
              It.IsAny<bool>())).
              ReturnsAsync(product);

            bool response = await _cartService.checkAvailabilty();

            
            Assert.NotNull(response);
            Assert.True(response);

            

        }
        
        [Fact]
        public async Task checkAvailabilty_retrunsFalse_whenStockNotAvailable()
        {
            var userId = "123";

            var product = new Product
            {
                ProductId = 1,
                ImageUrl = "image1.png",
                Description = "Description1",
                Name = "Product1",
                Price = 10,
                Quantity = 5,
                Discount = 2,
                ProductCategoryId = 1,
            };


            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };
            var userProducts = new List<UserProduct> { userProduct, userProduct };


            _mockTokenService
              .Setup(t => t.GetUserId()).Returns(userId);
            _mockUserProductRepo
            .Setup(repo => repo.GetAll(It.IsAny<Expression<Func<UserProduct, bool>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProducts);

            _mockProductRepository.Setup(repo => repo.Get(
              It.Is<Expression<Func<Product, bool>>>(filter => filter.Compile()(product)),
              It.IsAny<string>(),
              It.IsAny<bool>())).
              ReturnsAsync(product);

            bool response = await _cartService.checkAvailabilty();

            
            Assert.NotNull(response);
            Assert.False(response);

        }

        [Fact]
        public async Task GetCartItem_retrunsCartItem_WhenItExcists()
        {
            string userId = "s123";
            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };

            _mockTokenService
             .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
           .Setup(repo => repo.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProduct);

            var response = await _cartService.GetCartItem(1);

            Assert.NotNull(response);
            Assert.Equal(userProduct, response);

        }

        [Fact]
        public async Task GetCartItems_retrunsCartItem_WhenItExcists()
        {
            string userId = "s123";
            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };
            var userProducts = new List<UserProduct> { userProduct, userProduct };

            _mockTokenService
             .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
           .Setup(repo => repo.GetAll(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProducts);

            IEnumerable<CartItemResponse> response = await _cartService.GetItems(userId);

            Assert.NotNull(response);
            
            foreach(var item in response)
            {
                Assert.Equal(userProduct.Quantity, item.Quantity);
            }

        }

        [Fact]
        public async Task View_retrunsSuccessResponse_WhenItsucess()
        {
            string userId = "s123";
            
            var product = new Product
            {
                ProductId = 1,
                ImageUrl = "image1.png",
                Description = "Description1",
                Name = "Product1",
                Price = 10,
                Quantity = 5,
                Discount = 2,
                ProductCategoryId = 1,
                ProductCategory = new ProductCategory { CategoryId = 1, CategoryName = "Category1", CategoryIcon = "icon1.png" }
            };

            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6,Product= product};

            var userProducts = new List<UserProduct> { userProduct, userProduct };


            _mockTokenService
            .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
            .Setup(repo => repo.GetAll(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProducts);

            var respone = await _cartService.ViewCart();

            Assert.Equal("Cart Items", respone.Message);
            Assert.NotNull(respone);
            Assert.True(respone.Flag);
        }
        
        [Fact]
        public async Task View_retrunsFalse_WhenUserIdDoesNotExcists()
        {
            string userId = "s123";

            _mockTokenService
            .Setup(t => t.GetUserId());

            var respone = await _cartService.ViewCart();

            Assert.Equal("Login session expired. Please Login", respone.Message);
            Assert.NotNull(respone);
            Assert.False(respone.Flag);
        }

        [Fact]
        public async Task View_retrunsSuccessMessage_WhenCartItemDeletedSuccessfull()
        {
            string userId = "s123";
            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };

            _mockTokenService
             .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
           .Setup(repo => repo.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProduct);

            _mockUserProductRepo
                .Setup(up => up.Remove(It.IsAny<UserProduct>()));

            var response = await _cartService.RemoveCartItem(1);

            Assert.NotNull(response);
            Assert.True(response.Flag);
            Assert.Equal("Removed Sucessfully", response.Message);
            
        }
        
        [Fact]
        public async Task View_retrunsFalse_WhenCartItemDeletingUnsuccess()
        {
            string userId = "s123";
            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };

            _mockTokenService
             .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
           .Setup(repo => repo.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>()));

            var response = await _cartService.RemoveCartItem(1);

            Assert.NotNull(response);
            Assert.False(response.Flag);
            Assert.Equal("wrong Product ID", response.Message);
            
        }

        [Fact]
        public async Task ModifyCartIte_returnSuccess_WhenCartItemModifiedSuccessfully()
        {
            string userId = "s123";
            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };
            CartItemRequset item = new CartItemRequset
            {
                ProductId = 1,
                Quantity = 1,
            };

            _mockTokenService
             .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
           .Setup(repo => repo.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(userProduct);

            _mockUserProductRepo
                .Setup(repo => repo.Update(It.IsAny<UserProduct>()));


            var response = await _cartService.ModifyCartItem(item);

            Assert.NotNull(response);
            Assert.True(response.Flag);
            Assert.Equal("Cart Items updated Sucessfully", response.Message);

        }

        [Fact]
        public async Task ModifyCartItem_returnFalse_WhenCartItemModifiedUnSuccessfully()
        {
            string userId = "s123";
            var userProduct = new UserProduct { userId = userId, ProductId = 1, Quantity = 6 };
            CartItemRequset item = new CartItemRequset
            {
                ProductId = 1,
                Quantity = 1,
            };

            _mockTokenService
             .Setup(t => t.GetUserId()).Returns(userId);

            _mockUserProductRepo
           .Setup(repo => repo.Get(It.Is<Expression<Func<UserProduct, bool>>>(filter => filter.Compile()(userProduct)), It.IsAny<string>(), It.IsAny<bool>()));

          

            var response = await _cartService.ModifyCartItem(item);

            Assert.NotNull(response);
            Assert.False(response.Flag);
            Assert.Equal("Cart Item not found", response.Message);

        }

    }
}

