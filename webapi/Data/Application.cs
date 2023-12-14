using Microsoft.EntityFrameworkCore;
using webapi.Model;
namespace webapi.Data
{
    public class Application:DbContext
    {
      public   Application(DbContextOptions<Application> options): base(options)
      {

      }
      public DbSet<Rating> Feedback{get;set;}

    }
}