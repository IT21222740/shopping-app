using Application.DTOs;
using Auth0.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user using Auth0.
        /// </summary>
        /// <param name="newUser">The registration details encapsulated in a RegisterDTO object.</param>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// User Login Using Auth0.
        /// </summary>
        /// <param name="model">The login details encapsulated in a LoginDTO object.</param>
        /// <returns>An IActionResult indicating the success or failure of the login attempt.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
             
            var response = await _userService.LoginAsync(model);
            if(response == null)
            {
                return BadRequest("User Login Failed");
            }
            else
            {
                return Ok(response);
            }

        }
       
    

        /// <summary>
        /// Adds a new address for the user.
        /// </summary>
        /// <param name="model">The address details encapsulated in an AddressDTO object.</param>
        /// <returns>An IActionResult indicating the success or failure of the address addition.</returns>
        [HttpPost("addAddress")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
