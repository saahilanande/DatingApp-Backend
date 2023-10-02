using API.Data;
using Microsoft.AspNetCore.Mvc;

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
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        return dataContext.Users.ToList();
    }

    //Endpoint to get a user by Id
    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int id)
    {
        return dataContext.Users.Find(id);
    }

}
