using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Application.DTOs;

namespace UnitTests.Services
{
    public class CheckoutServiceTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IPamentService> _mockPamentService;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<UserProduct>> _mockUserProducts;
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IRepository<OrderProduct>> _mockOrderProducts;
        private readonly Mock<ICartService> _mockCartService;
        private readonly CheckoutService _checkoutService;

        public CheckoutServiceTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockPamentService = new Mock<IPamentService>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserProducts = new Mock<IRepository<UserProduct>>();
            _mockOrderService = new Mock<IOrderService>();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockOrderProducts = new Mock<IRepository<OrderProduct>>();
            _mockCartService = new Mock<ICartService>();

            _checkoutService = new CheckoutService(
                _mockTokenService.Object,
                _mockPamentService.Object,
                _mockUserRepository.Object,
                _mockUserProducts.Object,
                _mockOrderService.Object,
                _mockEmailSender.Object,
                _mockOrderProducts.Object,
                _mockCartService.Object
            );
        }

        [Fact]
       public async Task Checkout_ReturnsUrl_WhenCheckoutIsSuccessful()
        {
            // Arrange
            var userId = "s123";
            var user = new User { UserId = userId, StripeId = "stripe_123", Email = "user@gmial.com" };
            var userProducts = new List<UserProduct> { new UserProduct { userId = userId, ProductId = 1, Quantity = 1 }, new UserProduct { userId = userId, ProductId = 1, Quantity = 1 } };
            var paymentUrl = "http://payment-url";
            var orderResponse = new ServiceResponse(true, Message:"sample Message", data:"123") ;

            _mockTokenService.Setup(s => s.GetUserId()).Returns(userId);
            _mockUserRepository.Setup(repo => repo.Get(
                It.Is<Expression<Func<User, bool>>>(filter => filter.Compile()(user)),
                It.IsAny<string>(),
                It.IsAny<bool>())
            ).ReturnsAsync(user);

            _mockUserProducts.Setup(repo => repo.GetAll(
           It.IsAny<Expression<Func<UserProduct, bool>>>(), // Match any expression
           It.IsAny<string>(),
           It.IsAny<bool>()
           )).ReturnsAsync(userProducts);
            _mockCartService.Setup(s => s.checkAvailabilty()).ReturnsAsync(true);
            _mockOrderService.Setup(s => s.AddOrder(userProducts)).ReturnsAsync(orderResponse);
            _mockPamentService.Setup(s => s.Checkout(user.StripeId, userProducts, orderResponse.data.ToString())).ReturnsAsync(paymentUrl);

            // Act
            var result = await _checkoutService.checkout();

            // Assert
            Assert.Equal(paymentUrl, result);
            Assert.NotNull(result);
        }



    }
}


