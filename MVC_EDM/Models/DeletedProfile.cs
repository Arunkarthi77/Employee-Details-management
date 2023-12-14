using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_EDM.Models
{
    
    public class DeletedProfile
    {

        [Key]
        [Required]
        public String EmployeeId { get; set; }
        [Required]
        public string? EmployeeName { get; set; }
        [Required]
        public string? phonenumber { get; set; }

        public string? Designation { get; set; }

        [Required]
        public string? addressline1 { get; set; }
        public string? addressline2 { get; set; }
        [Required]
        [StringLength(6)]
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
        public string? mailid { get; set; }
        [Required]
        public string? Aadharnumber { get; set; }
        [Required]
        public string? TeamLead { get; set; }
        [Required]
        public string? dateofjoining { get; set; }
        [Required]
        public string? dateofbirth { get; set; }
        [Required]
        public string? Gender { get; set; }
       
        public byte[] image { get; set; }
        [Required]
        public int? salary { get; set; }

    }
}