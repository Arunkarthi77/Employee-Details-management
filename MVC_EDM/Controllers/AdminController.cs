using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_EDM.Models;
using System.Data.SqlClient;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Web.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace MVC_EDM.Controllers;

[Log]

public class AdminController : Controller
{

    private readonly ApplicationDbContext _database;
    public AdminController(ApplicationDbContext database)
    {
        _database = database;
    }



[HttpGet]
    public IActionResult Index()
    {

        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            Console.WriteLine(ViewBag.Session);
            return View();
        }
        return RedirectToAction("Login","Home");
    }


    public IActionResult Privacy()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            return View();
        }
        return RedirectToAction("Login", "Home");
    }
    

    [HttpGet]
    public IActionResult CreateEmployee()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            return View();
        }
        return RedirectToAction("Login", "Home");
    }
   
    [HttpPost]
    public IActionResult CreateEmployee(Details emp)
    {
        Console.WriteLine(emp.EmployeeId);

        string result = Database.createEmployee(emp);

        if (result == "Success")
        {
            ViewBag.Success = "Account Created Successfully";
            TempData["EmployeeId"] = emp.EmployeeId;
            TempData["EmployeeName"] = emp.EmployeeName;

            return RedirectToAction("Profile", "Admin");

        }
        else if (result == "Exists")
        {
            ViewBag.Message = "EmployeeId  Already Exists";
            return View("CreateEmployee");
        }
        else if (result == "Format")
        {
            ViewBag.Message = " Incorrect Password Format";
            return View("CreateEmployee");
        }
        ViewBag.Message = "Try Again";
        return View("CreateEmployee");


    }
   


    [HttpGet]
    public IActionResult Profile()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            EmployeeProfile employeeProfile = new EmployeeProfile();

            employeeProfile.EmployeeId = @TempData["EmployeeId"].ToString();
            Console.WriteLine(employeeProfile.EmployeeId);
            employeeProfile.EmployeeName = @TempData["EmployeeName"].ToString();

            return View(employeeProfile);
        }
        return RedirectToAction("Login","Home");
    }
   
    [HttpPost]

    public IActionResult Profile( EmployeeProfile employeeProfile)
    {
        

        
        foreach (var file in Request.Form.Files)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            employeeProfile.image = memoryStream.ToArray();

            memoryStream.Close();
            memoryStream.Dispose();

        }

        _database.EmployeeProfiles.Add(employeeProfile);
        _database.SaveChanges();
        return RedirectToAction("Index");
       

    }
    // [HttpGet]
    // public IActionResult SearchEmployee()
    // {
    //     if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
    //     {
    //         return View();
    //     }
    //     return RedirectToAction("Index", "Home");


    // }
    // [HttpPost]
    // public IActionResult SearchEmployee(EmployeeProfile employeeProfile)
    // {

    //     string employeeId = employeeProfile.EmployeeId;
    //     EmployeeProfile employeeprofile = _database.EmployeeProfiles.Find(employeeId);

    //     if (employeeprofile == null)
    //     {
    //         ViewBag.Message = "Employee Not Found";
    //         return View("SearchEmployee");
    //     }
    //     employeeprofile.image = null;
    //     return RedirectToAction("ViewProfile", "Admin", employeeprofile);
    // }

    // [HttpGet]
    // public IActionResult ViewProfile(EmployeeProfile employeeprofile)
    // {

    // }


    [HttpGet]
    public IActionResult ViewProfile(EmployeeProfile employeeprofile)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");

            EmployeeProfile temp = _database.EmployeeProfiles.Find(employeeprofile.EmployeeId);
            string imageBase64Data = Convert.ToBase64String(temp.image);
            // string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.url = imageBase64Data;
            HttpContext.Session.SetString("url", imageBase64Data);
            return View(temp);
        }
        return RedirectToAction("Index", "Home");

    }
    [HttpPost]
    public IActionResult UpdateProfile(EmployeeProfile employeeProfile)
    {
        Console.WriteLine(Request.Form.Files.Count);
        if (Request.Form.Files.Count == 0)
        {
            employeeProfile.image = Convert.FromBase64String(HttpContext.Session.GetString("url"));
            _database.EmployeeProfiles.Update(employeeProfile);
            _database.SaveChanges();
        }
        else
        {
            foreach (var file in Request.Form.Files)
            {
                MemoryStream memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                employeeProfile.image = memoryStream.ToArray();

                memoryStream.Close();
                memoryStream.Dispose();

            }
            _database.EmployeeProfiles.Update(employeeProfile);
            _database.SaveChanges();
        }


        // Console.WriteLine(employeeProfile.Aadharnumber);
        return RedirectToAction("Index", "Admin");
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



    // public IActionResult Employees()
    // {
    //     if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
    //     {
    //         IEnumerable<EmployeeProfile> employeesList = _database.EmployeeProfiles;

    //         return View(employeesList);
    //     }
    //     return RedirectToAction("Index", "Admin");
    // }


    public IActionResult EmployeeProfile(string EmployeeId)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            EmployeeProfile employeeprofile = _database.EmployeeProfiles.Find(EmployeeId);

            if (employeeprofile == null)
            {
                ViewBag.Message = "Employee Not Found";
                return View("SearchEmployee");
            }
            employeeprofile.image = null;
            return RedirectToAction("ViewProfile", "Admin", employeeprofile);
        }
        return RedirectToAction("Index", "Admin");

    }


    public IActionResult Delete(string EmployeeId)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            var employee = _database.EmployeeProfiles.Find(EmployeeId);
            _database.EmployeeProfiles.Remove(employee);
            _database.SaveChanges();
            string result = Database.deleteAccount(EmployeeId);
            if (result == "Success")
            {
                ViewBag.delete = "Account Deleted Successfully";
            }
            DeletedProfile deletedProfile = new DeletedProfile();
            deletedProfile.EmployeeId = employee.EmployeeId;
            deletedProfile.EmployeeName = employee.EmployeeName;
            deletedProfile.phonenumber = employee.phonenumber;
            deletedProfile.addressline1 = employee.addressline1;
            deletedProfile.addressline2 = employee.addressline2;
            deletedProfile.pincode = employee.pincode;
            deletedProfile.state = employee.state;
            deletedProfile.Designation = employee.Designation;
            deletedProfile.area = employee.area;
            deletedProfile.education = employee.education;
            deletedProfile.country = employee.country;
            deletedProfile.experience = employee.experience;
            deletedProfile.team = employee.team;
            deletedProfile.hrpartner = employee.hrpartner;
            deletedProfile.mailid = employee.mailid;
            deletedProfile.Aadharnumber = employee.Aadharnumber;
            deletedProfile.TeamLead = employee.TeamLead;
            deletedProfile.dateofbirth = employee.dateofbirth;
            deletedProfile.dateofjoining = employee.dateofjoining;
            deletedProfile.Gender = employee.Gender;
            deletedProfile.image = employee.image;
            deletedProfile.salary = employee.salary;
            AddProfile(deletedProfile);
            return RedirectToAction("Index", "Admin");
        }
        return RedirectToAction("Index", "Admin");

    }
    public void AddProfile(DeletedProfile deletedProfile)
    {
        _database.DeletedProfiles.Add(deletedProfile);
        _database.SaveChanges();
    }
    [HttpGet]
    public IActionResult FilterRemovedEmployees()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            var employeeprofile = _database.DeletedProfiles.ToList();
            int Count = employeeprofile.Count;
            ViewBag.employees = "Number of Employees:" + Count;
            return View(employeeprofile);
        }
        return RedirectToAction("Index", "Home");

    }

    [HttpPost]
    public IActionResult FilterRemovedEmployees(string s)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {

            string selectedvalue = Request.Form["selectedvalue"];
            string textvalue = Request.Form["textvalue"];

            if (selectedvalue == "EmployeeID")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.EmployeeId == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);
            }
            else if (selectedvalue == "EmployeeName")
            {
                var employeeprofile = _database.DeletedProfiles.Where(s => s.EmployeeName == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);
            }
            else if (selectedvalue == "Project Team")
            {
                var employeeprofile = _database.DeletedProfiles.Where(s => s.team == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);
            }
            else if (selectedvalue == "Hr Partner")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.hrpartner == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Designation")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.Designation == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Salary")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.salary == Convert.ToInt32(textvalue)).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "State")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.state == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Team Lead")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.TeamLead == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Experience")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.experience == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Gender" && textvalue == "Male")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.Gender == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Gender" && textvalue == "Female")
            {

                var employeeprofile = _database.DeletedProfiles.Where(s => s.Gender == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "null")
            {

                return View("Filterprofiles");

            }


        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult DeletedProfiles(string EmployeeId)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            DeletedProfile deletedprofile = _database.DeletedProfiles.Find(EmployeeId);

            if (deletedprofile == null)
            {
                ViewBag.Message = "Employee Not Found";
                return View("SearchEmployee");
            }
            deletedprofile.image = null;
            return RedirectToAction("DeletedEmployeeProfile", "Admin", deletedprofile);
        }
        return RedirectToAction("Index", "Admin");

    }
    [HttpGet]
    

    public IActionResult DeletedEmployeeProfile(DeletedProfile deletedprofile)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");

            DeletedProfile temp = _database.DeletedProfiles.Find(deletedprofile.EmployeeId);
            string imageBase64Data = Convert.ToBase64String(temp.image);
            // string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.url = imageBase64Data;
            HttpContext.Session.SetString("url", imageBase64Data);
            return View(temp);
        }
        return RedirectToAction("Index", "Admin");

    }



