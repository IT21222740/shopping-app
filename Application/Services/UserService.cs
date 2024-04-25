using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository user;
        private readonly IAuthentication authentication;
        public UserService(IUserRepository user,IAuthentication _authentication)
        {
            this.user = user;
            authentication = _authentication;
        }
        public async Task<ServiceResponse> SignUpAsync(SignUpDTO newUser)
        {
            

            AuthSignupResponse signupResponse = await authentication.SingupAuthAsync(newUser.Email, newUser.Password);


            if (signupResponse != null)
            {
                var user1 = new User
                {
                    Id = signupResponse.Id,
                    email = newUser.Email,

                };
                var result = await user.CreateUser(user1);
                return new ServiceResponse(true, "added");
            }
            else
            {
                return new ServiceResponse(false, "Failed");
            }

        }
    }
}
