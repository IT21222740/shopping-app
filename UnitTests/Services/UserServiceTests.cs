using Application.DTOs;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Azure;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Stripe.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IAuthentication> _mockAuthentication;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IRepository<Address>> _mockAddressRepository;
        private readonly Mock<IPamentService> _mockPamentService;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<IUserService>> _mockLogger;
        private readonly UserService userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockAuthentication = new Mock<IAuthentication>();
            _mockTokenService = new Mock<ITokenService>();
            _mockAddressRepository = new Mock<IRepository<Address>>();
            _mockPamentService = new Mock<IPamentService>();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<IUserService>>();

            userService = new UserService(
                _mockUserRepository.Object,
                _mockAuthentication.Object,
                _mockTokenService.Object,
                _mockPamentService.Object,
                _mockAddressRepository.Object,
                _mockEmailSender.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object
                );
        }

        [Fact]
        public async Task AddAddress_ReturnsSuccess_WhenSuccessfullInsert()
        {
            AddressDTO address = new AddressDTO
            {
                City = "sample",
                StreetName = "sample",
                PostalCode = 1234,
            };
            var userId = "s123";
            _mockTokenService.Setup(s => s.GetUserId()).Returns(userId);

            var response = await userService.AddAddress(address);

            Assert.NotNull(response);
            Assert.Equal("Add Address sucessFully", response.Message);
            Assert.True(response.Flag);
        }

        [Fact]
        public async Task SignUpAsync_ReturnsUnSuccess_WhenFailedToReg()
        {
            var sampleSignUpDTO = new SignUpDTO
            {
                Email = "john.doe@example.com",
                Password = "Password1!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Address = new AddressDTO
                {
                    StreetName = "sample",
                    City = "sample",
                    PostalCode = 1234
                    
                }
            };

            var sampleAuthDTO = new AuthDTO
            {
                Id = "abc123"
            };

            _mockAuthentication
                .Setup(auth=>auth.SingupAuthAsync(It.IsAny<string>(), It.IsAny<string>()));

             _mockPamentService
                .Setup(stripe=>stripe.RegisterUserToPayment(It.IsAny<string>(), It.IsAny<string>()));

            var response = await userService.SignUpAsync(sampleSignUpDTO);

            Assert.NotNull(response);
            Assert.False(response.Flag);
            Assert.Equal("User Registration Failed", response.Message);
        }

        [Fact]
        public async Task SignUpAsync_ReturnsUnSuccess_WhenUserIsNull()
        {
            var sampleSignUpDTO = new SignUpDTO
            {
                Email = "john.doe@example.com",
                Password = "Password1!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Address = new AddressDTO
                {
                    StreetName = "sample",
                    City = "sample",
                    PostalCode = 1234

                }
            };

            var sampleAuthDTO = new AuthDTO
            {
                Id = "abc123"
            };
            string stripeId = "s123";


            _mockAuthentication
                .Setup(auth => auth.SingupAuthAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sampleAuthDTO);

            _mockPamentService
               .Setup(stripe => stripe.RegisterUserToPayment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(stripeId);

            _mockUnitOfWork
                .Setup(u => u.BeginTransactionAsync());

            _mockUserRepository
                .Setup(u => u.CreateUser(It.IsAny<User>()));



            var response = await userService.SignUpAsync(sampleSignUpDTO);

            Assert.NotNull(response);
            Assert.False(response.Flag);
            Assert.Equal("User Registration Failed", response.Message);
        }

        [Fact]
        public async Task SignUpAsync_ReturnsSuccess_WhenUserCreated()
        {
            var sampleSignUpDTO = new SignUpDTO
            {
                Email = "john.doe@example.com",
                Password = "Password1!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Address = new AddressDTO
                {
                    StreetName = "sample",
                    City = "sample",
                    PostalCode = 1234

                }
            };

            var sampleAuthDTO = new AuthDTO
            {
                Id = "abc123"
            };
            string stripeId = "s123";

            ServiceResponse response = new ServiceResponse(Flag: true, Message: "sample");
           

            _mockAuthentication
                .Setup(auth => auth.SingupAuthAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sampleAuthDTO);

            _mockPamentService
               .Setup(stripe => stripe.RegisterUserToPayment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(stripeId);

            _mockUnitOfWork
                .Setup(u => u.BeginTransactionAsync());

            _mockUserRepository
                .Setup(u => u.CreateUser(It.IsAny<User>())).ReturnsAsync(response);

            _mockAddressRepository
                .Setup(addres => addres.Add(It.IsAny<Address>()));

            _mockEmailSender
                .Setup(e => e.ExecuteReg(It.IsAny<User>()));

            _mockUnitOfWork
                .Setup(u => u.CommitAsync());



            var singUpResponse = await userService.SignUpAsync(sampleSignUpDTO);

            Assert.NotNull(response);
            Assert.True(response.Flag);
            Assert.Equal("User Registration SuccessFull", singUpResponse.Message);
        }

        [Fact]
        public async Task SignUpAsync_ReturnsFailure_WhenExceptionThrown()
        {
            var sampleSignUpDTO = new SignUpDTO
            {
                Email = "john.doe@example.com",
                Password = "Password1!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Address = new AddressDTO
                {
                    StreetName = "sample",
                    City = "sample",
                    PostalCode = 1234

                }
            };

            var sampleAuthDTO = new AuthDTO
            {
                Id = "abc123"
            };
            string stripeId = "s123";

            ServiceResponse response = new ServiceResponse(Flag: true, Message: "sample");


            _mockAuthentication
                .Setup(auth => auth.SingupAuthAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sampleAuthDTO);

            _mockPamentService
               .Setup(stripe => stripe.RegisterUserToPayment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(stripeId);

            _mockUnitOfWork
                .Setup(u => u.BeginTransactionAsync());

            _mockUserRepository
                .Setup(u => u.CreateUser(It.IsAny<User>())).ThrowsAsync(new Exception("Simulated exception"));

            _mockUnitOfWork
                .Setup(t => t.RollbackAsync());

            // Act
            var signUpResponse = await userService.SignUpAsync(sampleSignUpDTO);

            // Assert
            Assert.NotNull(signUpResponse);
            Assert.False(signUpResponse.Flag);
            Assert.Equal("User Registration Failed", signUpResponse.Message);
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenSuccessLogin()
        {
            LoginResponse responeLogin = new LoginResponse() { 
                AcessToken = "sample"
            };

            LoginDTO loginDTO = new LoginDTO()
            {
                Email = "sample@123.com",
                Password = "Shift@123"
            };

            _mockAuthentication
                .Setup(auth => auth.AuthenticateUser(It.IsAny<LoginDTO>())).ReturnsAsync(responeLogin);

            var response = await userService.LoginAsync(loginDTO);

            Assert.Equal(responeLogin.AcessToken, response.AcessToken);
            Assert.NotNull(response);
            
        }

    }
}
