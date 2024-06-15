using Application.DTOs.Cart;
using Application.DTOs.Order;
using Application.Interfaces.Repositories;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<Order>> _orderRepository;
        private readonly Mock<IRepository<OrderProduct>> _orderProductRepository;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<ICartService> _cartService;
        private readonly Mock<IRepository<Product>> _productRepository;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderProductRepository = new Mock<IRepository<OrderProduct>>();
            _orderRepository = new Mock<IRepository<Order>>();
            _tokenService = new Mock<ITokenService>();
            _cartService = new Mock<ICartService>();
            _productRepository = new Mock<IRepository<Product>>();

            _orderService = new OrderService(
               _orderRepository.Object,
               _orderProductRepository.Object,
               _tokenService.Object,   
               _cartService.Object,     
               _productRepository.Object 
           );

          
        }

        [Fact]
        public async Task UpdateOrder_returnsSucessResponse_WhenItsSucess()
        {
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                OrderId = 1,
                UserId = "s12"
            };

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

            var paymentResponse = new PaymentCreation { 
                OrderId = 1,
                PaymentId ="p1",
                Status = "processed",
                TotalAmount = 1000,
                
            };


            var cartItems = new List<CartItemResponse>
            {
                new CartItemResponse { ProductId = 1,Quantity = 1}, new CartItemResponse { ProductId = 1,Quantity = 1}
            };
            _orderRepository
                .Setup(repo => repo.Get(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(order);

            _orderRepository
                .Setup(repo => repo.Update(It.IsAny<Order>()));

            _cartService
                .Setup(cart => cart.GetItems(It.IsAny<string>())).ReturnsAsync(cartItems);

            _productRepository
                .Setup(p => p.Get(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>(),It.IsAny<bool>())).ReturnsAsync(product);

            _productRepository
                .Setup(repo => repo.Update(It.IsAny<Product>()));

            _cartService
                .Setup(repo => repo.ClearCart(It.IsAny<string>()));


            var result = await  _orderService.UpdateOrder(paymentResponse);

            Assert.NotNull(result);
            Assert.Equal("Updated Order successfully",result.Message);





        }
    }
}
