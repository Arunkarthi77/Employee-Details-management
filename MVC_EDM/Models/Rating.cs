using System.ComponentModel.DataAnnotations;

namespace MVC_EDM.Models{
public class Rating
    {
      
       
        public string  EmployeeId{get;set;}
         [Key]
        public string Feedback{get;set;}
    }  

}
