namespace MVC_EDM.Models;
using Microsoft.EntityFrameworkCore;
using MVC_EDM.Models;
#nullable disable
public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {

    }
    
    public DbSet<EmployeeProfile> EmployeeProfiles{get;set;}
    public DbSet<DeletedProfile> DeletedProfiles{get;set;}
    public DbSet<Request> Requests{get;set;}

    public object EmployeeProfile { get; set; }
}