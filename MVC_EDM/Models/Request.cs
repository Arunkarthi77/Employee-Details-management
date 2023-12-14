using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVC_EDM.Models{
    public class Request
    {
  
   [Key]
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Sno{get;set;}
    [Required]
     public string?   EmployeeId{get;set;}
     [Required]
     public string? EmployeeName{get;set;}
     [Required]
     public string? Requestfor{get;set;}
     [Required]
     public string? Description{get;set;}

    }
}