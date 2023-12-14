using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_EDM.Models;
using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MVC_EDM.Controllers;
[Log]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
       
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(Details emp)
    {
       
        // if(emp.EmployeeId==emp.EmployeeName)
        // {
        //     ModelState.AddModelError("EmployeeId","Id and Name shouldnot be same");
        // }
         ClaimsIdentity identity=null;
         bool IsAuthenticate=false;
        
        string result = Database.login(emp);
        if (result == "ok")
        {
        //     var role=Database.getRole(emp);
        //     identity=new ClaimsIdentity(new[]
        //     {
        //         new Claim(ClaimTypes.Name,emp.EmployeeName),
        //         new Claim(ClaimTypes.Role,role)
        //     },CookieAuthenticationDefaults.AuthenticationScheme);
        //     IsAuthenticate=true;
            
        
        //     if(IsAuthenticate)
        // {
        //      var Role=Database.getRole(emp);
        //     var principal= new ClaimsPrincipal(identity);
        //     var login1= HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        //     Console.WriteLine(Role);
        //     return RedirectToAction("Index",$"{Role}");
        // }
        
            int result1 = Database.getRole(emp);
            if (result1 == 1)
            {
                //Console.WriteLine("hi");
                 HttpContext.Session.SetString("Session", emp.EmployeeId);
                 CookieOptions cookieOptions=new CookieOptions();
                 cookieOptions.Expires=DateTime.Now.AddDays(1);
                 Response.Cookies.Append("LastLogin",DateTime.Now.ToString(),cookieOptions);
                 TempData["Cookies"]="Last Login :"+Request.Cookies["LastLogin"] ;
    
                return RedirectToAction("Index", "Employee");
            }
            else if (result1 == 2)
            {
                 HttpContext.Session.SetString("Session", emp.EmployeeName);
                 CookieOptions cookieOptions=new CookieOptions();
                 cookieOptions.Expires=DateTime.Now.AddDays(1);
                 Response.Cookies.Append("LastLogin",DateTime.Now.ToString(),cookieOptions);
                 TempData["Cookies"]="Last Login :"+Request.Cookies["LastLogin"] ;
            // Console.WriteLine($"..........{role}");
               return RedirectToAction("Index", "Admin");


            }

        }
        
         ViewBag.Message = "Account not found. Try Again!";
         return View("Login");


    }
    [HttpGet]

    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]

    public IActionResult ForgotPassword(Details emp)
    {

        string result = Database.forgotPassword(emp);
        if (result == "Correct")
        {
            string result1 = Database.resetPassword(emp);
            if (result1 == "done")
            {
                ViewBag.Message = "Password Changed";
                return View("Index");
            }
            else if (result1 == "format")
            {
                ViewBag.Message = "Password is in Incorrect Format";
                return View("ForgotPassword");
            }
            else if (result1 == "match")
            {
                ViewBag.Message = "Password Confirm Password Doesn't Match";
                return View("ForgotPassword");
            }


        }
        else
        {
            ViewBag.Message = "Employee Id or Employee Name Incorrect";
            return View("ForgotPassword");
        }
        return View("ForgotPassword");


    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
