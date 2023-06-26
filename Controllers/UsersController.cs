using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

    [Authorize] // this means that the user must be logged in to access this controller
    public class UsersController : BaseApiController
    {

        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous] //this means that the user does not have to be logged in to access this controller
        [HttpGet] // essentially we are defining the route here - think of flask routes (app.route('/users')
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync(); // Get a list of users from the database
            return users;


        }


        [HttpGet("{id}")] // we are defining the route here for one user - think of flask routes (app.route('/users/<id>')
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);

        }
    }
}