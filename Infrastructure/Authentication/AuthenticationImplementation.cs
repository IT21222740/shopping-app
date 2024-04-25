﻿using Application.DTOs;
using Application.Interfaces;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public class AuthenticationImplementation : IAuthentication
    {
        private readonly AuthConfiguration _aConfiguration;

        public AuthenticationImplementation(IOptions<AuthConfiguration> options)
        {
            _aConfiguration = options.Value;
        }

        public async Task<AuthSignupResponse> SingupAuthAsync(string username, string password)
        {
            var signupRequest = new SignupUserRequest
            {
                ClientId = _aConfiguration.clientId,
                Connection = _aConfiguration.connection,
                Email = username,
                Password = password,
            };

            var authClient = new AuthenticationApiClient(new Uri($"https://{_aConfiguration.Domain}/"));

            try
            {
                var signupResponse = await authClient.SignupUserAsync(signupRequest);
                return new AuthSignupResponse
                {
                    Id = signupResponse.Id,
                };

            }
            catch (ApiException ex)
            {
                
               Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
