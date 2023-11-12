﻿using GGregator_Domain.DTOs;

namespace GGregator_Infrastructure.Facades
{
    public interface IAuthenticationFacade
    {
        public Task<SignedUpDTO?> Register(string username, string password);

        public Task<string?> Authenticate(string username, string password);
    }
}
