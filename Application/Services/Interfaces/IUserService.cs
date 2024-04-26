using Application.DTOs;
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

        public Task<ServiceResponse> updateProfile();
        public Task<ServiceResponse> AddAddress(AddressDTO addressDto);

    }
}
