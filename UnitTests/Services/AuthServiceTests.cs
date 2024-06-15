//using Auth0.AuthenticationApi;
//using Auth0.AuthenticationApi.Models;
//using Infrastructure.Authentication;
//using Microsoft.Extensions.Options;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UnitTests.Services
//{
//    public class AuthServiceTests
//    {
//        private readonly Mock<IOptions<AuthConfiguration>> _mockOptions;
//        private readonly Mock<AuthenticationApiClient> _mockauthenticationApiClient;
//        private readonly AuthConfiguration _authConfiguration;
//        private readonly Authentication _authentication;

//        public AuthServiceTests()
//        {
//            _authConfiguration = new AuthConfiguration
//            {
//                Domain = "test-domain",
//                ClientId = "test-client-id",
//                ClientSecret = "test-client-secret",
//                Connection = "test-connection",
//                Audience = "test-audience"

//            };

//            _mockOptions = new Mock<IOptions<AuthConfiguration>>();
//            _mockOptions
//                .Setup(o => o.Value).Returns(_authConfiguration);

//            _mockauthenticationApiClient = new Mock<AuthenticationApiClient>(new Uri($"https://{_authConfiguration.Domain}/"));
//            _authentication = new Authentication(_mockOptions.Object);
           
//        }


//        [Fact]
//        public async Task SingupAuthAsync_ShouldReturnAuthDTO_WhenSignupIsSuccessful()
//        {
//            // Arrange
//            var signupRequest = new SignupUserRequest
//            {
//                ClientId = _authConfiguration.ClientId,
//                Connection = _authConfiguration.Connection,
//                Email = "test@example.com",
//                Password = "password"
//            };

//            var signupResponse = new SignupUserResponse { Id = "test-id" };

//            _mockauthenticationApiClient.Setup(client => client.SignupUserAsync(signupRequest));

//            // Act
//            var result = await _authentication.SingupAuthAsync("test@example.com", "password");

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("test-id", result.Id);
//        }

//    }
//}
