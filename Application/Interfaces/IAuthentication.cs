using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthentication
    {
        Task<AuthSignupResponse> SingupAuthAsync(string username, string password);
    }
}
