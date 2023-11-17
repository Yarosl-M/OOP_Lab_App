﻿using BCrypt.Net;
using AT_Domain.DTOs.OutDTOs;
using AT_Domain.Models;
using AT_Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AT_Infrastructure.Facades
{
    // если использовать паттерн Aggregate, то, видимо, только одному классу нужно
    // будет работать как с юзерами, так и подписками? (т. е. этот фасад объединить с
    // SubscriptionFacade) ну или как-то так хз крч
    public class AppAuthFacade : IAuthenticationFacade
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AppAuthFacade(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<SignedUpDTO?> Register(string username, string password)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existUser != null)
            {
                return null;
            }
            var newUser = new User
            {
                Username = username,
                CreatedAt = DateTime.UtcNow,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsAdmin = false,
                IsSubscribed = false,
            };

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            
            var userToken = GenerateToken(newUser.Username, newUser.Id.ToString(), newUser.IsAdmin);

            return new SignedUpDTO
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Token = userToken
            };
        }

        public async Task<LoggedInDTO?> Authenticate(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new LoggedInDTO
                {
                    Token = GenerateToken(username, user.Id.ToString(), user.IsAdmin)
                };
            }

            return null;
        }

        private string GenerateToken(string username, string id, bool isAdmin)
        {
            //var sampleToken = "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJHR3JlZ2F0b3IuQXV0aCIsIlVzZXJuYW1lIjoiVGVzdFVzZXIiLCJBdWRpZW5jZSI6IkdHcmVnYXRvci5BdXRoLkNsaWVudCIsImV4cCI6MTcwMjQxMTEyOSwiaWF0IjoxNjk5ODE5MTI5fQ.VVLmJ9wxi2hHDwBgOGF9GE6LCWDrPRKqj2EJaOACrcM";

            var keyString = _configuration.GetSection("JWT Bearer")
                .GetValue<string>("SymmetricSecurityKey");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration.GetSection("JWT Bearer")
                .GetValue<string>("Issuer");
            var audience = _configuration.GetSection("JWT Bearer")
                .GetValue<string>("Audience");

            var claims = new List<Claim>
            {
                new Claim("id", id),
                new Claim("name", username),
                new Claim("is_admin", isAdmin.ToString(), ClaimValueTypes.Boolean),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: TokenExpirationDate(DateTime.UtcNow),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        private DateTime TokenExpirationDate(DateTime timestamp )
        {
            return timestamp.AddDays(1997);
        }
    }
}