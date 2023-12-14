using System.ComponentModel.DataAnnotations;

namespace webapi.Model
{
    public class Rating
    {
      
        [Required]
        public string  EmployeeId{get;set;}
       [Required]
       [Key]
        public string Feedback{get;set;}
    }  
}