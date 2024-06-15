using Application.DTOs;
using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthentication
    {
        Task<AuthDTO> SingupAuthAsync(string username, string password);
        Task<LoginResponse> AuthenticateUser(LoginDTO user);



    }
}
