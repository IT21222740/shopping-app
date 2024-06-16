using Application.DTOs;
using Application.DTOs.User;
using Application.Interfaces;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly AuthConfiguration _aConfiguration;
        private  IAuthenticationApiClient _authenticationApiClient;
        
        public Authentication(IOptions<AuthConfiguration> options)
        {
            _aConfiguration = options.Value;
            _authenticationApiClient = new AuthenticationApiClient(new Uri($"https://{_aConfiguration.Domain}/"));
        }

        public void setAuthenticationClient(IAuthenticationApiClient authenticationApiClient)
        {
            _authenticationApiClient = authenticationApiClient;
        }

        public Task<AuthDTO> GetIdAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthDTO> SingupAuthAsync(string username, string password)
        {
            var signupRequest = new SignupUserRequest
            {
                ClientId = _aConfiguration.ClientId,
                Connection = _aConfiguration.Connection,
                Email = username,
                Password = password,
            };

            var authClient = _authenticationApiClient;

            try
            {
                var signupResponse = await authClient.SignupUserAsync(signupRequest);
                return new AuthDTO
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

        public async Task<LoginResponse> AuthenticateUser(LoginDTO user)
        {

            var client = _authenticationApiClient;

            var request = new ResourceOwnerTokenRequest
            {
                ClientId = _aConfiguration.ClientId,
                ClientSecret = _aConfiguration.ClientSecret,
                Audience = _aConfiguration.Audience,
                Username = user.Email,
                Password = user.Password,
                Scope = "openid"
            };

            Console.WriteLine(request);

            var tokenResponse = await client.GetTokenAsync(request);
           

            return new LoginResponse
            {
                AcessToken = tokenResponse.AccessToken.ToString(),
            };
        }
    }
}
