using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Services.Implementation;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;



namespace UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _repositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Product>>();
            _productService = new ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                ProductId = productId,
                ImageUrl = "image1.png",
                Description = "Description1",
                Name = "Product1",
                Price = 10,
                Quantity = 5,
                Discount = 2,
                ProductCategoryId = 1,
                ProductCategory = new ProductCategory { CategoryId = 1, CategoryName = "Category1", CategoryIcon = "icon1.png" }
            };

            _repositoryMock.Setup(repo => repo.Get(
                It.Is<Expression<Func<Product, bool>>>(filter => filter.Compile()(product)),
                It.IsAny<string>(),
                It.IsAny<bool>())
            ).ReturnsAsync(product);


            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.True(result.Flag);
            Assert.Equal("You searched product is available",result.Message);
        }
        [Fact]
         public async Task getAllByCategory_ReturnsProducts_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var products = new List<Product>
        {
            new Product
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
            },
            new Product
            {
                ProductId = 2,
                ImageUrl = "image2.png",
                Description = "Description2",
                Name = "Product2",
                Price = 20,
                Quantity = 10,
                Discount = 3,
                ProductCategoryId = 1,
                ProductCategory = new ProductCategory { CategoryId = 1, CategoryName = "Category1", CategoryIcon = "icon1.png" }
            }
        };


            _repositoryMock.Setup(repo => repo.GetAll(
            It.IsAny<Expression<Func<Product, bool>>>(), 
            It.IsAny<string>(),
            It.IsAny<bool>()
            )).ReturnsAsync(products);


            // Act
            var result = await _productService.getAllByCategory(1);

         
            //Assert.Equal(products,result);
        }
    }
}
