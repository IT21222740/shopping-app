using Application.Services.Interfaces;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Implementation
{

    public class CheckoutService : ICheckoutService
    {
        private readonly ITokenService _tokenService;
        private readonly IPamentService _pamentService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserProduct> _userProducts;
        public CheckoutService(ITokenService tokenService, IPamentService pamentService,IRepository<User> userRepository,IRepository<UserProduct> userProdcuts) {
            _tokenService = tokenService;
            _pamentService = pamentService;
            _userRepository = userRepository;
            _userProducts = userProdcuts;
        }
        public async Task<string> checkout()
        {
            var userId = _tokenService.GetUserId();
            var User = await _userRepository.Get(u=>u.UserId == userId);
            var results = await _userProducts.GetAll(filter: up => up.userId == userId, includePropeties: "Product");
            var url =  await _pamentService.Checkout(User.StripeId,results.ToList());
            return url;
          
        }
    }

}

