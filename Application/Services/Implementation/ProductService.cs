using Application.DTOs;
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
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> products;
        public ProductService(IRepository<Product> _products)
        {
            products = _products;
        }
        public async Task<IEnumerable<ProductResponse>> getAllProducts()
        {

            var results = await products.GetAll(includePropeties: "ProductCategory");

            var productList = results.Select(p => new ProductResponse()
            {
                ProductId = p.ProductId,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Discount = p.Discount,
                ProductCategoryId = p.ProductCategoryId,
                ProductCategoryResponse = new ProductCategoryResponse()
                {
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory.CategoryName,
                    CategoryIcon = p.ProductCategory.CategoryIcon
                }

            }).ToList();


            if (productList != null)
            {

                return productList;

            }
            else
            {
                return Enumerable.Empty<ProductResponse>();
            }
        }

        public async Task<IEnumerable<ProductResponse>> getAllByCategory(int categoryId)
        {

            var results = await products.GetAll(filter: p => p.ProductCategoryId == categoryId, includePropeties: "ProductCategory");

            var productList = results.Select(p => new ProductResponse()
            {
                ProductId = p.ProductId,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Discount = p.Discount,
                ProductCategoryId = p.ProductCategoryId,
                ProductCategoryResponse = new ProductCategoryResponse()
                {
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory.CategoryName,
                    CategoryIcon = p.ProductCategory.CategoryIcon
                }

            }).ToList();

            if (productList != null)
            {

                return productList;

            }
            else
            {
                return Enumerable.Empty<ProductResponse>();
            }
        }

        public async Task<ServiceResponse> GetProductById(int id)
        {
            try
            {
                var product = await products.Get(filter: p => p.ProductId == id, includePropeties: "ProductCategory");
                if (product is not null)
                {
                    var response = new ProductResponse
                    {
                        ProductId = product.ProductId,
                        ImageUrl = product.ImageUrl,
                        Description = product.Description,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        Discount = product.Discount,
                        ProductCategoryId = product.ProductCategoryId,
                        ProductCategoryResponse = new ProductCategoryResponse
                        {
                            CategoryId = product.ProductCategoryId,
                            CategoryName = product.ProductCategory?.CategoryName,
                            CategoryIcon = product.ProductCategory?.CategoryIcon

                        }
                    };
                    return new ServiceResponse(true,Message: "You searched product is available",response);
                }
                else
                {
                    return (new ServiceResponse(false, Message: "Product Id is wrong"));
                }


            }catch(Exception ex)
            {
                return new ServiceResponse(false, Message: ex.Message);
            }
        }
    }
}
