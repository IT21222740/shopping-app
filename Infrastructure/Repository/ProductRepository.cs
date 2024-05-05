using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext _context) {
            _appDbContext = _context;
        }
        public async Task<IEnumerable<ProductDTO>> GetAsync()
        {
            var products = await _appDbContext.Products
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Discount = p.Discount,
                    ProductCategoryId = p.ProductCategoryId 
                })
            .ToListAsync();

            return products;
            
        }
        


    }
}
