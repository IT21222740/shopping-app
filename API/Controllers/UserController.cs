using Application.DTOs;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] SignUpDTO newUser)
        {
            try
            {
                var result = await _userService.SignUpAsync(newUser);
                return Ok(result);


            }
            catch (ErrorApiException ex)
            {
                // Extract error details from the exception
                var errorResponse = ex.ApiError;

                // Log the error for debugging purposes
                Console.WriteLine($"Auth0 API error: {errorResponse}");

                // Handle Auth0 API errors
                return BadRequest("Failed to sign up user: " + errorResponse.Message);
            }
            catch (ApiException ex)
            {
                // Handle Auth0 API errors
                return BadRequest("Failed to sign up user: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other errors
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {



                var auth0Domain = "dev-uh6xfalax4p08hsp.us.auth0.com";
                var auth0ClientId = "M4T99kSN2zG7aRvvnpBRvCMtZP4NSEHc";
                var auth0ClientSecret = "sT_i43dhesgULJ6lPxoERtmp9k5L_X_Yenh1OBHZQhOiwWH6HgMN2qPRNxrF2a0f";
                var audience = "https://people-service.com";

                var client = new AuthenticationApiClient(new Uri($"https://{auth0Domain}/"));

                var request = new ResourceOwnerTokenRequest
                {
                    ClientId = auth0ClientId,
                    ClientSecret = auth0ClientSecret,
                    Audience = audience,
                    Username = model.Email,
                    Password = model.Password,
                    Scope = "openid"
                };

                Console.WriteLine(request);

                var tokenResponse = await client.GetTokenAsync(request);
                Console.WriteLine(tokenResponse.AccessToken.ToString());
                Console.WriteLine(tokenResponse.IdToken.ToString());
                // If login successful, create JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(tokenResponse.AccessToken);


                var userInfo = await client.GetUserInfoAsync(tokenResponse.AccessToken);

                return Ok(new
                {
                    userInfo = userInfo,
                    AccessToken = tokenResponse.AccessToken,
                    IdToken = tokenResponse.IdToken,
                    Message = "User logged in successfully."
                });


            }
            catch (Exception ex)
            {
                // Log the exception

                return BadRequest("Invalid login attempt." + ex.Message);
            }

            



        }

    
        [HttpPost("addAddress")]
        [Authorize]
        public async Task<IActionResult> Address([FromBody] AddressDTO model)
        {
            var result = await _userService.AddAddress(model);
            if(result.Flag == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }


    }
}
