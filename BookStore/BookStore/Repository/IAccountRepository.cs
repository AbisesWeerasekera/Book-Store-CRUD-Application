using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(SignInModel signInModel);
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
    }
}