[HttpGet]
    public IActionResult Filterprofiles()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");

            var employeeprofile = _database.EmployeeProfiles.ToList();
            int Count = employeeprofile.Count;
            ViewBag.employees = "Number of Employees:" + Count;
            return View(employeeprofile);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Filterprofiles(string s)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            Console.WriteLine("hi");
            ViewBag.Session = HttpContext.Session.GetString("Session");

            string selectedvalue = Request.Form["selectedvalue"];
            string textvalue = Request.Form["textvalue"];

            Console.WriteLine(selectedvalue);

            if (selectedvalue == "EmployeeID")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.EmployeeId == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);
            }
            else if (selectedvalue == "EmployeeName")
            {
                var employeeprofile = _database.EmployeeProfiles.Where(s => s.EmployeeName == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);
            }
            else if (selectedvalue == "Project Team")
            {
                var employeeprofile = _database.EmployeeProfiles.Where(s => s.team == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);
            }
            else if (selectedvalue == "Hr Partner")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.hrpartner == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Designation")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.Designation == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Salary")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.salary == Convert.ToInt32(textvalue)).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "State")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.state == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Team Lead")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.TeamLead == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Experience")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.experience == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Gender" && textvalue == "Male")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.Gender == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "Gender" && textvalue == "Female")
            {

                var employeeprofile = _database.EmployeeProfiles.Where(s => s.Gender == textvalue).ToList();
                int Count = employeeprofile.Count;
                ViewBag.employees = "Number of Employees:" + Count;
                return View(employeeprofile);

            }
            else if (selectedvalue == "")
            {

                return View("Filterprofiles");

            }


        }
        return RedirectToAction("Index", "Home");
    }
    public IActionResult ViewRequests()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
             var user=_database.EmployeeProfiles.ToList();
            UserRequsetViewModel userRequsetViewModel=new UserRequsetViewModel();
            userRequsetViewModel.EmployeeProfileViewModel=user;
            // ViewBag.users=user;
            var request = _database.Requests.ToList();
            userRequsetViewModel.RequestViewModel=request;
            return View(userRequsetViewModel);
        }
        return RedirectToAction("Index", "Home");
    }
[HttpGet]
    public async Task<IActionResult> GetFeedback()
    {

        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
        
            HttpClient httpClient = new HttpClient();
            string apiUrl = ("http://localhost:5143/api/Rating");

            var responseofapi = httpClient.GetAsync(apiUrl).Result;

            var listoffeedback = responseofapi.Content.ReadAsAsync<IEnumerable<Rating>>().Result;
            return View(listoffeedback);
        }
        return RedirectToAction("Index", "Home");
    }
    //     public async Task<IActionResult> GetFeedback()
    // {
    //     HttpClient httpClient = new HttpClient();
    //     string apiUrl = "http://localhost:5143/api/Rating";

    //     var responseofapi = await httpClient.GetAsync(apiUrl);
    //     var listoffeedback = await responseofapi.Content.ReadFromJsonAsync<IEnumerable<Rating>>();

    //     return View(listoffeedback);
    // }

}
