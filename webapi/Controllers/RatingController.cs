using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Model;
namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly Application _context;

        public RatingController(Application context)
        {
            _context = context;
        }

        // GET: api/Rating
        [HttpGet]
        public async Task<IEnumerable<Rating>> GetFeedback()
        {
        //   if (_context.Feedback == null)
        //   {
        //       return NotFound();
        //   }
        //     return await _context.Feedback.ToListAsync();
            var listof = _context.Feedback.ToList();
            return listof;
        }

        [HttpPost]
        public async Task<bool> PutFeedback(Rating rating)
        {
            _context.Feedback.Add(rating);
            _context.SaveChanges();
            return true;
        }

    }
}
