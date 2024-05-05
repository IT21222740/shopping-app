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

        [HttpGet("all-products")]
        public async Task<IEnumerable<ProductResponse>> GetProducts()
        {
            var results = await  _productService.getAllProducts();
           return results;
        }

        [HttpPost("products-by-category")]
        public async Task<IEnumerable<ProductResponse>> GetProductsVyCategory([FromBody] CategoryRequest categoryRequest)
        {
            var results = await _productService.getAllByCategory(categoryRequest.CategoryId);
            return results;
        }
    }
}
