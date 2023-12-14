using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_EDM.Models;
using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace MVC_EDM.Controllers;
[Log]

public class EmployeeController : Controller
{

    private readonly ApplicationDbContext _database;

    public EmployeeController(ApplicationDbContext database)
    {
        _database = database;
    }



    [HttpGet]

    public IActionResult Index()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            return View();
        }
        return RedirectToAction("Index", "Home");
    }
    

    [HttpGet]

    public IActionResult Request()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");

            string employeeId = HttpContext.Session.GetString("Session");
            var Employee = _database.EmployeeProfiles.Find(employeeId);
            Request request = new Request();
            request.EmployeeId = Employee.EmployeeId;
            request.EmployeeName = Employee.EmployeeName;


            return View(request);
        }
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public IActionResult Request(Request request)
    {
        _database.Requests.Add(request);
        _database.SaveChanges();
        return RedirectToAction("Index", "Employee");
    }
    

    [HttpGet]
    public IActionResult MyProfile()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            string employeeId = HttpContext.Session.GetString("Session");
            var employeeProfile = _database.EmployeeProfiles.Find(employeeId);

            // Console.WriteLine(employeeProfile.image[1]);
            if (employeeProfile == null)
            {
                return NotFound();
            }
            string imagestring = Convert.ToBase64String(employeeProfile.image);
            ViewBag.url = imagestring;

            return View(employeeProfile);
        }
        return RedirectToAction("Index", "Home");

    }

    public IActionResult Logout()
    {
        HttpContext.Session.SetString("Session", "");
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
     
    public IActionResult PutFeedback()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
             ViewBag.Session=HttpContext.Session.GetString("Session");
             return View();
        }
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public async Task<IActionResult> PutFeedback(Rating rating)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session=HttpContext.Session.GetString("Session");
            Console.WriteLine(rating.Feedback);
            rating.EmployeeId=HttpContext.Session.GetString("Session");
            HttpClient httpClient = new HttpClient();
            string apiUrl = "http://localhost:5143/api/Rating";

            var jsondata = JsonConvert.SerializeObject(rating);

            var data1 = new StringContent(jsondata, Encoding.UTF8, "application/json");

            var httpresponse = httpClient.PostAsync(apiUrl, data1);
            var result = await httpresponse.Result.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            if (result == "true")
            {
                ViewBag.Message = "Feedback submited Successful";
                return View("PutFeedback");
            }
            return View("Index");
        }
        return RedirectToAction("Index", "Home");
    }
}
