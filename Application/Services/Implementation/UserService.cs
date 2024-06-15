using Application.DTOs;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;


namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthentication authentication;
        private readonly ITokenService tokenService;
        private readonly IRepository<Address> addressRepository;
        private readonly IPamentService pamentService;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork  unitOfWork;
        private readonly ILogger<IUserService> _logger;

        public UserService(IUserRepository _userRepostory, IAuthentication _authentication, ITokenService _tokenService, IPamentService _pamentService,IRepository<Address> _addressRepository,IEmailSender _emailSender,IUnitOfWork _unitOfWork,ILogger<IUserService> logger)
        {
            userRepository = _userRepostory;
            authentication = _authentication;
            tokenService = _tokenService;
            addressRepository = _addressRepository;
            pamentService = _pamentService;
            emailSender = _emailSender;
            unitOfWork = _unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResponse> AddAddress(AddressDTO addressDto)
        {
            var currentUserId = tokenService.GetUserId();
            var address = new Address
            {
                
                StreetName = addressDto.StreetName,
                City = addressDto.City,
                PostalCode = addressDto.PostalCode,
                UserId = currentUserId,
                IsPrimary = false
            };
            
            await addressRepository.Add(address);
            _logger.LogInformation("Address sucessFully added Successfully {@Address}", address);
            return new ServiceResponse(true,"Add Address sucessFully",address);


        }

        public async Task<ServiceResponse> SignUpAsync(SignUpDTO newUser)
        {


            AuthDTO signupResponse = await authentication.SingupAuthAsync(newUser.Email, newUser.Password);
            _logger.LogInformation("User Signup wiht Third Party @{SignUp response}",signupResponse);

            var StripeId = await pamentService.RegisterUserToPayment(newUser.Email, newUser.FirstName);
            _logger.LogInformation("User resgister to Third Party payement service @{SignUp response}", signupResponse);


            if (signupResponse != null)
            {
                try
                {
                    await unitOfWork.BeginTransactionAsync();
                    var user = new User
                    {
                        UserId = signupResponse.Id,
                        Email = newUser.Email,
                        StripeId = StripeId,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        PhoneNumber = newUser.PhoneNumber,


                    };
                    var result = await userRepository.CreateUser(user);
                    _logger.LogInformation("User created in db @{Created User}", result);

                    if (result != null)
                    {

                        var address = new Address
                        {
                            StreetName = newUser.Address.StreetName,
                            City = newUser.Address.City,
                            PostalCode = newUser.Address.PostalCode,
                            UserId = user.UserId,
                            IsPrimary = true

                        };

                        await addressRepository.Add(address);
                        _logger.LogInformation("User created in db @{Address}", address);

                        await emailSender.ExecuteReg(user);
                        await unitOfWork.CommitAsync();

                        _logger.LogInformation("Successful Registration @{User}", result);
                        return new ServiceResponse(true, "User Registration SuccessFull");
                    }
                    else
                    {
                        _logger.LogInformation("User Registration Failed");
                        return new ServiceResponse(false, "User Registration Failed");
                    }

                }
                catch (Exception ex)
                {
                    await unitOfWork.RollbackAsync();

                    _logger.LogError("User Registration Failed. Rollback user registaraion");
                    return new ServiceResponse(false, "User Registration Failed",ex);
                }
                
            }
            else
            {
                _logger.LogInformation("User Registration Failed");
                return new ServiceResponse(false, "User Registration Failed");
            }

        }

        public async Task<LoginResponse> LoginAsync(LoginDTO user)
        {
            var response = await authentication.AuthenticateUser(user);
            _logger.LogInformation("User login to the system @{user}",response);
            return response;
        }
    }
}
