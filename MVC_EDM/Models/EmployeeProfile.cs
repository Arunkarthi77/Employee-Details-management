using System;
using System.ComponentModel.DataAnnotations;
using MVC_EDM.Common;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace MVC_EDM.Models
{
    // [Bind(Exclude="addressline2")]
    public class EmployeeProfile
    {

        [Key]
        [Required]
        public String EmployeeId { get; set; }
        [Required]
        public string? EmployeeName { get; set; }
        [Required]
        [RegularExpression("^[6789][0-9]{9}$",ErrorMessage ="Mobile number is in incorrect format" )]
        public string? phonenumber { get; set; }

        public string? Designation { get; set; }

        [Required]
        public string? addressline1 { get; set; }
        [ScaffoldColumn(false)]
        public string? addressline2 { get; set; }
        [Required]
        public string? pincode { get; set; }
        [Required]
        public string? state { get; set; }
        [Required]
        public string? area { get; set; }
        [Required]
        public string? education { get; set; }
        [Required]
        public string? country { get; set; }

        public string? experience { get; set; }
        [Required]
        public string? team { get; set; }
        [Required]
        public string? hrpartner { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? mailid { get; set; }
        [Required]
        public string? Aadharnumber { get; set; }
        [Required]
        public string? TeamLead { get; set; }
        [Required]
        // [DisplayFormat(DataFormatString ="{0:d}",ApplyFormatInEditMode=true)]
        // [CurrentDate(ErrorMessage ="Please Enter a valid Date")]
        public string? dateofjoining { get; set; }
    //         public class CurrentDateAttribute : ValidationAttribute
    // {
    //     public override bool IsValid(object value)
    //     {
    //         DateTime dateTime=Convert.ToDateTime(value);
    //         return dateTime<=DateTime.Now;
           
    //     }
    // }
        [Required]
          
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
[BirthYear(ErrorMessage = "Please Enter a valid date")]
        public string? dateofbirth { get; set; }
        
public class BirthYearAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            
            var currentYear = DateTime.Now.Year;
            Console.WriteLine(currentYear);
            Console.WriteLine(date.Year);
            if (date.Year > currentYear)
            {
                return false;
            }
        }
        return true;
    }
}
        [Required]
        public string? Gender { get; set; }
     
        public byte[] image { get; set; }
        [Required]
        public int? salary { get; set; }

    }
}