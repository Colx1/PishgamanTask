﻿using System.ComponentModel.DataAnnotations;

namespace PishgamanTask.Maui.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
