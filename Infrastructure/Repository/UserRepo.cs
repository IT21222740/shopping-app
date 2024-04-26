using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{

    public class UserRepo : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;    
        }
       

        public async Task<ServiceResponse> CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            await saveChanges();
            return new ServiceResponse(true, "Added");
        }

        public Task<ServiceResponse> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        private async Task saveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

