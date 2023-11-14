﻿using System.ComponentModel.DataAnnotations;

namespace GGregator_Domain.DTOs.InDTOs
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}