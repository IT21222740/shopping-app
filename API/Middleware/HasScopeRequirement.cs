using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Middleware
{
    public class HasScopeRequirement:IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }
        public HasScopeRequirement(string scope,string issuer)
        {
            Scope = scope?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer?? throw new ArgumentNullException( nameof(issuer));
        }
    }
}
