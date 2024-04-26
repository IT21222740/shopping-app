using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthentication authentication;
        private readonly ITokenService tokenService;
        private readonly IAddressRepository addressRepository;
        public UserService(IUserRepository _userRepostory, IAuthentication _authentication, ITokenService _tokenService, IAddressRepository _addressRepository)
        {
            userRepository = _userRepostory;
            authentication = _authentication;
            tokenService = _tokenService;
            addressRepository = _addressRepository;
        }

        public async Task<ServiceResponse> AddAddress(AddressDTO addressDto)
        {
            var currentUserId = tokenService.GetUserId();
            var address = new Address
            {
                
                StreetName = addressDto.StreetName,
                City = addressDto.City,
                PostalCode = addressDto.PostalCode,
                UserId = currentUserId
            };
            var result = await addressRepository.AddasyncAddress(address);
            return result;


        }

        public async Task<ServiceResponse> SignUpAsync(SignUpDTO newUser)
        {


            AuthDTO signupResponse = await authentication.SingupAuthAsync(newUser.Email, newUser.Password);


            if (signupResponse != null)
            {
                var user1 = new User
                {
                    UserId = signupResponse.Id,
                    Email = newUser.Email,

                };
                var result = await userRepository.CreateUser(user1);
                if (result != null)
                {
                    return new ServiceResponse(true, "added");
                }
                else
                {
                    return new ServiceResponse(false, "removed");
                }
               
            }
            else
            {
                return new ServiceResponse(false, "Failed");
            }

        }

        public Task<ServiceResponse> updateProfile()
        {
            throw new NotImplementedException();
        }
    }
}
