using Application.DTOs;
using Application.Interfaces;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Domain.Entities;
using Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{

    public class AuthServiceTests
    {
        private readonly AuthConfiguration _configuration;
        private readonly Mock<IAuthenticationApiClient> _authenticationApiClientMock;
        private readonly Authentication _authentication;

        public AuthServiceTests()
        {
            IOptions<AuthConfiguration> _config = new OptionsWrapper<AuthConfiguration>(new AuthConfiguration
            {
                Domain = "dev-uh6xfalax4p08hsp.us.auth0.com",
                ClientId = "M4T99kSN2zG7aRvvnpBRvCMtZP4NSEHc",
                Connection = "Username-Password-Authentication",
                Audience = "https://people-service.com",
                ClientSecret = "sT_i43dhesgULJ6lPxoERtmp9k5L_X_Yenh1OBHZQhOiwWH6HgMN2qPRNxrF2a0f"
            });

            _authenticationApiClientMock = new Mock<IAuthenticationApiClient>();
            
            _authentication = new Authentication(_config);
            _authentication.setAuthenticationClient(_authenticationApiClientMock.Object);
        }

        [Fact]
        public async Task AuthenticateUser_ReturnsLoginResponse_WhenLoginSuccessful()
        {
            // Arrange
            var user = new LoginDTO
            {
                Email = "test@example.com",
                Password = "test123"
            };

            var tokenResponse = new AccessTokenResponse
            {
                AccessToken = "mock_access_token",
                IdToken = "mock_id_token"
            };

            var request = new ResourceOwnerTokenRequest
            {
                ClientId = "M4T99kSN2zG7aRvvnpBRvCMtZP4NSEHc",
                ClientSecret = "sT_i43dhesgULJ6lPxoERtmp9k5L_X_Yenh1OBHZQhOiwWH6HgMN2qPRNxrF2a0f",
                Audience = "https://people-service.com",
                Username = user.Email,
                Password = user.Password,
                Scope = "openid"
            };

            _authenticationApiClientMock
                 .Setup(auth => auth.GetTokenAsync(It.Is<ResourceOwnerTokenRequest>(r =>
                     r.ClientId == request.ClientId &&
                     r.ClientSecret == request.ClientSecret &&
                     r.Audience == request.Audience &&
                     r.Username == request.Username &&
                     r.Password == request.Password &&
                     r.Scope == request.Scope),
                     It.IsAny<CancellationToken>()))
                 .ReturnsAsync(tokenResponse);

            // Act
            var result = await _authentication.AuthenticateUser(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tokenResponse.AccessToken, result.AcessToken);
        }


        [Fact]
        public async Task SingupAuthAsync_ThrowsException_WhenSignUpUnSuccessful()
        {
            // Arrange
            var user = new LoginDTO
            {
                Email = "test@example.com",
                Password = "test123"
            };

            var sampleResponse = new SignupUserResponse
            {
                Id = "auth0|60d0fe4f5311236168a109ca",
                Email = "john.doe@example.com",
                EmailVerified = true
            };

            var request = new ResourceOwnerTokenRequest
            {
                ClientId = "M4T99kSN2zG7aRvvnpBRvCMtZP4NSEHc",
                ClientSecret = "sT_i43dhesgULJ6lPxoERtmp9k5L_X_Yenh1OBHZQhOiwWH6HgMN2qPRNxrF2a0f",
                Audience = "https://people-service.com",
                Username = user.Email,
                Password = user.Password,
                Scope = "openid"
            };

            _authenticationApiClientMock
                 .Setup(auth => auth.SignupUserAsync(It.IsAny<SignupUserRequest>(),
                     It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new Exception("Error"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _authentication.SingupAuthAsync("john.doe@example.com", "shift123"));
            Console.WriteLine(exception.Message);
            Assert.Equal("Error", exception.Message);

        }


        [Fact]
        public async Task SingupAuthAsync_ReturnsAuthDto_WhenSignUpSuccessful()
        {
            // Arrange
            var user = new LoginDTO
            {
                Email = "test@example.com",
                Password = "test123"
            };

            var sampleResponse = new SignupUserResponse
            {
                Id = "auth0|60d0fe4f5311236168a109ca", 
                Email = "john.doe@example.com",
                EmailVerified = true
            };

            var request = new ResourceOwnerTokenRequest
            {
                ClientId = "M4T99kSN2zG7aRvvnpBRvCMtZP4NSEHc",
                ClientSecret = "sT_i43dhesgULJ6lPxoERtmp9k5L_X_Yenh1OBHZQhOiwWH6HgMN2qPRNxrF2a0f",
                Audience = "https://people-service.com",
                Username = user.Email,
                Password = user.Password,
                Scope = "openid"
            };

            _authenticationApiClientMock
                 .Setup(auth => auth.SignupUserAsync(It.IsAny<SignupUserRequest>(),
                     It.IsAny<CancellationToken>()))
                 .ReturnsAsync(sampleResponse);

            // Act
            var result = await _authentication.SingupAuthAsync("john.doe@example.com","shift123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sampleResponse.Id, result.Id);
        }
    }

   
}

    
