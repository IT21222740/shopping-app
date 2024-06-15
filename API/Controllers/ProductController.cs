using Application.DTOs;
using Application.DTOs.Product;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of all available products.</returns>
        [HttpGet("all-products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<ProductResponse>> GetProducts()
        {
            var results = await  _productService.getAllProducts();
           return results;
        }

      
        /// <summary>
        /// Retrieves all products by category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A list of products that belong to the specified category.</returns>
        [HttpGet("products-by-category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<ProductResponse>> GetProductsByCategory(int categoryId)
        {
            var results = await _productService.getAllByCategory(categoryId);
            return results;
        }


        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>A ServiceResponse object containing the product details or an error message.</returns>
        [HttpGet("products-by-id/{productId:int}")]
        public async Task<ServiceResponse> GetProductById(int productId)
        {
            var result = await _productService.GetProductById(productId);
            return result;
        }
    }
}
