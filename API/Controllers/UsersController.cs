using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
            _context = context;
    }

    [AllowAnonymous]
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
