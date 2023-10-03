using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly DataContext dataContext;

    public UserController(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }


    //Endpoint to get all users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        return await dataContext.Users.ToListAsync();
    }

    //Endpoint to get a user by Id
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await dataContext.Users.FindAsync(id);
    }

    [HttpDelete("{id}")]

    public async Task<ActionResult> deleteUser(int id)
    {
        var user = await dataContext.Users.FindAsync(id);

        if (user != null)
        {
            dataContext.Users.Remove(user);
            dataContext.SaveChanges();
            return Ok(user);
        }

        return NotFound();
    }

}
