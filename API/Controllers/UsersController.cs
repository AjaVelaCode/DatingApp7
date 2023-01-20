using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]   // api/users
public class UsersController : ControllerBase
{
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
            _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        return _context.Users.ToList();   
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int Id)
    {
        return _context.Users.Find(Id);
    }
}