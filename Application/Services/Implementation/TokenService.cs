using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenService(IHttpContextAccessor http) {
            _contextAccessor = http;
        }

        public string? GetUserId()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            
            if(userId != null)
            {
                string[] splitIds = userId.Split("|");
                return splitIds[1];
            }
            else{
                return null;
            }

            
        }
    }
}