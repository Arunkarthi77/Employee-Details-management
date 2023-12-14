using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace MVC_EDM.Models
{
    // [Bind(Exclude="ConfirmPassword")]
    public class Details
    {
        [Key]
        [Required]
        public string? EmployeeId { get; set; }
        [Required]
        [Display(Name ="EmployeeName")]
        public string? EmployeeName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must in between 8-15")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contains atleast 1 Lowercase, 1 Uppercase, 1 specialcase and 1 Numeric value")]
        public string? Password { get; set; }
        [Required]
         [HiddenInput(DisplayValue=false)]
        public string? Role { get; set; }
        [Required]
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? NewPassword { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must be same")]
        public string? ConfirmPassword { get; set; }
        public string? Gender{get;set;}

    }
}