using Application.DTOs;
using Application.DTOs.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResponse> SignUpAsync(SignUpDTO newUser);

        public Task<LoginResponse> LoginAsync(LoginDTO user);
        public Task<ServiceResponse> AddAddress(AddressDTO addressDto);

    }
}
