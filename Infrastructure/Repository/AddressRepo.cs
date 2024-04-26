using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AddressRepo : IAddressRepository
    {
        private readonly AppDbContext _appDbContext;

        public AddressRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ServiceResponse> AddasyncAddress(Address address)
        {
            _appDbContext.Addresses.Add(address);
            await saveChanges();
            return new ServiceResponse(true, "added");
        }

        private async Task saveChanges()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
