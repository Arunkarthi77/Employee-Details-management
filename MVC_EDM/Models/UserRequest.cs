using System.Collections.Generic;
namespace MVC_EDM.Models;

    public class UserRequsetViewModel
    {
        public IEnumerable<EmployeeProfile>  EmployeeProfileViewModel{get;set;}
        public IEnumerable<Request> RequestViewModel{get;set;}
    }
