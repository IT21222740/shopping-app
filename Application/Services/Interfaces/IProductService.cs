using Application.DTOs;
using Application.DTOs.Product;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> getAllProducts();
        Task<IEnumerable<ProductResponse>> getAllByCategory(int categoryId);

        Task<ServiceResponse> GetProductById(int id);




    }
}
